namespace citrus.Parsing.Builtins;

public static class ConsoleBuiltin
{
    public const string Input = "input";

    private static readonly IReadOnlyDictionary<string, TokenName> _map
        = new Dictionary<string, TokenName>
        {
            { Input, TokenName.Builtin_Console_Input }
        };

    public static IReadOnlyDictionary<string, TokenName> Map => _map;
}
