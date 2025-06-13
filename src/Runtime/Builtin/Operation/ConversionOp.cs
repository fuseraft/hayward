using hayward.Parsing;
using hayward.Tracing.Error;
using hayward.Typing;

namespace hayward.Runtime.Builtin.Operation;

public struct ConversionOp
{
    public static long GetInteger(Token t, Value v, string message = "Expected an integer value.")
    {
        if (v.IsFloat())
        {
            return Convert.ToInt64(v.GetFloat());
        }
        
        if (!v.IsInteger())
        {
            throw new ConversionError(t, message);
        }

        return v.GetInteger();
    }

    public static double GetFloat(Token t, Value v, string message = "Expected an integer or float value.")
    {
        if (v.IsInteger())
        {
            return Convert.ToDouble(v.GetInteger());
        }
        else if (v.IsFloat())
        {
            return v.GetFloat();
        }

        throw new ConversionError(t, message);
    }

    public static List<Value> GetList(Value v)
    {
        if (!v.IsList())
        {
            List<Value> newList = [v];
            return newList;
        }
        else
        {
            return v.GetList();
        }
    }

    public static string ToString(Value v)
    {
        return Serializer.Serialize(v);
    }
}