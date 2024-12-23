namespace citrus.Parsing.AST;

public class SpawnNode : ASTNode
{
    public ASTNode? Expression { get; }

    public SpawnNode() : base(ASTNodeType.Spawn) {}
    public SpawnNode(ASTNode? expression)
      : base(ASTNodeType.Spawn)
    {
        Expression = expression;
    }

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);
        PrintASTNodeType();
        Expression?.Print(1 + depth);
    }

    public override ASTNode Clone()
    {
        return new SpawnNode(Expression?.Clone());
    }
}