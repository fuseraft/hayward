using hayward.Parsing;

namespace hayward.Tracing.Error;

public class TypeError(Token t, string message = "Unexpected type.")
    : KiwiError(t, "TypeError", message)
{
}