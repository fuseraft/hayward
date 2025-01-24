namespace citrus.Runner;
using citrus.Parsing;
using citrus.Runtime;
using citrus.Tracing;
using citrus.Tracing.Error;

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
        using Lexer lexer = new(script);
        var stream = lexer.GetTokenStream();
        var ast = new Parser().ParseTokenStream(stream, isEntryPoint: true);
        
        Interpreter interpreter = new ();

        try
        {
            interpreter.Interpret(ast);
        }
        catch (CitrusError e)
        {
            ErrorHandler.PrintError(e);
        }
        catch (Exception e)
        {
            ErrorHandler.DumpCrashLog(e);
        }

        return SuccessReturnCode;
    }
}