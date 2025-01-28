using hayward.Parsing;

namespace hayward.Tracing.Error;

public class TypeError(Token t, string message = "Unexpected type.")
    : HaywardError(t, "TypeError", message)
{
}