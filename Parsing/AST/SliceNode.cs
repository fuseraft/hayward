namespace citrus.Parsing.AST;

public class SliceNode : ASTNode
{
    public SliceNode() : base(ASTNodeType.Slice) { }
    public SliceNode(ASTNode? slicedObject, ASTNode? startExpression = null, ASTNode? stopExpression = null, ASTNode? stepExpression = null)
      : base(ASTNodeType.Slice)
    {
        SlicedObject = slicedObject;
        StartExpression = startExpression;
        StopExpression = stopExpression;
        StepExpression = stepExpression;
    }

    public ASTNode? SlicedObject { get; }
    public ASTNode? StartExpression { get; }
    public ASTNode? StopExpression { get; }
    public ASTNode? StepExpression { get; }

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);

        if (SlicedObject != null)
        {
            Console.WriteLine("Slice on object:");
            SlicedObject.Print(1 + depth);
        }

        if (StartExpression != null)
        {
            ASTTracer.PrintDepth(depth);
            Console.WriteLine("Slice start:");
            StartExpression.Print(1 + depth);
        }

        if (StopExpression != null)
        {
            ASTTracer.PrintDepth(depth);
            Console.WriteLine("Slice stop:");
            StopExpression.Print(1 + depth);
        }

        if (StepExpression != null)
        {
            ASTTracer.PrintDepth(depth);
            Console.WriteLine("Slice step:");
            StepExpression.Print(1 + depth);
        }
    }

    public override ASTNode Clone() => new SliceNode(SlicedObject?.Clone(), StartExpression?.Clone(), StopExpression?.Clone(), StepExpression?.Clone());
}