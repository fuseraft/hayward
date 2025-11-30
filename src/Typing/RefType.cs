namespace hayward.Typing;

public class InstanceRef
{
    public string Identifier { get; set; } = string.Empty;
    public string StructName { get; set; } = string.Empty;
    public Dictionary<string, Value> InstanceVariables { get; set; } = [];

    public bool HasVariable(string name)
    {
        return InstanceVariables.ContainsKey(name);
    }
}

public class LambdaRef
{
    public string Identifier { get; set; } = string.Empty;
}

public class StructRef
{
    public string Identifier { get; set; } = string.Empty;
}

public class StreamRef
{
    public string Identifier { get; set; } = string.Empty;
}

public class NullRef { }