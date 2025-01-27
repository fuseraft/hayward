using hayward.Parsing;

namespace hayward.Tracing.Error;

public class RangeError(Token t, string message = "Invalid range.")
    : KiwiError(t, "RangeError", message)
{
}