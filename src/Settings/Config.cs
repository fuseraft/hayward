using System.Text.Json;
using hayward.Tracing.Error;

namespace hayward.Settings;

public class Config
{
    public bool PrintTokens { get; set; } = false;
    public bool PrintAST { get; set; } = false;
    public List<string> Args { get; set; } = [];
    public List<string> Scripts { get; set; } = [];

    public static Config Configure(IEnumerable<string> cliArgs)
    {
        Config config = new();

        var iter = cliArgs.GetEnumerator();
        while (iter.MoveNext())
        {
            var current = iter.Current;

            switch (current.ToLower())
            {
                case "-s":
                case "--settings":
                    if (config.Scripts.Count == 0)
                    {
                        PrintSettings();
                    }
                    else
                    {
                        config.Args.Add(current);
                    }
                    break;

                case "-n":
                case "--new":
                    if (config.Scripts.Count == 0)
                    {
                        if (!iter.MoveNext())
                        {
                            throw new CliError($"Expected a filename after `{current}`.");
                        }

                        CreateNewFile(iter.Current);
                    }
                    else
                    {
                        config.Args.Add(current);
                    }
                    break;

                case "-p":
                case "--stdlib-path":
                    if (!iter.MoveNext())
                    {
                        throw new CliError($"Expected a path after `{current}`.");
                    }

                    Hayward.Settings.StandardLibrary = [new StandardLibraryPath
                    {
                        AutoLoad = true,
                        IncludeSubdirectories = true,
                        IsOverride = true,
                        Path = iter.Current
                    }];
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

                case "-sm":
                case "--safemode":
                    if (config.Scripts.Count == 0)
                    {
                        Hayward.Settings.SafeMode = true;
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
                        Hayward.Settings.StandardLibrary.Clear();
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

        foreach (var env in Hayward.Settings.EnvironmentVariables)
        {
            if (string.IsNullOrEmpty(env.Key))
            {
                continue;
            }

            Environment.SetEnvironmentVariable(env.Key, env.Value);
        }

        return config;
    }

    private static readonly JsonSerializerOptions s_writeOptions = new()
    {
        WriteIndented = true
    };

    private static void PrintSettings()
    {
        Console.WriteLine(JsonSerializer.Serialize(Hayward.Settings, s_writeOptions));
        Environment.Exit(0);
    }

    private static void CreateNewFile(string filename)
    {
        if (IsScript(ref filename))
        {
            throw new CliError($"The script already exists: {filename}");
        }

        if (!Path.HasExtension(filename))
        {
            filename += Hayward.Settings.Extensions.Primary;
        }

        using var fio = File.Create(filename);

        Console.WriteLine($"Created {filename}");
        Environment.Exit(0);
    }

    private static string GetFilename(ref IEnumerator<string> iter)
    {
        if (!iter.MoveNext())
        {
            throw new CliError("Expected a filename.");
        }

        var filename = iter.Current;
        if (!IsScript(ref filename))
        {
            throw new CliError($"The file does not exist: {filename}");
        }

        return filename;
    }

    private static void PrintVersion()
    {
        Console.WriteLine($"{Hayward.Settings.Name} {Hayward.Settings.Version}");
    }

    private static void PrintHelp()
    {
        List<(string, string)> commands =
        [
            ("-h, --help", "print this message"),
            ("-v, --version", "print current version"),
            ("-s, --settings", $"print {Hayward.Settings.Name} settings"),
            ("-a, --ast <input_path>", $"print syntax tree of `{Hayward.Settings.Extensions.Primary}` file (for debugging)"),
            ("-t, --tokenize <input_path>", $"print token stream of `{Hayward.Settings.Extensions.Primary}` file (for debugging)"),
            ("-n, --new <filename>", $"create a `{Hayward.Settings.Extensions.Primary}` file"),
            ("-ns, --no-stdlib", "run without standard library"),
            ("-sm, --safemode", "run in safemode"),
            ("-p, --stdlib-path", "run in safemode"),
            ("-<key>=<value>", "pass an argument to a program as a key-value pair")
        ];

        PrintVersion();

        Console.WriteLine($"Usage: {Hayward.Settings.Name} [--flags] <script|args>");
        Console.WriteLine("Options:");

        foreach (var (flag, description) in commands)
        {
            Console.WriteLine($"  {flag,-40}{description}");
        }
    }

    private static bool IsScript(ref string filename)
    {
        var original = filename;

        if (filename.StartsWith("~/"))
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var path = filename.Replace("~", home).Replace("//", "/");
            var dir = Path.GetDirectoryName(path) ?? throw new CliError($"Unable to retrieve parent path for: {path}");
            filename = Path.Combine([dir, Path.GetFileName(filename)]);
        }

        if (!Path.HasExtension(filename))
        {
            filename += Hayward.Settings.Extensions.Primary;
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