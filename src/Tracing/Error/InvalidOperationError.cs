using hayward.Parsing;

namespace hayward.Tracing.Error;

public class InvalidOperationError(Token t, string message = "An invalid operation occurred.")
    : HaywardError(t, "InvalidOperationError", message)
{
}