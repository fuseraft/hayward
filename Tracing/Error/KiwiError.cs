using citrus.Parsing;

namespace citrus.Tracing.Error;

public class CitrusError : Exception
{
    public const string DefaultErrorType = "CitrusError";

    public CitrusError()
    {
    }

    public CitrusError(Token token, string? message) : this(token, DefaultErrorType, message)
    {
    }

    public CitrusError(Token token, string type, string? message) : base(message)
    {
        Token = token;
        Type = type;
    }

    public Token Token { get; set; }

    public string Type { get; set; } = DefaultErrorType;
}