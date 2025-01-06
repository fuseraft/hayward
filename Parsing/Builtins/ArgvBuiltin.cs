namespace citrus.Parsing.Builtins;

public static class ArgvBuiltin
{
    public const string GetArgv = "__argv__";
    public const string GetXarg = "__xarg__";

    private static readonly IReadOnlyDictionary<string, TokenName> _map
        = new Dictionary<string, TokenName>
        {
            { GetArgv, TokenName.Builtin_Argv_GetArgv },
            { GetXarg, TokenName.Builtin_Argv_GetXarg }
        };

    public static IReadOnlyDictionary<string, TokenName> Map => _map;
}
