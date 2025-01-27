using hayward.Parsing;

namespace hayward.Tracing.Error;

public class ConversionError(Token t, string message = "A conversion error occurred.")
    : KiwiError(t, "ConversionError", message)
{
}