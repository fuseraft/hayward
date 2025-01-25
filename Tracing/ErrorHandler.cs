using citrus.Parsing;
using citrus.Settings;
using citrus.Tracing.Error;

namespace citrus.Tracing;

public static class ErrorHandler
{
    public static void PrintError(CitrusError e)
    {
        PrintError(e.Type, e.Message, e.Token);
    }

    public static void PrintError(Exception e, Token token)
    {
        PrintError("An unexpected error occurred", e.Message, token);
    }

    public static void PrintError(string type, string message, Token token)
    {
        var span = token.Span;
        var filePath = FileRegistry.Instance.GetFilePath(span.File);

        Console.Error.WriteLine($"{type}: {message}");
        Console.Error.WriteLine($"Near token: `{token.Type}`");
        Console.Error.WriteLine($"{filePath}:{span.Line}:{span.Pos}");
    }

    public static void DumpCrashLog(Exception? e)
    {
        List<string> lines = [$"Timestamp: {DateTime.Now:yyyy-MM-dd hh:mm:ss tt}"];

        while (e != null)
        {
            lines.Add($"Message: {e.Message}");
            lines.Add($"Stack: {e.StackTrace}");
            e = e.InnerException;
        }

        lines.Add(string.Empty);

        File.AppendAllLines(Citrus.Settings.CrashDumpPath, lines);

        Environment.Exit(1);
    }
}