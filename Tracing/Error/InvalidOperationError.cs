using citrus.Parsing;

namespace citrus.Tracing.Error;

public class InvalidOperationError(Token t, string message = "An invalid operation occurred.")
    : KiwiError(t, "InvalidOperationError", message)
{
}