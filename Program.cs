namespace citrus;
using citrus.CLI;

public class Program
{
    public static int Main(string[] args)
    {
        return new CLIHost(args).Run();
    }
}