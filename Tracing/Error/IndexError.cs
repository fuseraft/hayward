using citrus.Parsing;

namespace citrus.Tracing.Error;

public class IndexError(Token t, string message = "Invalid index.")
    : CitrusError(t, "IndexError", message)
{
}