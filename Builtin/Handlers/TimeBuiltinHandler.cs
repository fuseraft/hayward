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
            TokenName.Builtin_Time_AMPM => ExecuteAMPM(token, args),
            TokenName.Builtin_Time_Hour => ExecuteHour(token, args),
            TokenName.Builtin_Time_Minute => ExecuteMinute(token, args),
            TokenName.Builtin_Time_Second => ExecuteSecond(token, args),
            TokenName.Builtin_Time_Millisecond => ExecuteMillisecond(token, args),
            TokenName.Builtin_Time_Year => ExecuteYear(token, args),
            TokenName.Builtin_Time_Month => ExecuteMonth(token, args),
            TokenName.Builtin_Time_MonthDay => ExecuteMonthDay(token, args),
            TokenName.Builtin_Time_YearDay => ExecuteYearDay(token, args),
            TokenName.Builtin_Time_WeekDay => ExecuteWeekDay(token, args),
            TokenName.Builtin_Time_IsDST => ExecuteIsDST(token, args),
            TokenName.Builtin_Time_Now => ExecuteNow(token, args),
            TokenName.Builtin_Time_AddDay => ExecuteAddDay(token, args),
            TokenName.Builtin_Time_AddMonth => ExecuteAddMonth(token, args),
            TokenName.Builtin_Time_AddYear => ExecuteAddYear(token, args),
            TokenName.Builtin_Time_AddHour => ExecuteAddHour(token, args),
            TokenName.Builtin_Time_AddMinute => ExecuteAddMinute(token, args),
            TokenName.Builtin_Time_AddSecond => ExecuteAddSecond(token, args),
            TokenName.Builtin_Time_AddMillisecond => ExecuteAddMillisecond(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text)
        };
    }

    private static Value ExecuteAddHour(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsNumber()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddHour);
        }

        return Value.CreateDate(args[0].GetDate().AddHours(args[1].GetNumber()));
    }

    private static Value ExecuteAddMinute(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsNumber()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddMinute);
        }

        return Value.CreateDate(args[0].GetDate().AddMinutes(args[1].GetNumber()));
    }

    private static Value ExecuteAddSecond(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsNumber()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddSecond);
        }

        return Value.CreateDate(args[0].GetDate().AddSeconds(args[1].GetNumber()));
    }

    private static Value ExecuteAddMillisecond(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsNumber()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddMillisecond);
        }

        return Value.CreateDate(args[0].GetDate().AddMilliseconds(args[1].GetNumber()));
    }

    private static Value ExecuteAddDay(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsNumber()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddDay);
        }

        return Value.CreateDate(args[0].GetDate().AddDays(args[1].GetNumber()));
    }

    private static Value ExecuteAddMonth(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsInteger()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddMonth);
        }

        return Value.CreateDate(args[0].GetDate().AddMonths((int)args[1].GetInteger()));
    }

    private static Value ExecuteAddYear(Token token, List<Value> args)
    {
        if (args.Count != 2 || !(args[0].IsDate() && args[1].IsInteger()))
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.AddYear);
        }

        return Value.CreateDate(args[0].GetDate().AddYears((int)args[1].GetInteger()));
    }

    private static Value ExecuteNow(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, TimeBuiltin.Now);
        }

        return Value.CreateDate(DateTime.Now);
    }

    private static Value ExecuteIsDST(Token token, List<Value> args)
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

    private static Value ExecuteAMPM(Token token, List<Value> args)
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

    private static Value ExecuteHour(Token token, List<Value> args)
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

    private static Value ExecuteMinute(Token token, List<Value> args)
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

    private static Value ExecuteSecond(Token token, List<Value> args)
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

    private static Value ExecuteMillisecond(Token token, List<Value> args)
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

    private static Value ExecuteMonthDay(Token token, List<Value> args)
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

    private static Value ExecuteWeekDay(Token token, List<Value> args)
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

    private static Value ExecuteYearDay(Token token, List<Value> args)
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

    private static Value ExecuteMonth(Token token, List<Value> args)
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

    private static Value ExecuteYear(Token token, List<Value> args)
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

    private static Value ExecuteTicks(Token token, List<Value> args)
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