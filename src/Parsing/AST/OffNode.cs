namespace hayward.Parsing.AST;

public class OffNode(ASTNode eventName) : ASTNode(ASTNodeType.Off)
{
    public ASTNode EventName { get; } = eventName;

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);
        PrintASTNodeType();
        EventName?.Print(1 + depth);
    }

    public override ASTNode Clone() => new OffNode(EventName.Clone())
    {
        Token = Token
    };
}