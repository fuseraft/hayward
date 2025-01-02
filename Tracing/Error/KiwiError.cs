using citrus.Parsing;

namespace citrus.Tracing.Error;

public class KiwiError : Exception
{
    public KiwiError()
    {
    }

    public KiwiError(Token token, string type, string? message) : base(message)
    {
        Token = token;
        Type = type;
    }

    public Token Token { get; set; }

    public string Type { get; set; } = "KiwiError";
}