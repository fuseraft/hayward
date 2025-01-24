using citrus.Parsing;

namespace citrus.Tracing.Error;

public class InvalidContextError(Token t, string message = "Invalid object context.")
    : CitrusError(t, "InvalidContextError", message)
{
}