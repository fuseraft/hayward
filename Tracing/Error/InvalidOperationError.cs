using hayward.Parsing;

namespace hayward.Tracing.Error;

public class InvalidOperationError(Token t, string message = "An invalid operation occurred.")
    : KiwiError(t, "InvalidOperationError", message)
{
}