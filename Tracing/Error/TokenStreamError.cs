using hayward.Parsing;

namespace hayward.Tracing.Error;

public class TokenStreamError(Token t, string message = "A token stream error occurred.")
    : KiwiError(t, "TokenStreamError", message)
{
}