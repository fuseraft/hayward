namespace citrus.Parsing.Builtins;

public static class TimeBuiltin
{
    public const string Hour  = "__hour__";
    public const string Minute = "__min__";
    public const string Second = "__sec__";
    public const string MonthDay = "__mday__";
    public const string WeekDay  = "__wday__";
    public const string YearDay  = "__yday__";
    public const string Month    = "__mon__";
    public const string Year     = "__year__";
    public const string EpochMilliseconds = "__epochms__";
    public const string Delay = "__delay__";
    public const string IsDST = "__isdst__";
    public const string Ticks = "__ticks__";
    public const string TicksToMilliseconds = "__ticksms__";
    public const string AMPM = "__ampm__";
    public const string Timestamp = "__timestamp__";
    public const string FormatDateTime = "__dtformat__";

    private static readonly IReadOnlyDictionary<string, TokenName> _map
        = new Dictionary<string, TokenName>
        {
            { Hour,                TokenName.Builtin_Time_Hour },
            { Minute,              TokenName.Builtin_Time_Minute },
            { Second,              TokenName.Builtin_Time_Second },
            { MonthDay,            TokenName.Builtin_Time_MonthDay },
            { WeekDay,             TokenName.Builtin_Time_WeekDay },
            { YearDay,             TokenName.Builtin_Time_YearDay },
            { Month,               TokenName.Builtin_Time_Month },
            { Year,                TokenName.Builtin_Time_Year },
            { EpochMilliseconds,   TokenName.Builtin_Time_EpochMilliseconds },
            { Delay,               TokenName.Builtin_Time_Delay },
            { IsDST,               TokenName.Builtin_Time_IsDST },
            { Ticks,               TokenName.Builtin_Time_Ticks },
            { TicksToMilliseconds, TokenName.Builtin_Time_TicksToMilliseconds },
            { AMPM,                TokenName.Builtin_Time_AMPM },
            { Timestamp,           TokenName.Builtin_Time_Timestamp },
            { FormatDateTime,      TokenName.Builtin_Time_FormatDateTime }
        };

    private static readonly IReadOnlySet<TokenName> _names = Map.Values.ToHashSet();

    public static IReadOnlyDictionary<string, TokenName> Map => _map;

    public static bool IsBuiltin(TokenName name) => _names.Contains(name);
}
