namespace citrus.Runner;
using citrus.Parsing;

public class ScriptRunner : IRunner
{
    public ScriptRunner() {}

    public int Run(string script, List<string> args)
    {
        Parser parser = new ();
        using Lexer lexer = new (0, script);
        var stream = lexer.GetTokenStream();
        var ast = parser.ParseTokenStream(stream, isScript: true);

        ast.Print(0);

        return 1;
    }
}