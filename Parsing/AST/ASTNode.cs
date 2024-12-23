namespace citrus.Parsing.AST;

public class ASTNode(ASTNodeType type)
{
    public virtual void Print(int depth = 0)
    {
        ASTTracer.PrintDepth(depth);
        PrintASTNodeType();
    }

    public Token? Token { get; set; } = null;
    public ASTNodeType Type { get; } = type;

    public void PrintASTNodeType()
    {
        Console.WriteLine($"{Enum.GetName(typeof(ASTNodeType), Type)}:");
    }

    public virtual ASTNode Clone() => new (Type)
    {
        Token = Token
    };
}

public class ASTTracer
{
    public static void PrintDepth(int depth)
    {
        for (int i = 0; i < depth; ++i)
        {
            Console.Write("  ");
        }
    }

    public static string Unmangle(string? name)
    {
        if (name?.Length > 10)
        {
            var mangler = name.Substring(0, 10);

            if (mangler.StartsWith('_') && mangler.EndsWith('_'))
            {
                return name.Replace(mangler, string.Empty);
            }
        }

        return name ?? string.Empty;
    }
}

public enum ASTNodeType
{
    Assignment,
    BinaryOperation,
    Break,
    Case,
    CaseWhen,
    ConstAssignment,
    Exit,
    Export,
    ForLoop,
    Spawn,
    FunctionCall,
    Function,
    HashLiteral,
    Identifier,
    If,
    Import,
    IndexAssignment,
    Index,
    Interface,
    Lambda,
    LambdaCall,
    ListLiteral,
    Literal,
    MemberAccess,
    MemberAssignment,
    MethodCall,
    Next,
    NoOp,
    PackAssignment,
    Package,
    Parse,
    Print,
    PrintXy,
    Program,
    RangeLiteral,
    RepeatLoop,
    Return,
    Self,
    Slice,
    Struct,
    TernaryOperation,
    Throw,
    Try,
    UnaryOperation,
    Variable,
    WhileLoop,
};