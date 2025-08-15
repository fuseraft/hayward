
using hayward.Settings;
using hayward.Tracing;
using hayward.Runtime.Runner;
using hayward.Tracing.Error;

namespace hayward;
public class Program
{
    public static int Main(string[] args)
    {
        var exitCode = 1;

        if (System.Diagnostics.Debugger.IsAttached)
        {
            args = [.. Hayward.Settings.Debug.CommandLineArguments];
        }
        
        try
        {
            var config = Config.Configure(args);
            var runner = GetRunner(config, new ScriptRunner(new()
            {
                CliArgs = ParseKeyValueArgs(config.Args),
            }));

            foreach (var script in config.Scripts)
            {
                _ = runner.Run(script, config.Args);
            }

            exitCode = 0;
        }
        catch (CliError e)
        {
            Console.WriteLine(e.Message);
        }
        catch (Exception e)
        {
            ErrorHandler.DumpCrashLog(e);
        }

        return exitCode;
    }

    private static IRunner GetRunner(Config config, IRunner runner)
    {
        if (config.PrintAST)
        {
            return new ASTPrinter();
        }
        else if (config.PrintTokens)
        {
            return new TokenPrinter();
        }
        else if (config.UseREPL)
        {
            return new REPLRunner(new());
        }

        return runner;
    }

    /// <summary>
    /// A helper method for parsing command-line arguments.
    /// </summary>
    /// <param name="args">Command-line arguments.</param>
    /// <returns>A dictionary.</returns>
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
                var key = parts[0];
                var value = parts[1];
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