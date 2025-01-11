using citrus.Parsing;
using citrus.Parsing.Builtins;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Builtin.Handlers;

public static class TimeBuiltinHandler
{
    public static Value Execute(Token token, TokenName builtin, List<Value> args)
    {
        return builtin switch
        {
            TokenName.Builtin_Time_Ticks => ExecuteTicks(token, args),
            TokenName.Builtin_Time_TicksToMilliseconds => ExecuteTicksToMilliseconds(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text)
        };
    }

    private static Value ExecuteTicks(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.Ticks);
        }

        return Value.CreateInteger(DateTime.Now.Ticks);
    }

    private static Value ExecuteTicksToMilliseconds(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.TicksToMilliseconds);
        }

        if (!args[0].IsInteger())
        {
            throw new InvalidOperationError(token, "Expected an integer.");
        }

        var ticks = args[0].GetInteger();

        return Value.CreateInteger(TimeSpan.FromTicks(ticks).Milliseconds);
    }
}