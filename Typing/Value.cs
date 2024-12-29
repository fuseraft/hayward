using citrus.Parsing;
using citrus.Tracing.Error;
namespace citrus.Typing;

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

public class NullRef { }

public class SliceIndex
{
    public Value? IndexOrStart { get; set; }
    public Value? StopIndex { get; set; }
    public Value? StepValue { get; set; }
    public bool IsSlice { get; set; }
};

public class Value(object? value = null, ValueType type = ValueType.None)
{
    public object Value_ { get; set; } = value ?? new NullRef();
    public ValueType Type { get; set; } = type;

    public static Value Create(object? value)
    {
        return value switch
        {
            null => CreateNull(),
            long => CreateInteger(value),
            double => CreateFloat(value),
            bool => CreateBoolean(value),
            string => CreateString(value),
            List<Value> => CreateList(value),
            Dictionary<Value, Value> => CreateHashmap(value),
            InstanceRef => CreateObject(value),
            LambdaRef => CreateLambda(value),
            StructRef => CreateStruct(value),
            NullRef => CreateNull(),
            _ => CreateNull()
        };
    }

    public static Value EmptyString() { return CreateString(string.Empty); }
    public static Value CreateInteger(long value) => new(value, ValueType.Integer);
    public static Value CreateInteger(object value) => new(value, ValueType.Integer);
    public static Value CreateFloat(double value) => new(value, ValueType.Float);
    public static Value CreateFloat(object value) => new(value, ValueType.Float);
    public static Value CreateBoolean(bool value) => new(value, ValueType.Float);
    public static Value CreateBoolean(object value) => new(value, ValueType.Boolean);
    public static Value CreateString(string value) => new(value, ValueType.String);
    public static Value CreateString(object value) => new(value, ValueType.String);
    public static Value CreateList(List<Value> value) => new(value, ValueType.List);
    public static Value CreateList(object value) => new(value, ValueType.List);
    public static Value CreateList() => new(new List<Value>(), ValueType.List);
    public static Value CreateHashmap(Dictionary<Value, Value> value) => new(value, ValueType.Hashmap);
    public static Value CreateHashmap(object value) => new(value, ValueType.Hashmap);
    public static Value CreateHashmap() => new(new Dictionary<Value, Value>(), ValueType.Hashmap);
    public static Value CreateNull(NullRef value) => new(value, ValueType.None);
    public static Value CreateNull(object value) => new(value, ValueType.None);
    public static Value CreateNull() => new(new NullRef(), ValueType.None);
    public static Value CreateObject(InstanceRef value) => new(value, ValueType.Object);
    public static Value CreateObject(object value) => new(value, ValueType.Object);
    public static Value CreateLambda(LambdaRef value) => new (value, ValueType.Lambda);
    public static Value CreateLambda(object value) => new (value, ValueType.Lambda);
    public static Value CreateStruct(StructRef value) => new (value, ValueType.Struct);
    public static Value CreateStruct(object value) => new (value, ValueType.Struct);
    // public static Value CreatePointer(object value) => new(value, ValueType.Pointer);

    public long GetInteger() => (long)Value_;
    public double GetFloat() => (double)Value_;
    public bool GetBoolean() => (bool)Value_;
    public string GetString() => (string)Value_;
    public List<Value> GetList() => (List<Value>)Value_;
    public Dictionary<Value, Value> GetHashmap() => (Dictionary<Value, Value>)Value_;
    public InstanceRef GetObject() => (InstanceRef)Value_;
    public LambdaRef GetLambda() => (LambdaRef)Value_;
    public StructRef GetStruct() => (StructRef)Value_;
    public NullRef GetNull()
    {
        Value_ ??= new NullRef();
        return (NullRef)Value_;
    }

    public bool IsInteger() => Type == ValueType.Integer;
    public bool IsFloat() => Type == ValueType.Float;
    public bool IsBoolean() => Type == ValueType.Boolean;
    public bool IsString() => Type == ValueType.String;
    public bool IsList() => Type == ValueType.List;
    public bool IsHashmap() => Type == ValueType.Hashmap;
    public bool IsObject() => Type == ValueType.Object;
    public bool IsLambda() => Type == ValueType.Lambda;
    public bool IsNull() => Type == ValueType.None;
    public bool IsStruct() => Type == ValueType.Struct;
    public bool IsPointer() => Type == ValueType.Pointer;

    public static List<Value> Clone(List<Value> list)
    {
        List<Value> clone = [];
        
        foreach (var value in list)
        {
            clone.Add(value.Clone());
        }
        
        return clone;
    }

    public static Dictionary<Value, Value> Clone(Dictionary<Value, Value> hash)
    {
        Dictionary<Value, Value> clone = [];
        
        foreach (var key in hash.Keys)
        {
            clone.Add(key, hash[key].Clone());
        }

        return clone;
    }

    public Value Clone()
    {
        return Type switch
        {
            ValueType.Integer => CreateInteger(GetInteger()),
            ValueType.Float => CreateFloat(GetFloat()),
            ValueType.Boolean => CreateBoolean(GetBoolean()),
            ValueType.String => CreateString(GetString()),
            ValueType.List => CreateList(Clone(GetList())),
            ValueType.Hashmap => CreateHashmap(Clone(GetHashmap())),
            ValueType.Object => CreateObject(GetObject()),
            ValueType.Lambda => CreateLambda(GetLambda()),
            ValueType.None => CreateNull(),
            ValueType.Struct => CreateStruct(GetStruct()),
            _ => throw new Exception("Unsupported type for cloning"),
        };
    }

    public void Set(object? value, ValueType type)
    {
        Value_ = value ?? new NullRef();
        Type = type;
    }

    public void SetValue(Value value)
    {
        Value_ = value.Value_;
        Type = value.Type;
    }

    public void SetValue(long value)
    {
        Value_ = value;
        Type = ValueType.Integer;
    }

    public void SetValue(int value)
    {
        Value_ = (long)value;
        Type = ValueType.Integer;
    }

    public void SetValue(double value)
    {
        Value_ = value;
        Type = ValueType.Float;
    }

    public void SetValue(bool value)
    {
        Value_ = value;
        Type = ValueType.Boolean;
    }

    public void SetValue(string value)
    {
        Value_ = value;
        Type = ValueType.String;
    }

    public void SetValue(List<Value> value)
    {
        Value_ = value;
        Type = ValueType.List;
    }

    public void SetValue(Dictionary<Value, Value> value)
    {
        Value_ = value;
        Type = ValueType.Hashmap;
    }

    public void SetValue(InstanceRef value)
    {
        Value_ = value;
        Type = ValueType.Object;
    }

    public void SetValue(LambdaRef value)
    {
        Value_ = value;
        Type = ValueType.Lambda;
    }

    public void SetValue(StructRef value)
    {
        Value_ = value;
        Type = ValueType.Struct;
    }

    public void SetValue(NullRef value)
    {
        Value_ = value;
        Type = ValueType.None;
    }

    /*
    public void SetValue(const kPointer& value)
    {
        Value_ = value;
        Type = ValueType.Pointer;
    }*/
}


public enum ValueType
{
    Integer = 0,
    Float = 1,
    Boolean = 2,
    String = 3,
    List = 4,
    Hashmap = 5,
    Object = 6,
    Lambda = 7,
    None = 8,
    Struct = 9,
    Pointer = 10,
    Unset = 20
};