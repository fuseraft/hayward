using hayward.Parsing;

namespace hayward.Tracing.Error;

public class FileSystemError(Token t, string message = "A file system error occurred.")
    : HaywardError(t, "FileSystemError", message)
{
}