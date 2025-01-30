using hayward.Parsing;

namespace hayward.Tracing.Error;

public class HaywardError : Exception
{
    public const string DefaultErrorType = "HaywardError";

    public HaywardError()
    {
    }

    public HaywardError(Token token, string? message) : this(token, DefaultErrorType, message)
    {
    }

    public HaywardError(Token token, string type, string? message) : base(message)
    {
        Token = token;
        Type = type;
    }

    public Token Token { get; set; }

    public string Type { get; set; } = DefaultErrorType;
}