namespace hayward.Parsing.AST;

public class IndexingNode(ASTNode? indexedObject, ASTNode indexExpression) : ASTNode(ASTNodeType.Index)
{
    public ASTNode? IndexedObject { get; } = indexedObject;
    public ASTNode IndexExpression { get; } = indexExpression;
    public string? Name { get; }

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);

        if (IndexedObject == null)
        {
            PrintASTNodeType();
            ASTTracer.PrintDepth(1 + depth);
            Console.WriteLine($"Name: `{ASTTracer.Unmangle(Name)}`");
        }
        else
        {
            Console.WriteLine("Index on object:");
            IndexedObject.Print(1 + depth);
        }

        IndexExpression?.Print(1 + depth);
    }

    public override ASTNode Clone() => new IndexingNode(IndexedObject?.Clone(), IndexExpression.Clone())
    {
        Token = Token
    };
}