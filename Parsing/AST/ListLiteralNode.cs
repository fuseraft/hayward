namespace citrus.Parsing.AST;

public class ListLiteralNode : ASTNode
{
    public ListLiteralNode()
        : base(ASTNodeType.ListLiteral) {}
    public ListLiteralNode(List<ASTNode?> elements)
        : base(ASTNodeType.ListLiteral) => this.Elements = elements;

    public List<ASTNode?> Elements { get; } = [];

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);
        PrintASTNodeType();
        
        foreach (var element in Elements)
        {
            element?.Print(1 + depth);
        }
    }

    public override ASTNode Clone()
    {
        List<ASTNode?> clonedElements = [];
        
        foreach (var element in Elements)
        {
            clonedElements.Add(element?.Clone());
        }

        return new ListLiteralNode(clonedElements);
    }
}