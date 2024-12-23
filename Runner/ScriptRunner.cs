namespace citrus.Runner;
using citrus.Parsing;

public class ScriptRunner : IRunner
{
    private const int V = 0;

    public ScriptRunner() {}

    public int Run(string script, List<string> args)
    {
        using Lexer lexer = new (0, script);
        var stream = lexer.GetTokenStream();
        var ast = new Parser().ParseTokenStream(stream, isScript: true);

        ast.Print(0);

        return V;
    }
}