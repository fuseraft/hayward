using citrus.Parsing;

namespace citrus.Tracing.Error;

public class TokenStreamError(Token token, string message = "A token stream error occurred.") : KiwiError(token, message)
{
}