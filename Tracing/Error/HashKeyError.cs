using citrus.Parsing;

namespace citrus.Tracing.Error;

public class HashKeyError(Token t, string key)
    : CitrusError(t, "HashKeyError", $"Invalid hashmap key: '{key}'")
{
}