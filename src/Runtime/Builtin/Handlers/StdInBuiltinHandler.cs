using hayward.Parsing;
using hayward.Parsing.Keyword;
using hayward.Tracing.Error;
using hayward.Typing;

namespace hayward.Runtime.Builtin.Handlers;

public static class StdinBuiltinHandler
{
    private static readonly Lazy<StreamReader> _reader = new(
        () => new StreamReader(Console.OpenStandardInput(), leaveOpen: true),
        isThreadSafe: true
    );

    private static StreamReader Reader => _reader.Value;

    public static Value Execute(Token token, TokenName builtin, List<Value> args)
    {
        return builtin switch
        {
            TokenName.Builtin_Stdin_Read => Read(token, args),
            TokenName.Builtin_Stdin_ReadLine => ReadLine(token, args),
            TokenName.Builtin_Stdin_Lines => Lines(token, args),
            TokenName.Builtin_Stdin_Empty => Empty(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }

    private static Value Read(Token token, List<Value> args)
    {
        ParameterCountMismatchError.Check(token, StdInBuiltin.Read, 0, args.Count);

        var content = Reader.ReadToEnd();
        return Value.CreateString(content);
    }

    private static Value ReadLine(Token token, List<Value> args)
    {
        ParameterCountMismatchError.Check(token, StdInBuiltin.ReadLine, 0, args.Count);

        var line = Reader.ReadLine();
        return Value.CreateString(line ?? "");
    }

    private static Value Lines(Token token, List<Value> args)
    {
        ParameterCountMismatchError.Check(token, StdInBuiltin.Lines, 0, args.Count);

        List<Value> list = [];
        string? line;

        while ((line = Reader.ReadLine()) != null)
        {
            list.Add(Value.CreateString(line));
        }

        return Value.CreateList(list);
    }

    private static Value Empty(Token token, List<Value> args)
    {
        ParameterCountMismatchError.Check(token, StdInBuiltin.Empty, 0, args.Count);

        return Value.CreateBoolean(Reader.EndOfStream);
    }
}