namespace citrus.Runner;
using citrus.Parsing;
using citrus.Runtime;
using citrus.Tracing;
using citrus.Tracing.Error;

public class ScriptRunner : IRunner
{
    /// <summary>
    /// A success return code. A placeholder until a smarter mechanism is implemented.
    /// </summary>
    private const int SuccessReturnCode = 0;

    /// <summary>
    /// Runs a given script as the entrypoint to the program.
    /// </summary>
    /// <param name="script">The script.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>Returns <c>0</c> for now.</returns>
    public int Run(string script, List<string> args)
    {
        using Lexer lexer = new(script);
        var stream = lexer.GetTokenStream();
        var ast = new Parser().ParseTokenStream(stream, isEntryPoint: true);
        
        Interpreter interpreter = new ();

        try
        {
            interpreter.CliArgs = ParseKeyValueArgs(args);
            interpreter.Interpret(ast);
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

    private static Dictionary<string, string> ParseKeyValueArgs(List<string> args)
    {
        Dictionary<string, string> result = [];

        foreach (var arg in args)
        {
            string argWithoutPrefix;

            if (arg.StartsWith("--")) 
            {
                // e.g. "--key=value"
                argWithoutPrefix = arg[2..];
            }
            else if (arg.StartsWith('-') || arg.StartsWith('/'))
            {
                // e.g. "-key=value" or "/key=value"
                argWithoutPrefix = arg[1..];
            }
            else
            {
                // Doesn't match any known prefix, skip it
                result[arg] = arg;
                continue;
            }

            // Split on the first '='
            var parts = argWithoutPrefix.Split('=', 2, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length == 2)
            {
                // e.g. "key=value"
                string key = parts[0];
                string value = parts[1];
                result[key] = value;
            }
            else
            {
                // No '=' => treat as a boolean or a key-only argument
                // e.g. "/verbose" => (Key = "verbose", Value = "true")
                if (!string.IsNullOrWhiteSpace(argWithoutPrefix))
                {
                    result[argWithoutPrefix] = "true";
                }
            }
        }

        return result;
    }
}