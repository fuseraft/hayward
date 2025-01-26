using citrus.Builtin.Operation;
using citrus.Parsing;
using citrus.Parsing.Keyword;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Builtin.Handlers;

public static class EnvBuiltinHandler
{
    public static Value Execute(Token token, TokenName builtin, List<Value> args, Dictionary<string, string> cliArgs)
    {
        return builtin switch
        {
            TokenName.Builtin_Env_GetArgv => GetArgv(token, args, cliArgs),
            TokenName.Builtin_Env_GetXarg => GetXarg(token, args, cliArgs),
            TokenName.Builtin_Env_GetAll => GetAll(token, args),
            TokenName.Builtin_Env_GetEnvironmentVariable => GetEnvironmentVariable(token, args),
            TokenName.Builtin_Env_SetEnvironmentVariable => SetEnvironmentVariable(token, args),
            TokenName.Builtin_Env_Citrus => GetBinPath(token, args),
            TokenName.Builtin_Env_OS => OS(token, args),
            TokenName.Builtin_Env_User => User(token, args),
            TokenName.Builtin_Env_UserDomain => UserDomain(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }

    private static Value GetArgv(Token token, List<Value> args, Dictionary<string, string> cliArgs)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, EnvBuiltin.GetArgv);
        }

        Dictionary<Value, Value> argv = [];

        foreach (var pair in cliArgs)
        {
            argv[Value.CreateString(pair.Key)] = Value.CreateString(pair.Value);
        }

        return Value.CreateHashmap(argv);
    }

    private static Value GetXarg(Token token, List<Value> args, Dictionary<string, string> cliArgs)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, EnvBuiltin.GetXarg);
        }

        var xargName = ConversionOp.GetString(token, args[0]);

        foreach (var pair in cliArgs)
        {
            if (pair.Key.Equals(xargName))
            {
                return Value.CreateString(pair.Value);
            }
        }

        return Value.EmptyString();
    }

    private static Value OS(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, EnvBuiltin.OS);
        }

        return Value.CreateString(System.Runtime.InteropServices.RuntimeInformation.OSDescription);
    }

    private static Value User(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, EnvBuiltin.User);
        }

        return Value.CreateString(Environment.UserName);
    }

    private static Value UserDomain(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, EnvBuiltin.UserDomain);
        }

        return Value.CreateString(Environment.UserDomainName);
    }

    private static Value GetAll(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, EnvBuiltin.GetAll);
        }

        var envVars = Environment.GetEnvironmentVariables()
            .Cast<System.Collections.DictionaryEntry>()
            .ToDictionary(
                entry => Value.CreateString((string)entry.Key), 
                entry => Value.CreateString(entry.Value ?? string.Empty)
            );
        return Value.CreateHashmap(envVars);
    }

    private static Value GetBinPath(Token token, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, EnvBuiltin.Citrus);
        }

        var exePath = Environment.ProcessPath ?? throw new FileSystemError(token, "Could not get executable path.");
        return Value.CreateString(exePath);
    }

    private static Value GetEnvironmentVariable(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, EnvBuiltin.GetEnvironmentVariable);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var env = Environment.GetEnvironmentVariable(args[0].GetString()) ?? string.Empty;
        return Value.CreateString(env);
    }

    private static Value SetEnvironmentVariable(Token token, List<Value> args)
    {
        if (args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, EnvBuiltin.SetEnvironmentVariable);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        if (!args[1].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        Environment.SetEnvironmentVariable(args[0].GetString(), args[1].GetString());
        return Value.Default();
    }
}