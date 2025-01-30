using hayward.Parsing;

namespace hayward.Tracing.Error;

public class FileReadError(Token t, string path)
    : HaywardError(t, "FileReadError", $"Failed to read file: '{path}'")
{
}