
using System.Collections.Generic;

namespace citrus.Parsing.Keyword;
public static class CitrusBuiltin
{
    public const string Append       = "append";
    public const string Chars        = "chars";
    public const string IsA          = "is_a";
    public const string Join         = "join";
    public const string Split        = "split";
    public const string RSplit       = "rsplit";
    public const string Substring    = "substring";
    public const string LeftTrim     = "ltrim";
    public const string RightTrim    = "rtrim";
    public const string Trim         = "trim";
    public const string Size         = "size";
    public const string ToD          = "to_date";
    public const string ToF          = "to_float";
    public const string ToI          = "to_integer";
    public const string ToS          = "to_string";
    public const string ToBytes      = "to_bytes";
    public const string ToHex        = "to_hex";
    public const string Type         = "type";
    public const string Empty        = "empty";
    public const string Replace      = "replace";
    public const string RReplace     = "rreplace";
    public const string Reverse      = "reverse";
    public const string Contains     = "contains";
    public const string BeginsWith   = "begins_with";
    public const string EndsWith     = "ends_with";
    public const string IndexOf      = "index";
    public const string LastIndexOf  = "lastindex";
    public const string Uppercase    = "uppercase";
    public const string Lowercase    = "lowercase";
    public const string Keys         = "keys";
    public const string HasKey       = "has_key";
    public const string Members      = "members";
    public const string Push         = "push";
    public const string Pop          = "pop";
    public const string Enqueue      = "enqueue";
    public const string Dequeue      = "dequeue";
    public const string Shift        = "shift";
    public const string Unshift      = "unshift";
    public const string Clear        = "clear";
    public const string Remove       = "remove";
    public const string RemoveAt     = "remove_at";
    public const string Rotate       = "rotate";
    public const string Insert       = "insert";
    public const string Slice        = "slice";
    public const string Concat       = "concat";
    public const string Unique       = "unique";
    public const string Count        = "count";
    public const string Flatten      = "flatten";
    public const string Zip          = "zip";
    public const string Merge        = "merge";
    public const string Values       = "values";
    public const string Clone        = "clone";
    public const string Pretty       = "pretty";
    public const string Find         = "find";
    public const string Match        = "match";
    public const string Matches      = "matches";
    public const string MatchesAll   = "matches_all";
    public const string Scan         = "scan";
    public const string Set          = "set";
    public const string Swap         = "swap";
    public const string Get          = "get";
    public const string First        = "first";
    public const string Last         = "last";
    public const string Truthy       = "truthy";
    public const string Lines        = "lines";
    public const string Tokens       = "tokens";
    public const string Hour         = "hour";
    public const string Minute       = "minute";
    public const string Second       = "second";
    public const string Millisecond  = "millisecond";
    public const string Day          = "day";
    public const string Month        = "month";
    public const string Year         = "year";

    private static readonly IReadOnlyDictionary<string, TokenName> _map
        = new Dictionary<string, TokenName>
        {
            { Append,       TokenName.Builtin_Citrus_Append      },
            { Chars,        TokenName.Builtin_Citrus_Chars       },
            { IsA,          TokenName.Builtin_Citrus_IsA         },
            { Join,         TokenName.Builtin_Citrus_Join        },
            { Split,        TokenName.Builtin_Citrus_Split       },
            { RSplit,       TokenName.Builtin_Citrus_RSplit      },
            { Substring,    TokenName.Builtin_Citrus_Substring   },
            { LeftTrim,     TokenName.Builtin_Citrus_LeftTrim    },
            { RightTrim,    TokenName.Builtin_Citrus_RightTrim   },
            { Trim,         TokenName.Builtin_Citrus_Trim        },
            { Size,         TokenName.Builtin_Citrus_Size        },
            { ToD,          TokenName.Builtin_Citrus_ToDate      },
            { ToF,          TokenName.Builtin_Citrus_ToFloat     },
            { ToI,          TokenName.Builtin_Citrus_ToInteger   },
            { ToS,          TokenName.Builtin_Citrus_ToString    },
            { ToBytes,      TokenName.Builtin_Citrus_ToBytes     },
            { ToHex,        TokenName.Builtin_Citrus_ToHex       },
            { Type,         TokenName.Builtin_Citrus_Type        },
            { Empty,        TokenName.Builtin_Citrus_Empty       },
            { Replace,      TokenName.Builtin_Citrus_Replace     },
            { RReplace,     TokenName.Builtin_Citrus_RReplace    },
            { Reverse,      TokenName.Builtin_Citrus_Reverse     },
            { Contains,     TokenName.Builtin_Citrus_Contains    },
            { BeginsWith,   TokenName.Builtin_Citrus_BeginsWith  },
            { EndsWith,     TokenName.Builtin_Citrus_EndsWith    },
            { IndexOf,      TokenName.Builtin_Citrus_IndexOf     },
            { LastIndexOf,  TokenName.Builtin_Citrus_LastIndexOf },
            { Uppercase,    TokenName.Builtin_Citrus_Uppercase   },
            { Lowercase,    TokenName.Builtin_Citrus_Lowercase   },
            { Keys,         TokenName.Builtin_Citrus_Keys        },
            { HasKey,       TokenName.Builtin_Citrus_HasKey      },
            { Members,      TokenName.Builtin_Citrus_Members     },
            { Push,         TokenName.Builtin_Citrus_Push        },
            { Pop,          TokenName.Builtin_Citrus_Pop         },
            { Enqueue,      TokenName.Builtin_Citrus_Enqueue     },
            { Dequeue,      TokenName.Builtin_Citrus_Dequeue     },
            { Shift,        TokenName.Builtin_Citrus_Shift       },
            { Unshift,      TokenName.Builtin_Citrus_Unshift     },
            { Clear,        TokenName.Builtin_Citrus_Clear       },
            { Remove,       TokenName.Builtin_Citrus_Remove      },
            { RemoveAt,     TokenName.Builtin_Citrus_RemoveAt    },
            { Rotate,       TokenName.Builtin_Citrus_Rotate      },
            { Insert,       TokenName.Builtin_Citrus_Insert      },
            { Slice,        TokenName.Builtin_Citrus_Slice       },
            { Concat,       TokenName.Builtin_Citrus_Concat      },
            { Unique,       TokenName.Builtin_Citrus_Unique      },
            { Count,        TokenName.Builtin_Citrus_Count       },
            { Flatten,      TokenName.Builtin_Citrus_Flatten     },
            { Zip,          TokenName.Builtin_Citrus_Zip         },
            { Merge,        TokenName.Builtin_Citrus_Merge       },
            { Values,       TokenName.Builtin_Citrus_Values      },
            { Clone,        TokenName.Builtin_Citrus_Clone       },
            { Pretty,       TokenName.Builtin_Citrus_Pretty      },
            { Find,         TokenName.Builtin_Citrus_Find        },
            { Match,        TokenName.Builtin_Citrus_Match       },
            { Matches,      TokenName.Builtin_Citrus_Matches     },
            { MatchesAll,   TokenName.Builtin_Citrus_MatchesAll  },
            { Scan,         TokenName.Builtin_Citrus_Scan        },
            { Set,          TokenName.Builtin_Citrus_Set         },
            { Swap,         TokenName.Builtin_Citrus_Swap        },
            { Get,          TokenName.Builtin_Citrus_Get         },
            { First,        TokenName.Builtin_Citrus_First       },
            { Last,         TokenName.Builtin_Citrus_Last        },
            { Truthy,       TokenName.Builtin_Citrus_Truthy      },
            { Lines,        TokenName.Builtin_Citrus_Lines       },
            { Tokens,       TokenName.Builtin_Citrus_Tokens      },
            { Hour,         TokenName.Builtin_Citrus_Hour        },
            { Minute,       TokenName.Builtin_Citrus_Minute      },
            { Second,       TokenName.Builtin_Citrus_Second      },
            { Millisecond,  TokenName.Builtin_Citrus_Millisecond },
            { Day,          TokenName.Builtin_Citrus_Day         },
            { Month,        TokenName.Builtin_Citrus_Month       },
            { Year,         TokenName.Builtin_Citrus_Year        },
        };

    private static readonly IReadOnlySet<TokenName> _names = Map.Values.ToHashSet();
    public static IReadOnlyDictionary<string, TokenName> Map => _map;

    public static bool IsBuiltin(string arg)
    {
        if (ListBuiltin.Map.ContainsKey(arg))
        {
            return true;
        }

        return Map.ContainsKey(arg);
    }

    public static bool IsBuiltin(TokenName arg)
    {
        if (ListBuiltin.IsBuiltin(arg))
        {
            return true;
        }

        return _names.Contains(arg);
    }

    public static bool IsBuiltinMethod(string arg)
    {
        if (ConsoleBuiltin.Map.ContainsKey(arg))    return true;
        if (EnvBuiltin.Map.ContainsKey(arg))        return true;
        if (TimeBuiltin.Map.ContainsKey(arg))       return true;
        if (FileIOBuiltin.Map.ContainsKey(arg))     return true;
        if (MathBuiltin.Map.ContainsKey(arg))       return true;
        if (SysBuiltin.Map.ContainsKey(arg))        return true;
        if (HttpBuiltin.Map.ContainsKey(arg))       return true;
        if (WebServerBuiltin.Map.ContainsKey(arg))  return true;
        if (LoggingBuiltin.Map.ContainsKey(arg))    return true;
        if (EncoderBuiltin.Map.ContainsKey(arg))    return true;
        if (SerializerBuiltin.Map.ContainsKey(arg)) return true;
        if (FFIBuiltin.Map.ContainsKey(arg))        return true;
        if (ReflectorBuiltin.Map.ContainsKey(arg))  return true;
        if (SignalBuiltin.Map.ContainsKey(arg))     return true;
        if (SocketBuiltin.Map.ContainsKey(arg))     return true;
        if (TaskBuiltin.Map.ContainsKey(arg))       return true;

        // not a builtin method
        return false;
    }

    public static bool IsBuiltinMethod(TokenName arg)
    {
        if (ConsoleBuiltin.IsBuiltin(arg))    return true;
        if (EnvBuiltin.IsBuiltin(arg))        return true;
        if (TimeBuiltin.IsBuiltin(arg))       return true;
        if (FileIOBuiltin.IsBuiltin(arg))     return true;
        if (MathBuiltin.IsBuiltin(arg))       return true;
        if (SysBuiltin.IsBuiltin(arg))        return true;
        if (HttpBuiltin.IsBuiltin(arg))       return true;
        if (WebServerBuiltin.IsBuiltin(arg))  return true;
        if (LoggingBuiltin.IsBuiltin(arg))    return true;
        if (EncoderBuiltin.IsBuiltin(arg))    return true;
        if (SerializerBuiltin.IsBuiltin(arg)) return true;
        if (FFIBuiltin.IsBuiltin(arg))        return true;
        if (ReflectorBuiltin.IsBuiltin(arg))  return true;
        if (SignalBuiltin.IsBuiltin(arg))     return true;
        if (SocketBuiltin.IsBuiltin(arg))     return true;
        if (TaskBuiltin.IsBuiltin(arg))       return true;

        // not a builtin method
        return false;
    }
}
