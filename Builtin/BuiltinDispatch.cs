using citrus.Parsing;
using citrus.Parsing.Builtins;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Builtin;

public struct BuiltinDispatch
{

    public static Value Execute(Token token, TokenName builtin, Value v, List<Value> args)
    {
        if (KiwiBuiltin.IsBuiltin(builtin))
        {
            // return CoreBuiltinHandler::execute(token, builtin, v, args);
        }

        throw new FunctionUndefinedError(token, token.Text);
    }

    public static Value Execute(Token token, TokenName builtin, List<Value> args, Dictionary<string, string> cliArgs)
    {
        if (FileIOBuiltin.IsBuiltin(builtin))
        {
            // return FileIOBuiltinHandler::execute(token, builtin, args);
        }
        else if (TimeBuiltin.IsBuiltin(builtin))
        {
            // return TimeBuiltinHandler::execute(token, builtin, args);
        }
        else if (MathBuiltin.IsBuiltin(builtin))
        {
            // return MathBuiltinHandler::execute(token, builtin, args);
        }
        else if (EnvBuiltin.IsBuiltin(builtin))
        {
            // return EnvBuiltinHandler::execute(token, builtin, args);
        }
        else if (EncoderBuiltin.IsBuiltin(builtin))
        {
            // return EncoderBuiltinHandler::execute(token, builtin, args);
        }
        else if (ArgvBuiltin.IsBuiltin(builtin))
        {
            // return ArgvBuiltinHandler::execute(token, builtin, args, cliArgs);
        }
        else if (ConsoleBuiltin.IsBuiltin(builtin))
        {
            // return ConsoleBuiltinHandler::execute(token, builtin, args);
        }
        else if (SysBuiltin.IsBuiltin(builtin))
        {
            // return SysBuiltinHandler::execute(token, builtin, args);
        }
        else if (HttpBuiltin.IsBuiltin(builtin))
        {
            // return HttpBuiltinHandler::execute(token, builtin, args);
        }
        else if (LoggingBuiltin.IsBuiltin(builtin))
        {
            // return LoggingBuiltinHandler::execute(token, builtin, args);
        }


        throw new FunctionUndefinedError(token, token.Text);
    }

    /*
    public static Value Execute(ref FFIManager ffi, Token token, TokenName builtin, List<Value> args)
    {
        return FFIBuiltinHandler::execute(ffi, token, builtin, args);
    }

    public static Value Execute(ref SocketManager sockmgr, Token token, TokenName builtin, List<Value> args)
    {
        return NetBuiltinHandler::execute(sockmgr, token, builtin, args);
    }

    public static Value Execute(ref TaskManager taskmgr, Token token, TokenName builtin, List<Value> args)
    {
        return TaskBuiltinHandler::execute(taskmgr, token, builtin, args);
    }
    */
}