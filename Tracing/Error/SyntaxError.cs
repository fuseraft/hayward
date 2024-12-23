using citrus.Parsing;

namespace citrus.Tracing.Error;

public class SyntaxError(Token token, string message = "A syntax error occurred.") : KiwiError(token, message)
{
}