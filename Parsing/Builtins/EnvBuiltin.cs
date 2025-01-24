namespace citrus.Parsing.Builtins;

public static class EnvBuiltin
{
    public const string OS                        = "__env_os__";
    public const string User                      = "__env_user__";
    public const string UserDomain                = "__env_userdomain__";
    public const string GetAll                    = "__env_getall__";
    public const string GetEnvironmentVariable    = "__env_getenv__";
    public const string SetEnvironmentVariable    = "__env_setenv__";
    public const string Citrus                    = "__env_bin__";
    public const string CitrusLib                 = "__env_lib__";

    private static readonly IReadOnlyDictionary<string, TokenName> _map
        = new Dictionary<string, TokenName>
        {
            { GetAll,                   TokenName.Builtin_Env_GetAll },
            { OS,                       TokenName.Builtin_Env_OS },
            { User,                     TokenName.Builtin_Env_User },
            { UserDomain,               TokenName.Builtin_Env_UserDomain },
            { GetEnvironmentVariable,   TokenName.Builtin_Env_GetEnvironmentVariable },
            { SetEnvironmentVariable,   TokenName.Builtin_Env_SetEnvironmentVariable },
            { Citrus,                   TokenName.Builtin_Env_Citrus },
            { CitrusLib,                TokenName.Builtin_Env_CitrusLib }
        };

    private static readonly IReadOnlySet<TokenName> _names = Map.Values.ToHashSet();

    public static IReadOnlyDictionary<string, TokenName> Map => _map;

    public static bool IsBuiltin(TokenName name) => _names.Contains(name);
}
