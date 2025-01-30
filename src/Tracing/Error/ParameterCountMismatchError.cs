using hayward.Parsing;
using System.Xml.Linq;

namespace hayward.Tracing.Error;

public class ParameterCountMismatchError(Token t, string name)
    : HaywardError(t, "ParameterCountMismatchError", $"The parameter count for function `{name}` does not match parameters passed.")
{
}