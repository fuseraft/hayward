namespace hayward.Parsing.AST;

public class EmitNode(ASTNode eventName) : ASTNode(ASTNodeType.Emit)
{
    public ASTNode EventName { get; } = eventName;

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);
        PrintASTNodeType();
        EventName?.Print(1 + depth);
    }

    public override ASTNode Clone()
    {
        return new EmitNode(EventName.Clone())
        {
            Token = Token
        };
    }
}