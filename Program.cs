namespace citrus;
using citrus.Runner;

public class Program
{
    public static int Main(string[] args)
    {
        // args = ["/home/scs/GitHub/citrus/debug.kiwi"];
        var runner = new ScriptRunner();
        return new CLIHost(args).ExecuteCLI(runner);
    }
}

class CLIConfig
{
    public bool PrintTokens { get; set; } = false;
    public bool PrintAST { get; set; } = false;
}

class CLIHost(IEnumerable<string> cliArgs)
{
    private readonly IEnumerable<string> cliArgs = cliArgs;
    private readonly List<string> citrusArgs = [];
    private readonly List<string> citrusScripts = [];

    public int ExecuteCLI(IRunner runner)
    {
        try
        {
            var config = Configure();
            runner = GetRunner(config, runner);

            foreach (var script in citrusScripts)
            {
                _ = runner.Run(script, citrusArgs);
            }

            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(ex.StackTrace);
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
                    CreateNewFile(ref iter);
                    break;

                case "-t":
                case "--tokens":
                    PrintTokens(ref iter, config);
                    break;

                case "-a":
                case "--ast":
                    PrintAST(ref iter, config);
                    break;

                default:
                    if (IsScript(ref current))
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
        if (!Path.HasExtension(filename))
        {
            filename += ".kiwi";
        }

        return File.Exists(filename);
    }
}