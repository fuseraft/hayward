using hayward.Parsing;

namespace hayward.Tracing.Error;

public class RegexError(Token t, string message = "A regular expression error occurred.")
    : HaywardError(t, "Regex", message)
{
}