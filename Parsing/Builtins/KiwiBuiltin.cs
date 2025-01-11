namespace citrus.Parsing.Builtins;

using System.Collections.Generic;

public static class KiwiBuiltin
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
    public const string ToF          = "to_float";
    public const string ToI          = "to_int";
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

    //
    // 2) Put them into a read-only dictionary: string → TokenName
    //    (Mirroring your "SysBuiltin" pattern.)
    //

    private static readonly IReadOnlyDictionary<string, TokenName> _map
        = new Dictionary<string, TokenName>
        {
            { Append,       TokenName.Builtin_Kiwi_Append      },
            { Chars,        TokenName.Builtin_Kiwi_Chars       },
            { IsA,          TokenName.Builtin_Kiwi_IsA         },
            { Join,         TokenName.Builtin_Kiwi_Join        },
            { Split,        TokenName.Builtin_Kiwi_Split       },
            { RSplit,       TokenName.Builtin_Kiwi_RSplit      },
            { Substring,    TokenName.Builtin_Kiwi_Substring   },
            { LeftTrim,     TokenName.Builtin_Kiwi_LeftTrim    },
            { RightTrim,    TokenName.Builtin_Kiwi_RightTrim   },
            { Trim,         TokenName.Builtin_Kiwi_Trim        },
            { Size,         TokenName.Builtin_Kiwi_Size        },
            { ToF,          TokenName.Builtin_Kiwi_ToF         },
            { ToI,          TokenName.Builtin_Kiwi_ToI         },
            { ToS,          TokenName.Builtin_Kiwi_ToS         },
            { ToBytes,      TokenName.Builtin_Kiwi_ToBytes     },
            { ToHex,        TokenName.Builtin_Kiwi_ToHex       },
            { Type,         TokenName.Builtin_Kiwi_Type        },
            { Empty,        TokenName.Builtin_Kiwi_Empty       },
            { Replace,      TokenName.Builtin_Kiwi_Replace     },
            { RReplace,     TokenName.Builtin_Kiwi_RReplace    },
            { Reverse,      TokenName.Builtin_Kiwi_Reverse     },
            { Contains,     TokenName.Builtin_Kiwi_Contains    },
            { BeginsWith,   TokenName.Builtin_Kiwi_BeginsWith  },
            { EndsWith,     TokenName.Builtin_Kiwi_EndsWith    },
            { IndexOf,      TokenName.Builtin_Kiwi_IndexOf     },
            { LastIndexOf,  TokenName.Builtin_Kiwi_LastIndexOf },
            { Uppercase,    TokenName.Builtin_Kiwi_Uppercase   },
            { Lowercase,    TokenName.Builtin_Kiwi_Lowercase   },
            { Keys,         TokenName.Builtin_Kiwi_Keys        },
            { HasKey,       TokenName.Builtin_Kiwi_HasKey      },
            { Members,      TokenName.Builtin_Kiwi_Members     },
            { Push,         TokenName.Builtin_Kiwi_Push        },
            { Pop,          TokenName.Builtin_Kiwi_Pop         },
            { Enqueue,      TokenName.Builtin_Kiwi_Enqueue     },
            { Dequeue,      TokenName.Builtin_Kiwi_Dequeue     },
            { Shift,        TokenName.Builtin_Kiwi_Shift       },
            { Unshift,      TokenName.Builtin_Kiwi_Unshift     },
            { Clear,        TokenName.Builtin_Kiwi_Clear       },
            { Remove,       TokenName.Builtin_Kiwi_Remove      },
            { RemoveAt,     TokenName.Builtin_Kiwi_RemoveAt    },
            { Rotate,       TokenName.Builtin_Kiwi_Rotate      },
            { Insert,       TokenName.Builtin_Kiwi_Insert      },
            { Slice,        TokenName.Builtin_Kiwi_Slice       },
            { Concat,       TokenName.Builtin_Kiwi_Concat      },
            { Unique,       TokenName.Builtin_Kiwi_Unique      },
            { Count,        TokenName.Builtin_Kiwi_Count       },
            { Flatten,      TokenName.Builtin_Kiwi_Flatten     },
            { Zip,          TokenName.Builtin_Kiwi_Zip         },
            { Merge,        TokenName.Builtin_Kiwi_Merge       },
            { Values,       TokenName.Builtin_Kiwi_Values      },
            { Clone,        TokenName.Builtin_Kiwi_Clone       },
            { Pretty,       TokenName.Builtin_Kiwi_Pretty      },
            { Find,         TokenName.Builtin_Kiwi_Find        },
            { Match,        TokenName.Builtin_Kiwi_Match       },
            { Matches,      TokenName.Builtin_Kiwi_Matches     },
            { MatchesAll,   TokenName.Builtin_Kiwi_MatchesAll  },
            { Scan,         TokenName.Builtin_Kiwi_Scan        },
            { Set,          TokenName.Builtin_Kiwi_Set         },
            { Swap,         TokenName.Builtin_Kiwi_Swap        },
            { Get,          TokenName.Builtin_Kiwi_Get         },
            { First,        TokenName.Builtin_Kiwi_First       },
            { Last,         TokenName.Builtin_Kiwi_Last        },
            { Truthy,       TokenName.Builtin_Kiwi_Truthy      },
            { Lines,        TokenName.Builtin_Kiwi_Lines       },
            { Tokens,       TokenName.Builtin_Kiwi_Tokens      },
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
        if (ArgvBuiltin.Map.ContainsKey(arg))       return true;
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
        if (ArgvBuiltin.IsBuiltin(arg))       return true;
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
