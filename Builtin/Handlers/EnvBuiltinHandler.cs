using citrus.Parsing;
using citrus.Parsing.Builtins;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Builtin.Handlers;

public static class EnvBuiltinHandler
{
    public static Value Execute(Token token, TokenName builtin, List<Value> args)
    {
        return builtin switch
        {
            TokenName.Builtin_Env_GetAll => GetAll(token, args),
            TokenName.Builtin_Env_GetEnvironmentVariable => GetEnvironmentVariable(token, args),
            TokenName.Builtin_Env_SetEnvironmentVariable => SetEnvironmentVariable(token, args),
            TokenName.Builtin_Env_Kiwi => GetBinPath(token, args),
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
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
            throw new ParameterCountMismatchError(token, EnvBuiltin.Kiwi);
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