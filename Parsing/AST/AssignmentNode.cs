namespace hayward.Parsing.AST;

public class AssignmentNode : ASTNode
{
    public AssignmentNode() : base(ASTNodeType.Assignment) { }
    public AssignmentNode(ASTNode? left, string name,
                   TokenName op, ASTNode? initializer)
        : base(ASTNodeType.Assignment)
    {
        Left = left;
        Name = name;
        Op = op;
        Initializer = initializer;
    }

    public ASTNode? Left { get; set; }
    public string Name { get; set; } = string.Empty;
    public TokenName Op { get; set; } = TokenName.Default;
    public ASTNode? Initializer { get; set; }

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);
        Console.WriteLine($"Assignment: `{ASTTracer.Unmangle(Name)}`");
        ASTTracer.PrintDepth(1 + depth);
        Console.WriteLine("Initializer:");
        Initializer?.Print(2 + depth);
    }

    public override ASTNode Clone()
    {
        return new AssignmentNode(Left?.Clone(), Name, Op, Initializer?.Clone());
    }
}