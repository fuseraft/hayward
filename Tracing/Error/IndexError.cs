using hayward.Parsing;

namespace hayward.Tracing.Error;

public class IndexError(Token t, string message = "Invalid index.")
    : HaywardError(t, "IndexError", message)
{
}