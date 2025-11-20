using hayward.Parsing;

namespace hayward.Tracing.Error;

public class EventError(Token t, string message = "An event error occurred.")
    : HaywardError(t, "EventError", message)
{
}