using hayward.Parsing.AST;
using hayward.Parsing;
using hayward.Typing;

namespace hayward.Runtime;

public enum CallableType
{
    Builtin,
    Function,
    Method,
    Lambda,
};

public class Callable(CallableType type)
{
    public CallableType Type { get; set; } = type;
    public List<KeyValuePair<string, Value>> Parameters { get; set; } = [];
    public HashSet<string> DefaultParameters { get; set; } = [];
}

public class KBuiltin(Token token, string name) : Callable(CallableType.Builtin)
{
    public string Name { get; set; } = name;
    public Token Token { get; set; } = token;
    public List<ASTNode?> Body { get; set; } = [];

    public Callable Clone()
    {
        KBuiltin cloned = new(Token, Name)
        {
            Parameters = Parameters,
            DefaultParameters = DefaultParameters
        };
        return cloned;
    }
};

public class KFunction(ASTNode node) : Callable(CallableType.Function)
{
    public string Name { get; set; } = string.Empty;
    public FunctionNode Decl { get; set; } = (FunctionNode)node.Clone();
    public bool IsStatic { get; set; }
    public bool IsPrivate { get; set; }
    public bool IsCtor { get; set; }
    public Dictionary<string, TokenName> TypeHints { get; set; } = [];
    public TokenName ReturnTypeHint { get; set; } = TokenName.Types_Any;

    public KFunction Clone()
    {
        FunctionNode nodeptr = (FunctionNode)Decl.Clone();
        return new(nodeptr)
        {
            Name = Name,
            IsStatic = IsStatic,
            IsPrivate = IsPrivate,
            IsCtor = IsCtor,
            Parameters = Parameters,
            DefaultParameters = DefaultParameters,
            TypeHints = TypeHints,
            ReturnTypeHint = ReturnTypeHint
        };
    }
}

public class KLambda(ASTNode node) : Callable(CallableType.Lambda)
{
    public LambdaNode Decl { get; set; } = (LambdaNode)node.Clone();
    public Dictionary<string, TokenName> TypeHints { get; set; } = [];
    public TokenName ReturnTypeHint { get; set; } = TokenName.Types_Any;

    public KLambda Clone()
    {
        LambdaNode nodeptr = (LambdaNode)Decl.Clone();
        return new(nodeptr)
        {
            Parameters = Parameters,
            DefaultParameters = DefaultParameters,
            TypeHints = TypeHints,
            ReturnTypeHint = ReturnTypeHint
        };
    }
};

public class KStruct
{
    public string Name { get; set; } = string.Empty;
    public string BaseStruct { get; set; } = string.Empty;
    public Dictionary<string, KFunction> Methods { get; set; } = [];

    public KStruct Clone()
    {
        KStruct cloned = new()
        {
            Name = Name,
            BaseStruct = BaseStruct
        };

        foreach (var kvp in Methods)
        {
            cloned.Methods.Add(kvp.Key, kvp.Value.Clone());
        }

        return cloned;
    }
}

public class KPackage(ASTNode node)
{
    public PackageNode Decl { get; set; } = (PackageNode)node.Clone();

    public KPackage Clone()
    {
        PackageNode nodeptr = (PackageNode)Decl.Clone();
        return new KPackage(nodeptr);
    }
}
