using hayward.Parsing;

namespace hayward.Tracing.Error;

public class FileSystemError(Token t, string message = "A file system error occurred.")
    : KiwiError(t, "FileSystemError", message)
{
}