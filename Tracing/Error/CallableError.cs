using hayward.Parsing;

namespace hayward.Tracing.Error;

public class CallableError(Token t, string message = "Invalid callable.")
    : HaywardError(t, "CallableError", message)
{
}