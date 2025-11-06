using hayward.Parsing;
using hayward.Runtime.Builtin;
using hayward.Settings;
using hayward.Tracing;
using hayward.Tracing.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hayward.Runtime.Runner;

public class StdInRunner(Interpreter interpreter) : ScriptRunner(interpreter)
{
    /// <summary>
    /// Maximum size of input data streamed over input stream
    /// </summary>
    protected const long MaxInputBufferSize = 40 * 1024 * 1024;

    /// <summary>
    /// Runs the standard input as a script stream as the entrypoint to the program.
    /// </summary>
    /// <param name="script">The script.</param>
    /// <param name="args">The arguments.</param>
    /// <returns>Returns <c>0</c> for now.</returns>
    public override int Run(string script, List<string> args)
    {
        int res = SuccessReturnCode;
        try
        {
            var stdIn = Console.OpenStandardInput();
            var memoryStream = new MemoryStream();
            var size = BufferMemoryStream(ref memoryStream, stdIn, MaxInputBufferSize);
            if(size > 0)
            {
                using Lexer lexer = new(memoryStream);
                res = RunLexer(lexer);
            }
        }
        catch (HaywardError e)
        {
            ErrorHandler.PrintError(e);
        }
        catch (Exception e)
        {
            ErrorHandler.DumpCrashLog(e);
        }

        return res;
    }

    public long BufferMemoryStream(ref MemoryStream memoryStream, Stream stdIn, long maxBytes)
    {
        long res = 0;
        var buffer = new byte[1024];
        while(true)
        {
            int read = stdIn.Read(buffer, 0, buffer.Length);
            if (read == 0) break;

            res += read;
            if (res > maxBytes) throw new IOException($"Input exceeded maximum cap of {maxBytes} bytes.");
            for(int i=0; i < 1024; i++)
            {
                if (buffer[i] == 0) break;
                memoryStream.WriteByte(buffer[i]);
            }
        }
        memoryStream.Position = 0;

        return res;
    }
}
