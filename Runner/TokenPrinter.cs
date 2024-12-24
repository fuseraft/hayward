namespace citrus.Runner;
using citrus.Parsing;

public class TokenPrinter : IRunner
{
    private const int SuccessReturnCode = 0;

    public TokenPrinter() {}

    public int Run(string script, List<string> args)
    {
        Console.WriteLine($"Tokenizing: {script}\n");
        Console.WriteLine($"{"Token #",15}{"Type",20}{"Name",20}");
        Console.WriteLine($"{"-------",15}{"----",20}{"----",20}");
        
        using Lexer lexer = new (0, script);
        var stream = lexer.GetTokenStream();
        var counter = 0;

        while (stream.CanRead)
        {
            var token = stream.Current();

            Console.WriteLine($"{++counter,15}{token.Type,20}{token.Name,20}");

            stream.Next();
        }

        Console.WriteLine();

        return SuccessReturnCode;
    }
}