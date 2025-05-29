using hayward.Parsing;
using ValueType = hayward.Typing.ValueType;

namespace hayward.Tracing.Error;

public class ParameterTypeMismatchError : HaywardError
{
    public ParameterTypeMismatchError(Token t, string name, int position, ValueType expectedType, ValueType actualType)
        : base(t, "ParameterTypeMismatchError", $"The type for parameter {position} of `{name}` is not an expected type. Expected {expectedType} but received {actualType}.")
    {
    }
}