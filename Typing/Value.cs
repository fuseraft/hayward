namespace citrus.Typing;

public class Value(object? value = null, ValueType type = ValueType.None)
{
    public object? Value_ { get; set; } = value ?? null;
    public ValueType Type { get; set; } = type;

    public static Value Create(object? value)
    {
        if (value == null)
        {
            return CreateNull();
        }

        if (value is long)
        {
            return CreateInteger(value);
        }
        else if (value is double)
        {
            return CreateFloat(value);
        }
        else if (value is bool)
        {
            return CreateBoolean(value);
        }
        else if (value is string)
        {
            return CreateString(value);
        }
        else if (value is List<Value>)
        {
            return CreateList(value);
        }
        else if (value is Dictionary<Value, Value>)
        {
            return CreateHashmap(value);
        }

        /*case ValueType.Object:
            return CreateObject(value);
        case ValueType.Lambda:
            return CreateLambda(value);
        case ValueType.None:
            return CreateNull();
        case ValueType.Struct:
            return CreateStruct(value);
        case ValueType.Pointer:
            return CreatePointer(value);
        default:
            return {};
        }*/
        return CreateNull();
    }

    public static Value EmptyString() { return CreateString(string.Empty); }

    public static Value CreateInteger(long value) => new(value, ValueType.Integer);
    public static Value CreateInteger(object value) => new(value, ValueType.Integer);
    public static Value CreateFloat(object value) => new(value, ValueType.Float);
    public static Value CreateBoolean(object value) => new(value, ValueType.Boolean);
    public static Value CreateString(object value) => new(value, ValueType.String);
    public static Value CreateList(object value) => new(value, ValueType.List);
    public static Value CreateHashmap(object value) => new(value, ValueType.Hashmap);
    public static Value CreateNull(object value) => new(value, ValueType.None);
    public static Value CreateNull() => new(null, ValueType.None);
    /*
    public static Value CreateObject(object value) {
        return {value, ValueType.Object};
    }
    public static Value CreateLambda(object value) {
        return {value, ValueType.Lambda};
    }
    public static Value CreateStruct(object value) {
        return {value, ValueType.Struct);
    }
    public static Value CreatePointer(object value) {
        return new (value, ValueType.Pointer);
    }*/

    public long GetInteger() => Convert.ToInt64(Value_);
    public double GetFloat() => Convert.ToDouble(Value_);
    public bool GetBoolean() => Convert.ToBoolean(Value_);
    public string GetString() => Value_ != null ? (Value_ as string) ?? string.Empty : string.Empty;
    public List<Value> GetList() => Value_ != null ? (Value_ as List<Value>) ?? [] : [];
    public Dictionary<Value, Value> GetHashmap() => Value_ != null ? (Value_ as Dictionary<Value, Value>) ?? [] : [];
    /*public const kObject GetObject() const { return std::get<kObject>(Value_); }
    public const kLambda GetLambda() const { return std::get<kLambda>(Value_); }
    public const k_null GetNull() const { return std::get<k_null>(Value_); }
    public const kStruct GetStruct() const { return std::get<kStruct>(Value_); }
    public const kPointer GetPointer() const { return std::get<kPointer>(Value_); }*/

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

    public void Set(object? value, ValueType type)
    {
        Value_ = value;
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

    /*public void SetValue(const kObject& value)
    {
        Value_ = value;
        Type = ValueType.Object;
    }
    public void SetValue(const kLambda& value)
    {
        Value_ = value;
        Type = ValueType.Lambda;
    }
    public void SetValue(const k_null& value)
    {
        Value_ = value;
        Type = ValueType.None;
    }
    public void SetValue(const kStruct& value)
    {
        Value_ = value;
        Type = ValueType.Struct;
    }
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