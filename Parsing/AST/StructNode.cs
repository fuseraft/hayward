namespace citrus.Parsing.AST;

public class StructNode : ASTNode
{
    private readonly string baseStruct = string.Empty;

    public StructNode() : base(ASTNodeType.Struct) { }
    public StructNode(string name, string baseStruct, List<string> interfaces, List<ASTNode?> methods)
        : base(ASTNodeType.Struct)
    {
        this.BaseStruct = name;
        this.baseStruct = baseStruct;
        this.Interfaces.AddRange(interfaces);
        this.Methods.AddRange(methods);
    }

    public string Name => BaseStruct;
    public string BaseStruct { get; } = string.Empty;
    public List<string> Interfaces { get; } = [];
    public List<ASTNode?> Methods { get; } = [];

    public override void Print(int depth)
    {
        ASTTracer.PrintDepth(depth);
        PrintASTNodeType();

        ASTTracer.PrintDepth(1 + depth);
        Console.WriteLine($"Name: {BaseStruct}");

        if (!string.IsNullOrEmpty(baseStruct))
        {
            ASTTracer.PrintDepth(1 + depth);
            Console.WriteLine($"Base: {baseStruct}");
        }

        if (Interfaces.Count > 0)
        {
            ASTTracer.PrintDepth(1 + depth);
            Console.WriteLine("Interfaces:");
            foreach (var iface in Interfaces)
            {
                ASTTracer.PrintDepth(2 + depth);
                Console.WriteLine(iface);
            }
        }

        if (Methods.Count > 0)
        {
            ASTTracer.PrintDepth(1 + depth);
            Console.WriteLine("Methods:");
            foreach (var method in Methods)
            {
                method?.Print(2 + depth);
            }
        }
    }

    public override ASTNode Clone()
    {
        List<ASTNode?> clonedMethods = [];
        foreach (var method in Methods)
        {
            clonedMethods.Add(method?.Clone());
        }

        return new StructNode(BaseStruct, baseStruct, Interfaces, clonedMethods);
    }
}