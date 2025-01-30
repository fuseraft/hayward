using hayward.Parsing;

namespace hayward.Tracing.Error;

public class RangeError(Token t, string message = "Invalid range.")
    : HaywardError(t, "RangeError", message)
{
}