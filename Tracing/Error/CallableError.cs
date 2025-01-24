using citrus.Parsing;

namespace citrus.Tracing.Error;

public class CallableError(Token t, string message = "Invalid callable.")
    : CitrusError(t, "CallableError", message)
{
}