using citrus.Builtin.Operation;
using citrus.Parsing;
using citrus.Parsing.Builtins;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Builtin.Handlers;

public static class ArgvBuiltinHandler
{
    public static Value Execute(Token token, TokenName builtin, List<Value> args, Dictionary<string, string> cliArgs)
    {
        return builtin switch
        {
            TokenName.Builtin_Argv_GetArgv => GetArgv(token, args, cliArgs),
            TokenName.Builtin_Argv_GetXarg => GetXarg(token, args, cliArgs),
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }

    private static Value GetArgv(Token token, List<Value> args, Dictionary<string, string> cliArgs)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, ArgvBuiltin.GetArgv);
        }

        List<Value> argv = [];

        foreach (var pair in cliArgs)
        {
            if (pair.Key.StartsWith("argv_"))
            {
                argv.Add(Value.CreateString(pair.Value));
            }
        }

        return Value.CreateList(argv);
    }

    private static Value GetXarg(Token token, List<Value> args, Dictionary<string, string> cliArgs)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, ArgvBuiltin.GetXarg);
        }

        var xargName = ConversionOp.GetString(token, args[0]);

        foreach (var pair in cliArgs)
        {
            if (pair.Key.Equals(xargName))
            {
                return Value.CreateString(pair.Value);
            }
        }

        return Value.EmptyString();
    }
}