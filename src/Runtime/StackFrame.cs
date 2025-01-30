using hayward.Typing;

namespace hayward.Runtime;

[Flags]
public enum FrameFlags : ushort
{
    None = 0,
    Return = 1 << 0,
    SubFrame = 1 << 1,
    InLoop = 1 << 2,
    Break = 1 << 3,
    Next = 1 << 4,
    InTry = 1 << 5,
    InObject = 1 << 6,
    InLambda = 1 << 7,
}

public class StackFrame
{
    public Dictionary<string, Value> Variables { get; set; } = [];
    public Value? ReturnValue { get; set; }
    public InstanceRef? ObjectContext { get; set; }
    public FrameFlags Flags { get; set; } = FrameFlags.None;
    public string Name { get; set; } = string.Empty;

    public bool HasVariable(string name) => Variables.ContainsKey(name);

    public void SetObjectContext(InstanceRef? obj)
    {
        ObjectContext = obj;
        SetFlag(FrameFlags.InObject);
    }

    public bool InObjectContext() => IsFlagSet(FrameFlags.InObject);
    public InstanceRef? GetObjectContext() => ObjectContext;

    public void SetFlag(FrameFlags flag) => Flags |= flag;
    public void ClearFlag(FrameFlags flag) => Flags &= ~flag;
    public bool IsFlagSet(FrameFlags flag) => (Flags & flag) == flag;
}