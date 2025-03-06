using hayward.Parsing;
using hayward.Settings;
using hayward.Tracing.Error;

namespace hayward.Tracing;

public static class ErrorHandler
{
    public static void PrintError(HaywardError e)
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

        List<string> lines = 
        [
            $"Timestamp: {DateTime.Now:yyyy-MM-dd hh:mm:ss tt}", 
            $"{type}: {message}", 
            $"Near token: [Type=`{token.Type}`, Text=`{token.Text}`]", 
            $"{filePath}:{span.Line}:{span.Pos}"
        ];

        foreach (var line in lines.Skip(1))
        {
            Console.Error.WriteLine(line);
        }


        lines.Add(string.Empty);

        File.AppendAllLines(Hayward.Settings.CrashDumpPath, lines);
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

        File.AppendAllLines(Hayward.Settings.CrashDumpPath, lines);
        Console.Error.WriteLine($"Please check the log: {Hayward.Settings.CrashDumpPath}");

        Environment.Exit(1);
    }
}