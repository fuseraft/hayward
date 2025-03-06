namespace hayward.Parsing.AST;

public class AssignmentNode(ASTNode? left, string name, TokenName op, ASTNode? initializer) : ASTNode(ASTNodeType.Assignment)
{
    public ASTNode? Left { get; set; } = left;
    public string Name { get; set; } = name;
    public TokenName Op { get; set; } = op;
    public ASTNode? Initializer { get; set; } = initializer;
    public ASTNode? Condition { get; set; } = null;

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);
        Console.WriteLine($"Assignment: `{ASTTracer.Unmangle(Name)}`");
        ASTTracer.PrintDepth(1 + depth);
        Console.WriteLine("Initializer:");
        Initializer?.Print(2 + depth);
    }

    public override ASTNode Clone() => new AssignmentNode(Left?.Clone(), Name, Op, Initializer?.Clone())
    {
        Condition = Condition?.Clone(),
        Token = Token
    };
}