using citrus.Parsing;
using citrus.Parsing.Keyword;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Runtime.Builtin.Handlers;

public static class SysBuiltinHandler
{
    public static Value Execute(Token token, TokenName builtin, List<Value> args)
    {
        return builtin switch
        {
            TokenName.Builtin_Sys_Exec => Exec(token, args),
            TokenName.Builtin_Sys_ExecOut => ExecOut(token, args),
            TokenName.Builtin_Sys_Open => Open(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }

    private static Value Open(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, SysBuiltin.Exec);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var process = new System.Diagnostics.Process
        {
            StartInfo = new System.Diagnostics.ProcessStartInfo
            {
                FileName = args[0].GetString()
            }
        };

        process.Start();

        return Value.Default();
    }

    private static Value Exec(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, SysBuiltin.Exec);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var startInfo = GetStartInfo(args[0].GetString());
        startInfo.CreateNoWindow = true;

        var process = new System.Diagnostics.Process
        {
            StartInfo = startInfo
        };

        process.Start();
        process.WaitForExit();

        return Value.CreateInteger(process.ExitCode);
    }

    private static Value ExecOut(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, SysBuiltin.ExecOut);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var startInfo = GetStartInfo(args[0].GetString());
        startInfo.RedirectStandardOutput = true;
        startInfo.RedirectStandardError = true;
        startInfo.CreateNoWindow = true;

        var process = new System.Diagnostics.Process
        {
            StartInfo = startInfo
        };

        process.Start();

        var output = process.StandardOutput.ReadToEnd();
        var error = process.StandardError.ReadToEnd();

        process.WaitForExit();

        Dictionary<Value, Value> execOut = [];
        execOut[Value.CreateString("stdout")] = Value.CreateString(output);
        execOut[Value.CreateString("stderr")] = Value.CreateString(error);

        return Value.CreateHashmap(execOut);
    }

    private static System.Diagnostics.ProcessStartInfo GetStartInfo(string command)
    {
        return new System.Diagnostics.ProcessStartInfo
        {
            FileName = OperatingSystem.IsWindows() ? "cmd.exe" : "bash",
            Arguments = (OperatingSystem.IsWindows() ? "/c " : "-c ") + command
        };
    }
}
