namespace citrus.Parsing.Builtins;

public static class EnvBuiltin
{
    public const string GetEnvironmentVariable    = "__getenv__";
    public const string SetEnvironmentVariable    = "__setenv__";
    public const string UnsetEnvironmentVariable  = "__unsetenv__";
    public const string Kiwi                      = "__kiwi__";
    public const string KiwiLib                   = "__kiwilib__";

    private static readonly IReadOnlyDictionary<string, TokenName> _map
        = new Dictionary<string, TokenName>
        {
            { GetEnvironmentVariable,   TokenName.Builtin_Env_GetEnvironmentVariable },
            { SetEnvironmentVariable,   TokenName.Builtin_Env_SetEnvironmentVariable },
            { UnsetEnvironmentVariable, TokenName.Builtin_Env_UnsetEnvironmentVariable },
            { Kiwi,                     TokenName.Builtin_Env_Kiwi },
            { KiwiLib,                  TokenName.Builtin_Env_KiwiLib }
        };

    public static IReadOnlyDictionary<string, TokenName> Map => _map;
}
