using hayward.Parsing;

namespace hayward.Tracing.Error;

public class VariableUndefinedError(Token t, string name)
    : HaywardError(t, "VariableUndefinedError", $"The variable has not been declared: '{name}'")
{
}