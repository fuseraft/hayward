using hayward.Parsing;

namespace hayward.Tracing.Error;

public class DivideByZeroError(Token t, string message = "Division by zero.")
    : HaywardError(t, "DivideByZeroError", message)
{
}