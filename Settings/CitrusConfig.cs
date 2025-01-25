namespace citrus.Settings;

public class CitrusConfig
{
    public bool PrintTokens { get; set; } = false;
    public bool PrintAST { get; set; } = false;
    public bool PrintVersion { get; set; } = false;
    public bool CreateNewFile { get; set; } = false;
    public List<string> Args { get; set; } = [];
    public List<string> Scripts { get; set; } = [];

    public static CitrusConfig Configure(IEnumerable<string> cliArgs)
    {
        CitrusConfig config = new();

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

                case "-v":
                case "--version":
                    if (config.Scripts.Count == 0)
                    {
                        Console.WriteLine($"{Citrus.Settings.Name} {Citrus.Settings.Version}");
                        Console.WriteLine();
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

        foreach (var env in Citrus.Settings.EnvironmentVariables)
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

    private static bool IsScript(ref string filename)
    {
        var original = filename;

        if (!Path.HasExtension(filename))
        {
            filename += Citrus.Settings.Extensions.Primary;
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