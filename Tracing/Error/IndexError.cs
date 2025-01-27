using hayward.Parsing;

namespace hayward.Tracing.Error;

public class IndexError(Token t, string message = "Invalid index.")
    : KiwiError(t, "IndexError", message)
{
}