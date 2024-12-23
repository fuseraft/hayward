using citrus.Parsing;

namespace citrus.Tracing.Error;

public class KiwiError : Exception
{
    public KiwiError()
    {
    }

    public KiwiError(Token token, string? message) : base(message)
    {
        Token = token;
    }

    public KiwiError(Token token, string? message, Exception? innerException) : base(message, innerException)
    {
        Token = token;
    }

    public Token Token { get; set; }
}