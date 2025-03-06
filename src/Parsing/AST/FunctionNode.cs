using hayward.Typing;

namespace hayward.Parsing.AST;

public class FunctionNode : ASTNode
{
    public FunctionNode()
        : base(ASTNodeType.Function) { }

    public string Name { get; set; } = string.Empty;
    public List<KeyValuePair<string, ASTNode?>> Parameters { get; set; } = [];
    public List<ASTNode?> Body { get; set; } = [];
    public Dictionary<string, TokenName> TypeHints { get; set; } = [];
    public TokenName ReturnTypeHint { get; set; } = TokenName.Types_Any;
    public bool IsStatic { get; set; }
    public bool IsPrivate { get; set; }

    public override void Print(int depth = 0)
    {
        ASTTracer.PrintDepth(depth);
        Console.WriteLine($"Function: `{ASTTracer.Unmangle(Name)}`");

        if (IsStatic || IsPrivate)
        {
            ASTTracer.PrintDepth(1 + depth);
            Console.WriteLine("Modifiers:");
            ASTTracer.PrintDepth(2 + depth);
            Console.WriteLine($"Private: {IsPrivate}, Static: {IsStatic}");
        }

        ASTTracer.PrintDepth(1 + depth);
        Console.WriteLine($"ReturnType: {Serializer.GetTypenameString(ReturnTypeHint)}");

        ASTTracer.PrintDepth(1 + depth);
        Console.WriteLine("Parameters: ");
        foreach (var param in Parameters)
        {
            ASTTracer.PrintDepth(2 + depth);
            Console.WriteLine($"Identifier: `{ASTTracer.Unmangle(param.Key)}`");

            if (TypeHints.ContainsKey(param.Key))
            {
                ASTTracer.PrintDepth(3 + depth);
                var typeHint = TypeHints[param.Key];
                Console.WriteLine($"ParameterType: {Serializer.GetTypenameString(typeHint)}");
            }

            if (param.Value == null)
            {
                continue;
            }

            ASTTracer.PrintDepth(2 + depth);
            Console.WriteLine("Default: ");
            param.Value.Print(3 + depth);
        }

        ASTTracer.PrintDepth(1 + depth);
        Console.WriteLine("Statements:");
        foreach (var stmt in Body)
        {
            stmt?.Print(2 + depth);
        }
    }

    public override ASTNode Clone()
    {
        List<KeyValuePair<string, ASTNode?>> clonedParameters = [];
        foreach (var param in Parameters)
        {
            KeyValuePair<string, ASTNode?> item = new(param.Key, param.Value?.Clone());
            clonedParameters.Add(item);
        }

        List<ASTNode?> clonedBody = [];
        foreach (var stmt in Body)
        {
            clonedBody.Add(stmt?.Clone());
        }

        return new FunctionNode
        {
            Name = Name,
            Parameters = clonedParameters,
            Body = clonedBody,
            IsStatic = IsStatic,
            IsPrivate = IsPrivate,
            TypeHints = TypeHints,
            ReturnTypeHint = ReturnTypeHint,
            Token = Token
        };
    }
}