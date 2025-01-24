using citrus.Parsing;

namespace citrus.Tracing.Error;

public class StructUndefinedError(Token t, string name)
    : CitrusError(t, "StructUndefinedError", $"The struct has not been declared: '{name}'")
{
}