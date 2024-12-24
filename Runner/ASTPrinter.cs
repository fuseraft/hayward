namespace citrus.Runner;
using citrus.Parsing;

public class ASTPrinter : IRunner
{
    private const int SuccessReturnCode = 0;

    public ASTPrinter() {}

    public int Run(string script, List<string> args)
    {
        using Lexer lexer = new (0, script);
        var stream = lexer.GetTokenStream();

        Console.WriteLine($"Generating AST: {script}\n");
        var ast = new Parser().ParseTokenStream(stream, isScript: true);
        ast.Print(0);

        Console.WriteLine();

        return SuccessReturnCode;
    }
}