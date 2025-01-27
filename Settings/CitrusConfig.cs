namespace hayward.Settings;

public class KiwiConfig
{
    public bool PrintTokens { get; set; } = false;
    public bool PrintAST { get; set; } = false;
    public bool CreateNewFile { get; set; } = false;
    public List<string> Args { get; set; } = [];
    public List<string> Scripts { get; set; } = [];

    public static KiwiConfig Configure(IEnumerable<string> cliArgs)
    {
        KiwiConfig config = new();

        var iter = cliArgs.GetEnumerator();
        while (iter.MoveNext())
        {
            var current = iter.Current;

            switch (current.ToLower())
            {
                case "-n":
                case "--new":
                    if (config.Scripts.Count == 0)
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
                        Environment.Exit(0);
                    }
                    else
                    {
                        config.Args.Add(current);
                    }
                    break;

                case "-t":
                case "--tokens":
                    if (config.Scripts.Count == 0)
                    {
                        var filename = GetFilename(ref iter);
                        config.Scripts.Add(filename);
                        config.PrintTokens = true;
                    }
                    else
                    {
                        config.Args.Add(current);
                    }
                    break;

                case "-a":
                case "--ast":
                    if (config.Scripts.Count == 0)
                    {
                        var filename = GetFilename(ref iter);
                        config.Scripts.Add(filename);
                        config.PrintAST = true;
                    }
                    else
                    {
                        config.Args.Add(current);
                    }
                    break;

                case "-s":
                case "--safemode":
                    if (config.Scripts.Count == 0)
                    {
                        Kiwi.Settings.SafeMode = true;
                    }
                    else
                    {
                        config.Args.Add(current);
                    }
                    break;

                case "-ns":
                case "--no-stdlib":
                    if (config.Scripts.Count == 0)
                    {
                        Kiwi.Settings.StandardLibrary.Clear();
                    }
                    else
                    {
                        config.Args.Add(current);
                    }
                    break;

                case "-v":
                case "--version":
                    if (config.Scripts.Count == 0)
                    {
                        PrintVersion();
                        Environment.Exit(0);
                    }
                    else
                    {
                        config.Args.Add(current);
                    }
                    break;

                case "-h":
                case "--help":
                    if (config.Scripts.Count == 0)
                    {
                        PrintHelp();
                        Environment.Exit(0);
                    }
                    else
                    {
                        config.Args.Add(current);
                    }
                    break;

                default:
                    if (config.Scripts.Count == 0 && IsScript(ref current))
                    {
                        config.Scripts.Add(current);
                    }
                    else
                    {
                        config.Args.Add(current);
                    }
                    break;
            }
        }

        foreach (var env in Kiwi.Settings.EnvironmentVariables)
        {
            if (string.IsNullOrEmpty(env.Key))
            {
                continue;
            }

            Environment.SetEnvironmentVariable(env.Key, env.Value);
        }

        return config;
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

    private static void PrintVersion()
    {
        Console.WriteLine($"{Kiwi.Settings.Name} {Kiwi.Settings.Version}");
        Console.WriteLine();
    }

    private static void PrintHelp()
    {
        List<(string, string)> commands =
        [
            ("-h, --help", "print this message"),
            ("-v, --version", "print the current version"),
            ("-n, --new <file_path>", $"create a `{Kiwi.Settings.Extensions.Primary}` file"),
            ("-p, --parse <code>", "parse code as an argument"),
            ("-s, --safemode", "run in safemode"),
            ("-ns, --no-stdlib", "run without standard library"),
            ("-a, --ast <input_file_path>", $"print syntax tree of `{Kiwi.Settings.Extensions.Primary}` file"),
            ("-t, --tokenize <input_file_path>", "tokenize a file with the lexer"),
            ("-<key>=<value>", "specify an argument as a key-value pair")
        ];

        PrintVersion();

        Console.WriteLine($"Usage: {Kiwi.Settings.Name} [--flags] <script|args>");
        Console.WriteLine("Options:");

        foreach (var (flag, description) in commands)
        {
            Console.WriteLine($"  {flag,-40}{description}");
        }
        
        Console.WriteLine();
    }

    private static bool IsScript(ref string filename)
    {
        var original = filename;

        if (!Path.HasExtension(filename))
        {
            filename += Kiwi.Settings.Extensions.Primary;
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