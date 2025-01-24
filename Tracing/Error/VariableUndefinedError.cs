using citrus.Parsing;

namespace citrus.Tracing.Error;

public class VariableUndefinedError(Token t, string name)
    : CitrusError(t, "VariableUndefinedError", $"The variable has not been declared: '{name}'")
{
}