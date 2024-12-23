namespace citrus;
using citrus.Runner;

public class Program
{
    public static int Main(string[] args)
    {
        ScriptRunner runner = new ();
        args = ["/home/scs/GitHub/citrus/debug.kiwi"];
        return new CLIHost(args).ExecuteCLI(runner);
    }
}

class CLIHost(IEnumerable<string> cliArgs)
{
    private readonly IEnumerable<string> cliArgs = cliArgs;
    private List<string> citrusArgs = [];
    private List<string> citrusScripts = [];

    public int ExecuteCLI(IRunner runner)
    {
        try
        {
            HandleCommandLineArguments();
            return Execute(runner);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            Console.Error.WriteLine(ex.StackTrace);
            return 1;
        }
    }

    private int Execute(IRunner runner)
    {
        foreach (var script in citrusScripts)
        {
            runner.Run(script, citrusArgs);
        }

        return 0;
    }

    private void HandleCommandLineArguments()
    {
        var iter = cliArgs.GetEnumerator();
        while (iter.MoveNext())
        {
            var current = iter.Current;

            switch (current.ToLower())
            {
                case "-n":
                    CreateNewFile(ref iter);
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
    }

    private void CreateNewFile(ref IEnumerator<string> iter)
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

    private bool IsScript(ref string filename)
    {
        if (!Path.HasExtension(filename))
        {
            filename += ".kiwi";
        }

        return File.Exists(filename);
    }
}