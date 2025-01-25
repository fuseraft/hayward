using citrus.Parsing;
using citrus.Parsing.Builtins;
using citrus.Runtime;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Builtin.Handlers;

public class SerializerBuiltinHandler
{
    public static Value Execute(Token token, TokenName op, List<Value> args)
    {
        return op switch
        {
            TokenName.Builtin_Serializer_Deserialize => Deserialize(token, args),
            TokenName.Builtin_Serializer_Serialize => Serialize(token, args),
            _ => throw new InvalidOperationError(token, "Come back later."),
        };
    }

    private static Value Deserialize(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, SerializerBuiltin.Deserialize);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var input = args[0].GetString();
        Parser parser = new (true);
        using Lexer lexer = new (input, false);
        var stream = lexer.GetTokenStream();
        var ast = parser.ParseTokenStream(stream, true);

        Interpreter interpreter = new ();
        return interpreter.Interpret(ast);
    }

    private static Value Serialize(Token token, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, SerializerBuiltin.Deserialize);
        }

        return Value.CreateString(Serializer.Serialize(args[0]));
    }
}