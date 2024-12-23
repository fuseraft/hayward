namespace citrus.Parsing.AST;

public class LiteralNode : ASTNode
{
    public LiteralNode() : base(ASTNodeType.Literal) {}
    public LiteralNode(object? value)
      : base(ASTNodeType.Literal) => Value = value;

    public object? Value { get; }

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);
        Console.WriteLine($"Literal: {Value}"); // Serializer::serialize(value)
    }

    public override ASTNode Clone() => new LiteralNode(Value);
};