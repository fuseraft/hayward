namespace citrus.Runner;
using citrus.Parsing;

public class TokenPrinter : IRunner
{
    /// <summary>
    /// A success return code. A placeholder until a smarter mechanism is implemented.
    /// </summary>
    private const int SuccessReturnCode = 0;

    /// <summary>
    /// Prints the tokens of a given script.
    /// </summary>
    /// <param name="script">The script.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>Returns <c>0</c> for now.</returns>
    public int Run(string script, List<string> args)
    {
        Console.WriteLine($"Tokenizing: {script}\n");
        Console.WriteLine($"{"Token #",-10} {"Type",15}  {"Name",-15} {"Text",-20}");
        Console.WriteLine($"{"-------",-10} {"----",15}  {"----",-15} {"----",-20}");

        using Lexer lexer = new(0, script);
        var stream = lexer.GetTokenStream();
        var counter = 0;

        while (stream.CanRead)
        {
            var token = stream.Current();

            Console.WriteLine($"{++counter,-10} {token.Type,15}  {token.Name,-15} {token.Text,-20}");

            stream.Next();
        }

        Console.WriteLine();

        return SuccessReturnCode;
    }
}