namespace citrus.Parsing.AST;

public class IndexingNode : ASTNode
{
    public IndexingNode() : base(ASTNodeType.Index) { }
    public IndexingNode(string? name, ASTNode? indexExpression)
        : base(ASTNodeType.Index)
    {
        this.Name = name;
        this.IndexExpression = indexExpression;
    }
    public IndexingNode(ASTNode? indexedObject, ASTNode? indexExpression)
        : base(ASTNodeType.Index)
    {
        this.IndexedObject = indexedObject;
        this.IndexExpression = indexExpression;
    }

    public ASTNode? IndexedObject { get; }
    public ASTNode? IndexExpression { get; }
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

    public override ASTNode Clone() => new IndexingNode(IndexedObject?.Clone(), IndexExpression?.Clone());
}