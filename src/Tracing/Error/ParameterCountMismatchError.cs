using hayward.Parsing;

namespace hayward.Tracing.Error;

public class ParameterCountMismatchError : HaywardError
{
    public ParameterCountMismatchError(Token t, string name)
        : base(t, "ParameterCountMismatchError", $"The parameter count for function `{name}` does not match parameters passed.")
    {
    }

    public ParameterCountMismatchError(Token t, string name, int expectedCount)
        : base(t, "ParameterCountMismatchError", $"The parameter count for function `{name}` does not match parameters passed. Expected {expectedCount}.")
    {
    }

    public ParameterCountMismatchError(Token t, string name, int expectedCount, int actualCount)
        : base(t, "ParameterCountMismatchError", $"The parameter count for function `{name}` does not match parameters passed. Expected {expectedCount} but received {actualCount}.")
    {
    }
}
