namespace citrus.Parsing.Builtins;

public static class EnvBuiltin
{
    public const string GetAll                    = "__env_getall__";
    public const string GetEnvironmentVariable    = "__env_getenv__";
    public const string SetEnvironmentVariable    = "__env_setenv__";
    public const string UnsetEnvironmentVariable  = "__env_unsetenv__";
    public const string Kiwi                      = "__env_kiwi__";
    public const string KiwiLib                   = "__env_kiwilib__";

    private static readonly IReadOnlyDictionary<string, TokenName> _map
        = new Dictionary<string, TokenName>
        {
            { GetAll,                   TokenName.Builtin_Env_GetAll },
            { GetEnvironmentVariable,   TokenName.Builtin_Env_GetEnvironmentVariable },
            { SetEnvironmentVariable,   TokenName.Builtin_Env_SetEnvironmentVariable },
            { UnsetEnvironmentVariable, TokenName.Builtin_Env_UnsetEnvironmentVariable },
            { Kiwi,                     TokenName.Builtin_Env_Kiwi },
            { KiwiLib,                  TokenName.Builtin_Env_KiwiLib }
        };

    private static readonly IReadOnlySet<TokenName> _names = Map.Values.ToHashSet();

    public static IReadOnlyDictionary<string, TokenName> Map => _map;

    public static bool IsBuiltin(TokenName name) => _names.Contains(name);
}
