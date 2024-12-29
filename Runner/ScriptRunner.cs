namespace citrus.Runner;
using citrus.Parsing;

public class ScriptRunner : IRunner
{
    /// <summary>
    /// A success return code. A placeholder until a smarter mechanism is implemented.
    /// </summary>
    private const int SuccessReturnCode = 0;

    /// <summary>
    /// Runs a given script as the entrypoint to the program.
    /// </summary>
    /// <param name="script">The script.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>Returns <c>0</c> for now.</returns>
    public int Run(string script, List<string> args)
    {
        using Lexer lexer = new(0, script);
        var stream = lexer.GetTokenStream();
        var ast = new Parser().ParseTokenStream(stream, isEntryPoint: true);

        ast.Print(0);

        return SuccessReturnCode;
    }
}