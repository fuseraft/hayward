using citrus.Parsing;

namespace citrus.Tracing.Error;

public class ConversionError(Token t, string message = "A conversion error occurred.")
    : KiwiError(t, message)
{
}