namespace citrus.Parsing.Builtins;

public static class SysBuiltin
{
    public const string EffectiveUserId = "__euid__";
    public const string Exec            = "__exec__";
    public const string ExecOut         = "__execout__";

    private static readonly IReadOnlyDictionary<string, TokenName> _map
        = new Dictionary<string, TokenName>
        {
            { EffectiveUserId, TokenName.Builtin_Sys_EffectiveUserId },
            { Exec,            TokenName.Builtin_Sys_Exec },
            { ExecOut,         TokenName.Builtin_Sys_ExecOut }
        };

    public static IReadOnlyDictionary<string, TokenName> Map => _map;
}
