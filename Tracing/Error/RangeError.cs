using citrus.Parsing;

namespace citrus.Tracing.Error;

public class RangeError(Token t, string message = "Invalid range.")
    : CitrusError(t, "RangeError", message)
{
}