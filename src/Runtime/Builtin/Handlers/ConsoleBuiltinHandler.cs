using hayward.Parsing;
using hayward.Parsing.Keyword;
using hayward.Tracing.Error;
using hayward.Typing;

namespace hayward.Runtime.Builtin.Handlers;
public static class ConsoleBuiltinHandler
{
    public static Value Execute(Token token, TokenName builtin, List<Value> args)
    {
        return builtin switch
        {
            TokenName.Builtin_Console_Input => Input(token, args),
            TokenName.Builtin_Console_Foreground => Foreground(token, args),
            TokenName.Builtin_Console_Background => Background(token, args),
            TokenName.Builtin_Console_Clear => Clear(token, args),
            TokenName.Builtin_Console_Reset => Reset(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }

    private static Value Clear(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, ConsoleBuiltin.Clear);
        }

        Console.Clear();

        return Value.Default;
    }

    private static Value Reset(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, ConsoleBuiltin.Clear);
        }

        Console.ResetColor();

        return Value.Default;
    }

    private static Value Input(Token token, List<Value> args)
    {
        if (args.Count != 0 && args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, ConsoleBuiltin.Input);
        }

        if (args.Count == 1)
        {
            if (!args[0].IsString())
            {
                throw new InvalidOperationError(token, "Expected a string.");
            }
            
            Console.Write(args[0].GetString());
        }

        var content = Console.ReadLine() ?? string.Empty;
        return Value.CreateString(content);
    }

    private static Value Foreground(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, ConsoleBuiltin.Foreground);
        }

        if (!args[0].IsInteger())
        {
            throw new InvalidOperationError(token, "Expected an integer.");
        }

        Console.ForegroundColor = (ConsoleColor)args[0].GetInteger();

        return Value.Default;
    }

    private static Value Background(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, ConsoleBuiltin.Background);
        }

        if (!args[0].IsInteger())
        {
            throw new InvalidOperationError(token, "Expected an integer.");
        }

        Console.BackgroundColor = (ConsoleColor)args[0].GetInteger();

        return Value.Default;
    }
}