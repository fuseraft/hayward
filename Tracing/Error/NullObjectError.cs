using hayward.Parsing;

namespace hayward.Tracing.Error;

public class NullObjectError(Token t, string message = "Object context is null.")
    : KiwiError(t, "NullObjectError", message)
{
}