using citrus.Parsing;

namespace citrus.Tracing.Error;

public class TokenStreamError(Token t, string message = "A token stream error occurred.")
    : CitrusError(t, "TokenStreamError", message)
{
}