namespace citrus.Runner;

using citrus.Parsing;
using citrus.Runtime;
using citrus.Settings;
using citrus.Tracing;
using citrus.Tracing.Error;

public class ScriptRunner(Interpreter interpreter) : IRunner
{
    /// <summary>
    /// A success return code. A placeholder until a smarter mechanism is implemented.
    /// </summary>
    private const int SuccessReturnCode = 0;

    /// <summary>
    /// Gets the local interpreter.
    /// </summary>
    private Interpreter Interpreter { get; } = interpreter;

    /// <summary>
    /// Gets or sets a flag indicating whether the standard library has been loaded.
    /// </summary>
    private bool StandardLibraryLoaded { get; set; } = false;

    /// <summary>
    /// Runs a given script as the entrypoint to the program.
    /// </summary>
    /// <param name="script">The script.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>Returns <c>0</c> for now.</returns>
    public int Run(string script, List<string> args)
    {
        try
        {
            Parser parser = new();

            List<TokenStream> streams = [];
            LoadStandardLibrary(ref streams);

            using Lexer lexer = new(script);
            streams.Add(lexer.GetTokenStream());
            var ast = parser.ParseTokenStreamCollection(streams);

            if (parser.HasError)
            {
                return 1;
            }

            Interpreter.Interpret(ast);
        }
        catch (CitrusError e)
        {
            ErrorHandler.PrintError(e);
        }
        catch (Exception e)
        {
            ErrorHandler.DumpCrashLog(e);
        }

        return SuccessReturnCode;
    }

    private void LoadStandardLibrary(ref List<TokenStream> streams)
    {
        if (StandardLibraryLoaded)
        {
            return;
        }

        List<string> paths = [];

        foreach (var library in Citrus.Settings.StandardLibrary)
        {
            if (!library.AutoLoad)
            {
                continue;
            }

            if (Directory.Exists(library.Path) && library.IncludeSubdirectories)
            {
                foreach (var path in Directory.EnumerateFiles(library.Path, "*.*"))
                {
                    if (IsRecognizedScript(path))
                    {
                        paths.Add(path);
                    }
                }
            }
            else if (IsRecognizedScript(library.Path))
            {
                paths.Add(library.Path);
            }
        }

        foreach (var path in paths)
        {
            using Lexer lexer = new(path);
            streams.Add(lexer.GetTokenStream());
        }

        StandardLibraryLoaded = true;
    }

    private static bool IsRecognizedScript(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        var ext = Path.GetExtension(path);
        return Citrus.Settings.Extensions.Recognized.Contains(ext);
    }
}