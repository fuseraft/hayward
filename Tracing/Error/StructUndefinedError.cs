using hayward.Parsing;

namespace hayward.Tracing.Error;

public class StructUndefinedError(Token t, string name)
    : HaywardError(t, "StructUndefinedError", $"The struct has not been declared: '{name}'")
{
}