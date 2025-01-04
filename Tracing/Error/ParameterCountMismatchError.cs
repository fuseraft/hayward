using citrus.Parsing;
using System.Xml.Linq;

namespace citrus.Tracing.Error;

public class ParameterCountMismatchError(Token t, string name)
    : KiwiError(t, "ParameterCountMismatchError", $"The parameter count for function `{name}` does not match parameters passed.")
{
}