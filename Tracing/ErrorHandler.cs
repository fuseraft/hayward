using citrus.Tracing.Error;

namespace citrus.Tracing;

public static class ErrorHandler
{
    public static void PrintError(KiwiError e)
    {
        Console.Error.WriteLine($"{e.Type}: {e.Message}");
        Console.Error.WriteLine($"Near token: `{e.Token.Type}`");

        var span = e.Token.Span;
        var filePath = FileRegistry.Instance.GetFilePath(span.File);
        Console.Error.WriteLine($"{filePath}:{span.Line}:{span.Pos}");
    }
}