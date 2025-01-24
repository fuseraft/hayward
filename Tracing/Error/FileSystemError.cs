using citrus.Parsing;

namespace citrus.Tracing.Error;

public class FileSystemError(Token t, string message = "A file system error occurred.")
    : CitrusError(t, "FileSystemError", message)
{
}