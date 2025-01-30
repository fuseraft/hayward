using hayward.Parsing;

namespace hayward.Tracing.Error;

public class SyntaxError(Token t, string message = "A syntax error occurred.")
    : HaywardError(t, "SyntaxError", message)
{
}