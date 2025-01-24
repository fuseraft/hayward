using citrus.Parsing;

namespace citrus.Tracing.Error;

public class PackageUndefinedError(Token t, string name)
    : CitrusError(t, "PackageUndefinedError", $"The package is undefined: '{name}'")
{
}