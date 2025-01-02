using citrus.Parsing;

namespace citrus.Tracing.Error;

public class FileReadError(Token t, string path)
    : KiwiError(t, "FileReadError", $"Failed to read file: '{path}'")
{
}