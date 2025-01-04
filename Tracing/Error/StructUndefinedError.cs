using citrus.Parsing;

namespace citrus.Tracing.Error;

public class StructUndefinedError(Token t, string name)
    : KiwiError(t, "StructUndefinedError", $"The struct has not been declared: '{name}'")
{
}