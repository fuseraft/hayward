using citrus.Tracing;
using citrus.Runner;

namespace citrus.CLI;

public class CLIHost(IEnumerable<string> cliArgs)
{
    const string Name = "citrus";
    const string Version = "1.0.0";

    private readonly IEnumerable<string> cliArgs = cliArgs;
    private readonly List<string> citrusArgs = [];
    private readonly List<string> citrusScripts = [];

    public int Run()
    {
        var runner = new ScriptRunner();
        return Run(runner);
    }

    private int Run(ScriptRunner runner)
    {
        try
        {
            var config = Configure();
            var runContext = GetRunner(config, runner);

            foreach (var script in citrusScripts)
            {
                _ = runContext.Run(script, citrusArgs);
            }

            return 0;
        }
        catch (Exception e)
        {
            ErrorHandler.DumpCrashLog(e);
            return 1;
        }
    }

    private CLIConfig Configure()
    {
        CLIConfig config = new();

        var iter = cliArgs.GetEnumerator();
        while (iter.MoveNext())
        {
            var current = iter.Current;

            switch (current.ToLower())
            {
                case "-n":
                case "--new":
                    if (citrusScripts.Count == 0)
                    {
                        CreateNewFile(ref iter);
                    }
                    else
                    {
                        citrusArgs.Add(current);
                    }
                    break;

                case "-t":
                case "--tokens":
                    if (citrusScripts.Count == 0)
                    {
                        PrintTokens(ref iter, config);
                    }
                    else
                    {
                        citrusArgs.Add(current);
                    }
                    break;

                case "-a":
                case "--ast":
                    if (citrusScripts.Count == 0)
                    {
                        PrintAST(ref iter, config);
                    }
                    else
                    {
                        citrusArgs.Add(current);
                    }
                    break;

                case "-v":
                case "--version":
                    if (citrusScripts.Count == 0)
                    {
                        PrintVersion();
                    }
                    else
                    {
                        citrusArgs.Add(current);
                    }
                    break;

                default:
                    if (citrusScripts.Count == 0 && IsScript(ref current))
                    {
                        citrusScripts.Add(current);
                    }
                    else
                    {
                        citrusArgs.Add(current);
                    }
                    break;
            }
        }

        return config;
    }

    private static void PrintVersion()
    {
        Console.WriteLine($"{Name} {Version}");
        Console.WriteLine();
        
        Environment.Exit(0);
    }

    private void PrintTokens(ref IEnumerator<string> iter, CLIConfig config)
    {
        var filename = GetFilename(ref iter);
        citrusScripts.Add(filename);
        config.PrintTokens = true;
    }

    private void PrintAST(ref IEnumerator<string> iter, CLIConfig config)
    {
        var filename = GetFilename(ref iter);
        citrusScripts.Add(filename);
        config.PrintAST = true;
    }

    private static IRunner GetRunner(CLIConfig config, IRunner runner)
    {
        if (config.PrintAST)
        {
            return new ASTPrinter();
        }
        else if (config.PrintTokens)
        {
            return new TokenPrinter();
        }

        return runner;
    }

    private static void CreateNewFile(ref IEnumerator<string> iter)
    {
        if (!iter.MoveNext())
        {
            throw new ArgumentException("Expected a filename after `new`.");
        }

        var filename = iter.Current;
        if (IsScript(ref filename))
        {
            throw new ArgumentException($"The script already exists: {filename}");
        }

        using var fs = File.Create(filename);

        Console.WriteLine($"Created {filename}");
    }

    private static string GetFilename(ref IEnumerator<string> iter)
    {
        if (!iter.MoveNext())
        {
            throw new ArgumentException("Expected a filename.");
        }

        var filename = iter.Current;
        if (!IsScript(ref filename))
        {
            throw new FileNotFoundException($"The file does not exist: {filename}");
        }

        return filename;
    }

    private static bool IsScript(ref string filename)
    {
        var original = filename;
        
        if (!Path.HasExtension(filename))
        {
            filename += ".kiwi";
        }

        if (File.Exists(filename))
        {
            return true;
        }
        else
        {
            filename = original;
        }

        return false;
    }
}