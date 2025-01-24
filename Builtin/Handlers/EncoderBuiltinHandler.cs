using citrus.Parsing;
using citrus.Parsing.Builtins;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Builtin.Handlers;

public static class EncoderBuiltinHandler
{
    public static Value Execute(Token token, TokenName builtin, List<Value> args)
    {
        return builtin switch
        {
            TokenName.Builtin_Encoder_Base64Decode => Base64Decode(token, args),
            TokenName.Builtin_Encoder_Base64Encode => Base64Encode(token, args),
            TokenName.Builtin_Encoder_UrlDecode => UrlDecode(token, args),
            TokenName.Builtin_Encoder_UrlEncode => UrlEncode(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }

    private static Value Base64Decode(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, EncoderBuiltin.Base64Decode);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var base64 = args[0].GetString();

        if (string.IsNullOrEmpty(base64))
        {
            return Value.CreateList([]);
        }

        var bytes = Convert.FromBase64String(base64).Select(x => Value.CreateInteger(x)).ToList();
        return Value.CreateList(bytes);
    }

    private static Value Base64Encode(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, EncoderBuiltin.Base64Encode);
        }

        if (!args[0].IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        List<byte> resultBytes = [];

        foreach (var item in args[0].GetList())
        {
            if (!item.IsInteger())
            {
                throw new InvalidOperationError(token, "Expected a list of integers.");
            }

            resultBytes.Add((byte)item.GetInteger());
        }

        var res = Convert.ToBase64String(resultBytes.ToArray());
        return Value.CreateString(res);
    }

    private static Value UrlDecode(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, EncoderBuiltin.UrlDecode);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var res = System.Web.HttpUtility.UrlDecode(args[0].GetString()) ?? string.Empty;
        return Value.CreateString(res);
    }

    private static Value UrlEncode(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, EncoderBuiltin.UrlEncode);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var res = System.Web.HttpUtility.UrlEncode(args[0].GetString()) ?? string.Empty;
        return Value.CreateString(res);
    }
}