using hayward.Parsing;

namespace hayward.Tracing.Error;

public class PackageUndefinedError(Token t, string name)
    : HaywardError(t, "PackageUndefinedError", $"The package is undefined: '{name}'")
{
}