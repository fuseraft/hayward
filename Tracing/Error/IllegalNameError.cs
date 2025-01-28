using hayward.Parsing;

namespace hayward.Tracing.Error;

public class IllegalNameError(Token t, string name)
    : HaywardError(t, "IllegalNameError", $"The following name cannot be declared: '{name}'")
{
}