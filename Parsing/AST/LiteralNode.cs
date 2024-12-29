using citrus.Typing;

namespace citrus.Parsing.AST;

public class LiteralNode(Value value) : ASTNode(ASTNodeType.Literal)
{
    public Value Value { get; } = value;

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);
        Console.WriteLine($"Literal: {Token?.Text}");
    }

    public override ASTNode Clone() => new LiteralNode(Value);
};