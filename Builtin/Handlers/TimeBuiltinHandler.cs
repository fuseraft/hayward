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
            TokenName.Builtin_Time_Ticks => Ticks(token, args),
            TokenName.Builtin_Time_TicksToMilliseconds => TicksToMilliseconds(token, args),
            TokenName.Builtin_Time_AMPM => AMPM(token, args),
            TokenName.Builtin_Time_Hour => Hour(token, args),
            TokenName.Builtin_Time_Minute => Minute(token, args),
            TokenName.Builtin_Time_Second => Second(token, args),
            TokenName.Builtin_Time_Millisecond => Millisecond(token, args),
            TokenName.Builtin_Time_Year => Year(token, args),
            TokenName.Builtin_Time_Month => Month(token, args),
            TokenName.Builtin_Time_MonthDay => MonthDay(token, args),
            TokenName.Builtin_Time_YearDay => YearDay(token, args),
            TokenName.Builtin_Time_WeekDay => WeekDay(token, args),
            TokenName.Builtin_Time_IsDST => IsDST(token, args),
            TokenName.Builtin_Time_Now => Now(token, args),
            TokenName.Builtin_Time_AddDay => AddDay(token, args),
            TokenName.Builtin_Time_AddMonth => AddMonth(token, args),
            TokenName.Builtin_Time_AddYear => AddYear(token, args),
            TokenName.Builtin_Time_AddHour => AddHour(token, args),
            TokenName.Builtin_Time_AddMinute => AddMinute(token, args),
            TokenName.Builtin_Time_AddSecond => AddSecond(token, args),
            TokenName.Builtin_Time_AddMillisecond => AddMillisecond(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text)
        };
    }

    private static Value AddHour(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsNumber()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddHour);
        }

        return Value.CreateDate(args[0].GetDate().AddHours(args[1].GetNumber()));
    }

    private static Value AddMinute(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsNumber()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddMinute);
        }

        return Value.CreateDate(args[0].GetDate().AddMinutes(args[1].GetNumber()));
    }

    private static Value AddSecond(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsNumber()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddSecond);
        }

        return Value.CreateDate(args[0].GetDate().AddSeconds(args[1].GetNumber()));
    }

    private static Value AddMillisecond(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsNumber()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddMillisecond);
        }

        return Value.CreateDate(args[0].GetDate().AddMilliseconds(args[1].GetNumber()));
    }

    private static Value AddDay(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsNumber()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddDay);
        }

        return Value.CreateDate(args[0].GetDate().AddDays(args[1].GetNumber()));
    }

    private static Value AddMonth(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsInteger()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddMonth);
        }

        return Value.CreateDate(args[0].GetDate().AddMonths((int)args[1].GetInteger()));
    }

    private static Value AddYear(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsInteger()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddYear);
        }

        return Value.CreateDate(args[0].GetDate().AddYears((int)args[1].GetInteger()));
    }

    private static Value Now(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.Now);
        }

        return Value.CreateDate(DateTime.Now);
    }

    private static Value IsDST(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.IsDST);
        }

        if (args.Count == 0)
        {
            return Value.CreateBoolean(DateTime.Now.IsDaylightSavingTime());
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateBoolean(args[0].GetDate().IsDaylightSavingTime());
        }

        throw new InvalidOperationError(token);
    }

    private static Value AMPM(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AMPM);
        }

        if (args.Count == 0)
        {
            return Value.CreateString(DateTime.Now.ToString("tt"));
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateString(args[0].GetDate().ToString("tt"));
        }

        throw new InvalidOperationError(token);
    }

    private static Value Hour(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.Hour);
        }

        if (args.Count == 0)
        {
            return Value.CreateInteger(DateTime.Now.Hour);
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateInteger(args[0].GetDate().Hour);
        }

        throw new InvalidOperationError(token);
    }

    private static Value Minute(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.Minute);
        }

        if (args.Count == 0)
        {
            return Value.CreateInteger(DateTime.Now.Minute);
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateInteger(args[0].GetDate().Minute);
        }

        throw new InvalidOperationError(token);
    }

    private static Value Second(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.Second);
        }

        if (args.Count == 0)
        {
            return Value.CreateInteger(DateTime.Now.Second);
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateInteger(args[0].GetDate().Second);
        }

        throw new InvalidOperationError(token);
    }

    private static Value Millisecond(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.Millisecond);
        }

        if (args.Count == 0)
        {
            return Value.CreateInteger(DateTime.Now.Millisecond);
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateInteger(args[0].GetDate().Millisecond);
        }

        throw new InvalidOperationError(token);
    }

    private static Value MonthDay(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.MonthDay);
        }

        if (args.Count == 0)
        {
            return Value.CreateInteger(DateTime.Now.Day);
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateInteger(args[0].GetDate().Day);
        }

        throw new InvalidOperationError(token);
    }

    private static Value WeekDay(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.WeekDay);
        }

        if (args.Count == 0)
        {
            return Value.CreateInteger(DateTime.Now.DayOfWeek);
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateInteger(args[0].GetDate().DayOfWeek);
        }

        throw new InvalidOperationError(token);
    }

    private static Value YearDay(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.YearDay);
        }

        if (args.Count == 0)
        {
            return Value.CreateInteger(DateTime.Now.DayOfYear);
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateInteger(args[0].GetDate().DayOfYear);
        }

        throw new InvalidOperationError(token);
    }

    private static Value Month(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.Month);
        }

        if (args.Count == 0)
        {
            return Value.CreateInteger(DateTime.Now.Month);
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateInteger(args[0].GetDate().Month);
        }

        throw new InvalidOperationError(token);
    }

    private static Value Year(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.Year);
        }

        if (args.Count == 0)
        {
            return Value.CreateInteger(DateTime.Now.Year);
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateInteger(args[0].GetDate().Year);
        }

        throw new InvalidOperationError(token);
    }

    private static Value Ticks(Token token, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.Ticks);
        }

        if (args.Count == 0)
        {
            return Value.CreateInteger(DateTime.Now.Ticks);
        }
        else if (args.Count == 1 && args[0].IsDate())
        {
            return Value.CreateInteger(args[0].GetDate().Ticks);
        }

        throw new InvalidOperationError(token);
    }

    private static Value TicksToMilliseconds(Token token, List<Value> args)
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