using citrus.Parsing;

namespace citrus.Tracing.Error;

public class TypeError(Token t, string message = "Unexpected type.")
    : CitrusError(t, "TypeError", message)
{
}