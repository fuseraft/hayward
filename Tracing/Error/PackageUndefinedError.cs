using hayward.Parsing;

namespace hayward.Tracing.Error;

public class PackageUndefinedError(Token t, string name)
    : KiwiError(t, "PackageUndefinedError", $"The package is undefined: '{name}'")
{
}