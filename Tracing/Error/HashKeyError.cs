using hayward.Parsing;

namespace hayward.Tracing.Error;

public class HashKeyError(Token t, string key)
    : HaywardError(t, "HashKeyError", $"Invalid hashmap key: '{key}'")
{
}