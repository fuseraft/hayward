using hayward.Parsing;

namespace hayward.Tracing.Error;

public class CallableError(Token t, string message = "Invalid callable.")
    : KiwiError(t, "CallableError", message)
{
}