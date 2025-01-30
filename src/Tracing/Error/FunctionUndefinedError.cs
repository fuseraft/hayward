using hayward.Parsing;

namespace hayward.Tracing.Error;

public class FunctionUndefinedError(Token t, string name)
    : HaywardError(t, "FunctionUndefinedError", $"Function `{name}` is undefined.")
{
}