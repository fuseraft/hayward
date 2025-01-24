using citrus.Parsing;

namespace citrus.Tracing.Error;

public class SyntaxError(Token t, string message = "A syntax error occurred.")
    : CitrusError(t, "SyntaxError", message)
{
}