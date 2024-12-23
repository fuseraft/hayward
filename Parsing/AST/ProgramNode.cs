namespace citrus.Parsing.AST;

public class ProgramNode : ASTNode
{
    public ProgramNode()
        : base(ASTNodeType.Program) {}

    public ProgramNode(List<ASTNode> statements)
        : base(ASTNodeType.Program)
    {
        Statements.AddRange(statements);
    }

    public List<ASTNode> Statements { get; set; } = [];
    public bool IsScript { get; set; } = false;

    public override void Print(int depth = 0)
    {
        ASTTracer.PrintDepth(depth);
        PrintASTNodeType();
        foreach (var statement in Statements)
        {
            statement.Print(1 + depth);
        }
    }

    public override ASTNode Clone()
    {
        List<ASTNode> clonedStatements = [];

        foreach (var statement in Statements)
        {
            clonedStatements.Add(statement.Clone());
        }

        return new ProgramNode(clonedStatements);
    }
}