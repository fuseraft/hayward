using citrus.Parsing;

namespace citrus.Tracing.Error;

public class FunctionUndefinedError(Token t, string name)
    : KiwiError(t, "FunctionUndefinedError", $"Function `{name}` is undefined.")
{
}