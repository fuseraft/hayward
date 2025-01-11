using citrus.Builtin.Operation;
using citrus.Parsing;
using citrus.Parsing.Builtins;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Builtin.Handlers;

public static class CoreBuiltinHandler
{
    public static Value Execute(Token token, TokenName builtin, Value value, List<Value> args)
    {
        return builtin switch
        {
            TokenName.Builtin_Kiwi_Chars => ExecuteChars(token, value, args),
            TokenName.Builtin_Kiwi_Join => ExecuteJoin(token, value, args),
            TokenName.Builtin_Kiwi_HasKey => ExecuteHasKey(token, value, args),
            TokenName.Builtin_Kiwi_Size => ExecuteSize(token, value, args),
            TokenName.Builtin_Kiwi_Uppercase => ExecuteUppercase(token, value, args),
            TokenName.Builtin_Kiwi_Lowercase => ExecuteLowercase(token, value, args),
            TokenName.Builtin_Kiwi_BeginsWith => ExecuteBeginsWith(token, value, args),
            TokenName.Builtin_Kiwi_Contains => ExecuteContains(token, value, args),
            TokenName.Builtin_Kiwi_EndsWith => ExecuteEndsWith(token, value, args),
            TokenName.Builtin_Kiwi_LeftTrim => ExecuteLeftTrim(token, value, args),
            TokenName.Builtin_Kiwi_RightTrim => ExecuteRightTrim(token, value, args),
            TokenName.Builtin_Kiwi_Trim => ExecuteTrim(token, value, args),
            TokenName.Builtin_Kiwi_Empty => ExecuteEmpty(token, value, args),
            TokenName.Builtin_Kiwi_Type => ExecuteType(token, value, args),
            TokenName.Builtin_Kiwi_First => ExecuteFirst(token, value, args),
            TokenName.Builtin_Kiwi_Last => ExecuteLast(token, value, args),
            TokenName.Builtin_Kiwi_Push => ExecutePush(token, value, args),
            TokenName.Builtin_Kiwi_Pop => ExecutePop(token, value, args),
            TokenName.Builtin_Kiwi_Clear => ExecuteClear(token, value, args),
            TokenName.Builtin_Kiwi_Replace => ExecuteReplace(token, value, args),
            TokenName.Builtin_Kiwi_Remove => ExecuteRemove(token, value, args),
            TokenName.Builtin_Kiwi_RemoveAt => ExecuteRemoveAt(token, value, args),
            TokenName.Builtin_Kiwi_Get => ExecuteGet(token, value, args),
            TokenName.Builtin_Kiwi_Set => ExecuteSet(token, value, args),
            TokenName.Builtin_Kiwi_Truthy => ExecuteTruthy(token, value, args),
            TokenName.Builtin_Kiwi_Split => ExecuteSplit(token, value, args),
            TokenName.Builtin_Kiwi_Lines => ExecuteLines(token, value, args),
            TokenName.Builtin_Kiwi_Clone => ExecuteClone(token, value, args),
            TokenName.Builtin_Kiwi_Reverse => ExecuteReverse(token, value, args),
            TokenName.Builtin_Kiwi_IndexOf => ExecuteIndexOf(token, value, args),
            TokenName.Builtin_Kiwi_LastIndexOf => ExecuteLastIndexOf(token, value, args),
            TokenName.Builtin_Kiwi_Substring => ExecuteSubstring(token, value, args),
            TokenName.Builtin_Kiwi_Concat => ExecuteConcat(token, value, args),
            TokenName.Builtin_Kiwi_Unique => ExecuteUnique(token, value, args),
            TokenName.Builtin_Kiwi_Count => ExecuteCount(token, value, args),
            TokenName.Builtin_Kiwi_Enqueue => ExecuteEnqueue(token, value, args),
            TokenName.Builtin_Kiwi_Dequeue => ExecuteDequeue(token, value, args),
            TokenName.Builtin_Kiwi_Shift => ExecuteShift(token, value, args),
            TokenName.Builtin_Kiwi_Unshift => ExecuteUnshift(token, value, args),
            TokenName.Builtin_Kiwi_Insert => ExecuteInsert(token, value, args),
            TokenName.Builtin_Kiwi_Keys => ExecuteKeys(token, value, args),
            TokenName.Builtin_Kiwi_Values => ExecuteValues(token, value, args),
            TokenName.Builtin_Kiwi_Zip => ExecuteZip(token, value, args),
            TokenName.Builtin_Kiwi_Flatten => ExecuteFlatten(token, value, args),
            TokenName.Builtin_Kiwi_Merge => ExecuteMerge(token, value, args),
            TokenName.Builtin_Kiwi_Rotate => ExecuteRotate(token, value, args),
            TokenName.Builtin_Kiwi_Slice => ExecuteSlice(token, value, args),
            TokenName.Builtin_Kiwi_IsA => ExecuteIsA(token, value, args),
            TokenName.Builtin_Kiwi_ToBytes => ExecuteToBytes(token, value, args),
            TokenName.Builtin_Kiwi_ToHex => ExecuteToHex(token, value, args),
            TokenName.Builtin_Kiwi_ToF => ExecuteToFloat(token, value, args),
            TokenName.Builtin_Kiwi_ToI => ExecuteToInteger(token, value, args),
            TokenName.Builtin_Kiwi_ToS => ExecuteToString(token, value, args),
            TokenName.Builtin_Kiwi_Swap => ExecuteSwap(token, value, args),
            TokenName.Builtin_Kiwi_Pretty => ExecutePretty(token, value, args),
            /*
            TokenName.Builtin_Kiwi_RReplace => ExecuteRReplace(token, value, args),
            TokenName.Builtin_Kiwi_RSplit => ExecuteRSplit(token, value, args),
            TokenName.Builtin_Kiwi_Find => ExecuteFind(token, value, args),
            TokenName.Builtin_Kiwi_Match => ExecuteMatch(token, value, args),
            TokenName.Builtin_Kiwi_Matches => ExecuteMatches(token, value, args),
            TokenName.Builtin_Kiwi_MatchesAll => ExecuteMatchesAll(token, value, args),
            TokenName.Builtin_Kiwi_Scan => ExecuteScan(token, value, args),
            TokenName.Builtin_Kiwi_Tokens => ExecuteTokens(token, value, args),
            */
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }

    private static Value ExecutePretty(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Pretty);
        }

        var pretty = Serializer.PrettySerialize(value);
        return Value.CreateString(pretty);
    }

    private static Value ExecuteIsA(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.IsA);
        }

        var typeName = args[0].GetString();

        return value.Type switch
        {
            Typing.ValueType.Integer => Value.CreateBoolean(typeName.Equals("Integer") || typeName.Equals("integer")),
            Typing.ValueType.Float => Value.CreateBoolean(typeName.Equals("Float") || typeName.Equals("float")),
            Typing.ValueType.Boolean => Value.CreateBoolean(typeName.Equals("Boolean") || typeName.Equals("boolean")),
            Typing.ValueType.String => Value.CreateBoolean(typeName.Equals("String") || typeName.Equals("string")),
            Typing.ValueType.List => Value.CreateBoolean(typeName.Equals("List") || typeName.Equals("list")),
            Typing.ValueType.Hashmap => Value.CreateBoolean(typeName.Equals("Hashmap") || typeName.Equals("hashmap")),
            Typing.ValueType.Object => Value.CreateBoolean(typeName.Equals("Object") || typeName.Equals("object")),
            Typing.ValueType.Lambda => Value.CreateBoolean(typeName.Equals("Lambda") || typeName.Equals("lambda")),
            Typing.ValueType.None => Value.CreateBoolean(typeName.Equals("None") || typeName.Equals("none")),
            _ => Value.CreateBoolean(false),
        };
    }

    private static Value ExecuteToBytes(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.ToBytes);
        }

        if (value.IsString())
        {
            var stringValue = value.GetString();
            var bytes = System.Text.Encoding.UTF8.GetBytes(stringValue);
            var byteList = new List<Value>(bytes.Length);

            foreach (var b in bytes)
            {
                byteList.Add(Value.CreateInteger(b));
            }

            return Value.CreateList(byteList);
        }
        else if (value.IsList())
        {
            var listElements = value.GetList();
            var resultBytes = new List<Value>();

            foreach (var item in listElements)
            {
                if (!item.IsString())
                {
                    throw new InvalidOperationError(token, "Expected a list of strings for byte conversion.");
                }

                var stringValue = item.GetString();
                var bytes = System.Text.Encoding.UTF8.GetBytes(stringValue);

                foreach (var b in bytes)
                {
                    resultBytes.Add(Value.CreateInteger(b));
                }
            }

            return Value.CreateList(resultBytes);
        }

        throw new InvalidOperationError(token, "Expected a string or list of strings to convert to bytes.");
    }

    private static Value ExecuteToHex(Token token, Value value, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.ToHex);
        }

        // value is an integer
        if (value.IsInteger())
        {
            var number = value.GetInteger();
            var hexString = number.ToString("x");
            return Value.CreateString(hexString);
        }

        // if not an integer, we expect a list of integers (bytes)
        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list value for byte-to-hex conversion.");
        }

        var elements = value.GetList();
        if (elements.Count == 0)
        {
            throw new InvalidOperationError(token, "Cannot convert an empty list to hex.");
        }

        var width = 2;

        if (args.Count == 1)
        {
            if (!args[0].IsInteger())
            {
                throw new ConversionError(token, "Expected an integer as the width argument.");
            }

            width = (int)args[0].GetInteger();
            if (width < 2)
            {
                throw new InvalidOperationError(token, "Width must be >= 2.");
            }
        }

        var sb = new System.Text.StringBuilder();

        foreach (var item in elements)
        {
            if (!item.IsInteger())
            {
                throw new InvalidOperationError(token, "Expected a list of integers.");
            }

            var byteValue = item.GetInteger();
            var b = (byte)(byteValue & 0xFF);
            sb.Append(b.ToString("x").PadLeft(width, '0'));
        }

        return Value.CreateString(sb.ToString());
    }

    private static Value ExecuteToFloat(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.ToF);
        }

        if (value.IsString())
        {
            var stringValue = value.GetString();

            if (double.TryParse(stringValue, System.Globalization.NumberStyles.Float | System.Globalization.NumberStyles.AllowThousands, System.Globalization.CultureInfo.InvariantCulture, out double f))
            {
                return Value.CreateFloat(f);
            }
            else
            {
                throw new ConversionError(token, $"Cannot convert non-numeric value to a float: `{stringValue}`");
            }
        }
        else if (value.IsInteger())
        {
            return Value.CreateFloat(value.GetInteger());
        }
        else if (value.IsFloat())
        {
            return value;
        }

        throw new ConversionError(token, "Cannot convert non-numeric value to a float.");
    }

    private static Value ExecuteToInteger(Token token, Value value, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.ToI);
        }

        var numberBase = 10;

        // validate base if detected
        if (args.Count == 1)
        {
            if (!args[0].IsInteger())
            {
                throw new ConversionError(token, "Expected an integer as the base argument.");
            }

            numberBase = (int)args[0].GetInteger();
            if (numberBase < 2 || numberBase > 36)
            {
                throw new InvalidOperationError(
                    token, "Base must be between 2 and 36, inclusive."
                );
            }
        }

        if (value.IsString())
        {
            var stringValue = value.GetString();

            try
            {
                var parsed = ParseStringToIntBase(token, stringValue, numberBase);
                return Value.CreateInteger(parsed);
            }
            catch (Exception)
            {
                throw new ConversionError(token, $"Cannot convert non-numeric string to an integer: `{stringValue}`");
            }
        }
        else if (value.IsFloat())
        {
            // truncate decimal part
            var intValue = (long)value.GetFloat();
            return Value.CreateInteger(intValue);
        }
        else if (value.IsInteger())
        {
            // already an integer, return as-is
            return value;
        }
        else
        {
            // we can't convert
            throw new ConversionError(token, "Cannot convert non-numeric value to an integer.");
        }
    }

    /// <summary>
    /// Parses a string to a long using the specified base (2 to 36).
    /// Throws an exception if the string has invalid characters or overflows long.
    /// </summary>
    /// <param name="token">The tracing token.</param>
    /// <param name="s">The input string (e.g., "FF", "1010", "-1A").</param>
    /// <param name="numberBase">Base from 2 to 36 inclusive.</param>
    /// <returns>A long integer parsed from the string in the given base.</returns>
    private static long ParseStringToIntBase(Token token, string s, int numberBase)
    {
        s = s.Trim();

        if (string.IsNullOrEmpty(s))
        {
            throw new InvalidOperationError(token, "Empty string cannot be converted to an integer.");
        }

        var negative = false;
        if (s.StartsWith('-'))
        {
            negative = true;
            s = s[1..];
            if (string.IsNullOrEmpty(s))
            {
                // just "-" is not valid
                throw new InvalidOperationError(token, "Invalid numeric string.");
            }
        }

        var result = 0L;

        foreach (char c in s)
        {
            var digit = CharToDigit(token, c);
            if (digit < 0 || digit >= numberBase)
            {
                throw new InvalidOperationError(token, $"Invalid digit '{c}' for base {numberBase}.");
            }

            checked // to detect overflow
            {
                result = result * numberBase + digit;
            }
        }

        return negative ? -result : result;
    }

    /// <summary>
    /// Converts a single character ('0'-'9', 'a'-'z', 'A'-'Z') to its numeric value.
    /// For digits 0-9 => 0..9
    /// For letters a-z => 10..35
    /// For letters A-Z => 10..35
    /// Throws an exception if the character is not in a valid range.
    /// </summary>
    private static int CharToDigit(Token token, char c)
    {
        if (c >= '0' && c <= '9')
        {
            return c - '0';
        }

        if (c >= 'a' && c <= 'z')
        {
            return c - 'a' + 10;
        }

        if (c >= 'A' && c <= 'Z')
        {
            return c - 'A' + 10;
        }

        throw new InvalidOperationError(token, $"Invalid character '{c}' in numeric string.");
    }


    private static Value ExecuteToString(Token token, Value value, List<Value> args)
    {
        // 0 or 1 argument is allowed
        if (args.Count != 0 && args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.ToS);
        }

        // no format argument provided
        if (args.Count == 0)
        {
            var serialized = Serializer.Serialize(value);
            return Value.CreateString(serialized);
        }

        // format argument must be a string
        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var format = args[0].GetString().Trim();

        // if empty, just serialize
        if (string.IsNullOrEmpty(format))
        {
            var serialized = Serializer.Serialize(value);
            return Value.CreateString(serialized);
        }

        // numeric formatting requires integer or float
        var needsNumeric =
            format.Equals("b", StringComparison.OrdinalIgnoreCase) ||
            format.Equals("x", StringComparison.OrdinalIgnoreCase) ||
            format.Equals("o", StringComparison.OrdinalIgnoreCase) ||
            format.StartsWith("f", StringComparison.OrdinalIgnoreCase);

        if (needsNumeric && !value.IsInteger() && !value.IsFloat())
        {
            throw new InvalidOperationError(token, "Expected an integer or float.");
        }

        string formatted;
        switch (format.ToLowerInvariant())
        {
            case "b":
            case "B":
                {
                    long num = value.IsInteger() ? value.GetInteger() : (long)value.GetFloat();
                    var bin = Convert.ToString(num, 2).PadLeft(16, '0');
                    formatted = bin;
                    break;
                }

            case "x":
            case "X":
                {
                    long num = value.IsInteger() ? value.GetInteger() : (long)value.GetFloat();
                    formatted = Convert.ToString(num, 16);
                    if (format == "X")
                    {
                        // Uppercase hex
                        formatted = formatted.ToUpperInvariant();
                    }
                    break;
                }

            case "o":
            case "O":
                {
                    long num = value.IsInteger() ? value.GetInteger() : (long)value.GetFloat();
                    formatted = Convert.ToString(num, 8);
                    if (format == "O")
                    {
                        formatted = formatted.ToUpperInvariant();
                    }
                    break;
                }

            default:
                {
                    // "f" or "fN" (fixed-point)
                    if (format.StartsWith("f", StringComparison.OrdinalIgnoreCase))
                    {
                        var precisionPart = format[1..];
                        var numericVal = value.IsInteger() ? value.GetInteger() : value.GetFloat();

                        try
                        {
                            if (string.IsNullOrEmpty(precisionPart))
                            {
                                // default to 0 decimals
                                formatted = numericVal.ToString("F0", System.Globalization.CultureInfo.InvariantCulture);
                            }
                            else
                            {
                                var precision = int.Parse(precisionPart);
                                var spec = "F" + precision;
                                formatted = numericVal.ToString(spec, System.Globalization.CultureInfo.InvariantCulture);
                            }
                        }
                        catch (Exception)
                        {
                            throw new InvalidOperationError(token, $"Invalid fixed-point format `{format}`.");
                        }
                    }
                    else
                    {
                        throw new InvalidOperationError(token, $"Unknown format specifier `{format}`.");
                    }

                    break;
                }
        }

        return Value.CreateString(formatted);
    }


    private static Value ExecuteSwap(Token token, Value value, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsInteger() && !args[1].IsInteger())
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Swap);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var firstIndex = (int)args[0].GetInteger();
        var secondIndex = (int)args[1].GetInteger();
        var lst = value.GetList();

        if (firstIndex < 0 || firstIndex >= lst.Count)
        {
            throw new RangeError(token);
        }

        if (secondIndex < 0 || secondIndex >= lst.Count)
        {
            throw new RangeError(token);
        }

        var firstValue = lst[firstIndex];
        var secondValue = lst[secondIndex];
        lst[firstIndex] = secondValue;
        lst[secondIndex] = firstValue;
        return value;
    }

    private static Value ExecuteRotate(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1 || !args[0].IsInteger())
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Rotate);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var elements = value.GetList();
        var rotation = (int)args[0].GetInteger();

        if (elements.Count == 0)
        {
            return value;
        }

        // normalize the rotation (mod by the list size)
        rotation %= elements.Count;
        if (rotation < 0)
        {
            // convert negative rotation to an equivalent positive rotation
            rotation += elements.Count;
        }

        var leftRotation = elements.Count - rotation;

        if (leftRotation > 0 && leftRotation < elements.Count)
        {
            var front = elements.GetRange(0, leftRotation);
            var back = elements.GetRange(leftRotation, elements.Count - leftRotation);

            elements.Clear();
            elements.AddRange(back);
            elements.AddRange(front);
        }

        return value;
    }

    private static Value ExecuteSlice(Token token, Value value, List<Value> args)
    {
        if (args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Slice);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var elements = value.GetList();
        var start = (int)args[0].GetInteger();
        var end = (int)args[1].GetInteger();

        if (start < 0 || end < 0 || start > end || end > elements.Count)
        {
            throw new IndexError(token);
        }

        var sliceSize = end - start;
        var slice = elements.GetRange(start, sliceSize);

        return Value.CreateList(slice);
    }

    private static Value ExecuteMerge(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Merge);
        }

        if (!value.IsHashmap())
        {
            throw new InvalidOperationError(token, "Expected a hashmap.");
        }

        if (!args[0].IsHashmap())
        {
            throw new InvalidOperationError(token, "Expected a hashmap.");
        }

        var targetMap = value.GetHashmap();
        var sourceMap = args[0].GetHashmap();

        foreach (var kvp in sourceMap)
        {
            targetMap[kvp.Key] = kvp.Value;
        }

        return value;
    }

    private static Value ExecuteZip(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1 || !args[0].IsList())
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Zip);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var elements1 = value.GetList();
        var elements2 = args[0].GetList();
        var zipped = new List<Value>();
        var winMin = Math.Min(elements1.Count, elements2.Count);

        for (var i = 0; i < winMin; i++)
        {
            List<Value> pair = [elements1[i], elements2[i]];
            zipped.Add(Value.CreateList(pair));
        }

        return Value.CreateList(zipped);
    }


    private static Value ExecuteFlatten(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Flatten);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var originalList = value.GetList();
        var flattened = FlattenListRecursive(originalList);

        return Value.CreateList(flattened);
    }

    private static List<Value> FlattenListRecursive(List<Value> input)
    {
        var result = new List<Value>();

        foreach (var item in input)
        {
            if (item.IsList())
            {
                var subList = item.GetList();
                var flattenedSubList = FlattenListRecursive(subList);
                result.AddRange(flattenedSubList);
            }
            else
            {
                result.Add(item);
            }
        }

        return result;
    }

    private static Value ExecuteInsert(Token token, Value value, List<Value> args)
    {
        if (args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Insert);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var lst = value.GetList();
        var index = 0;

        // note: in kiwi, the second parameter of `.insert()` is the index.
        if (!args[1].IsInteger())
        {
            throw new InvalidOperationError(token, "Expected an integer.");
        }

        index = (int)args[1].GetInteger();

        if (index >= lst.Count)
        {
            throw new IndexError(token);
        }

        lst.Insert(index, args[0]);
        return value;
    }

    private static Value ExecuteKeys(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Keys);
        }

        if (!value.IsHashmap())
        {
            throw new InvalidOperationError(token, "Expected a hashmap.");
        }

        var keys = value.GetHashmap().Keys.ToList();
        return Value.CreateList(keys);
    }

    private static Value ExecuteValues(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Values);
        }

        if (!value.IsHashmap())
        {
            throw new InvalidOperationError(token, "Expected a hashmap.");
        }

        var values = value.GetHashmap().Values.ToList();
        return Value.CreateList(values);
    }

    private static Value ExecuteEnqueue(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Enqueue);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        value.GetList().Add(args[0]);
        return value;
    }

    private static Value ExecuteDequeue(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Dequeue);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var lst = value.GetList();

        if (lst.Count == 0)
        {
            return Value.CreateNull();
        }

        var front = lst.First().Clone();
        lst.RemoveAt(0);
        return front;
    }

    private static Value ExecuteShift(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Shift);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var lst = value.GetList();

        if (lst.Count == 0)
        {
            return Value.CreateNull();
        }

        var front = lst.First().Clone();
        lst.RemoveAt(0);
        return front;
    }

    private static Value ExecuteUnshift(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Unshift);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        value.GetList().Insert(0, args[0]);
        return value;
    }

    private static Value ExecuteConcat(Token token, Value value, List<Value> args)
    {
        if (args.Count == 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Concat);
        }

        if (value.IsString())
        {
            System.Text.StringBuilder sv = new();
            sv.Append(value.GetString());

            foreach (var arg in args)
            {
                sv.Append(Serializer.Serialize(arg));
            }

            return Value.CreateString(sv.ToString());
        }
        else if (value.IsList())
        {
            var lst = value.GetList();

            foreach (var arg in args)
            {
                if (arg.IsList())
                {
                    lst.AddRange(arg.GetList());
                }
                else
                {
                    lst.Add(arg);
                }
            }

            return value;
        }

        throw new InvalidOperationError(token, "Expected a string or list.");
    }

    private static Value ExecuteUnique(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Unique);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var originalList = value.GetList();
        var uniqueList = new List<Value>();

        foreach (var item in originalList)
        {
            if (!uniqueList.Contains(item))
            {
                uniqueList.Add(item);
            }
        }

        return Value.CreateList(uniqueList);
    }


    private static Value ExecuteCount(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Count);
        }

        if (value.IsString())
        {
            if (!args[0].IsString())
            {
                throw new InvalidOperationError(token, "Expected a string to match against.");
            }

            var s = value.GetString();
            var target = args[0].GetString();
            var count = 0;

            if (string.IsNullOrEmpty(s) || string.IsNullOrEmpty(target) || target.Length > s.Length)
            {
                throw new InvalidOperationError(token, "Expected source string to be non-empty and longer than target string.");
            }

            for (var i = 0; i <= s.Length - target.Length; i++)
            {
                if (s.Substring(i, target.Length) == target)
                {
                    count++;
                }
            }

            return Value.CreateInteger(count);
        }
        else if (value.IsList())
        {
            var list = value.GetList();
            var count = 0;

            foreach (var item in list)
            {
                if (item.Equals(args[0]))
                {
                    count++;
                }
            }

            return Value.CreateInteger(count);
        }

        throw new InvalidOperationError(token, "Expected a string or list.");
    }


    private static Value ExecuteSubstring(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1 && args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Substring);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var s = value.GetString();
        var index = (int)args[0].GetInteger();
        var size = s.Length;

        if (args.Count == 2)
        {
            if (!args[1].IsInteger())
            {
                throw new InvalidOperationError(token, "Expected an integer.");
            }

            size = (int)args[1].GetInteger();
        }

        return Value.CreateString(s.Substring(index, Math.Min(size, s.Length - index)));
    }

    private static Value ExecuteReverse(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Reverse);
        }

        if (value.IsString())
        {
            return Value.CreateString(string.Join(string.Empty, value.GetString().Reverse()));
        }
        else if (value.IsList())
        {
            value.GetList().Reverse();
            return value;
        }

        throw new InvalidOperationError(token, "Expected a string or list.");
    }

    private static Value ExecuteIndexOf(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.IndexOf);
        }

        if (value.IsString())
        {
            if (!args[0].IsString())
            {
                throw new InvalidOperationError(token, "Expected a string.");
            }

            return Value.CreateInteger(value.GetString().IndexOf(args[0].GetString()));
        }
        else if (value.IsList())
        {
            return Value.CreateInteger(value.GetList().IndexOf(args[0]));
        }

        throw new InvalidOperationError(token, "Expected a string or list.");
    }

    private static Value ExecuteLastIndexOf(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.LastIndexOf);
        }

        if (value.IsString())
        {
            if (!args[0].IsString())
            {
                throw new InvalidOperationError(token, "Expected a string.");
            }

            return Value.CreateInteger(value.GetString().LastIndexOf(args[0].GetString()));
        }
        else if (value.IsList())
        {
            return Value.CreateInteger(value.GetList().LastIndexOf(args[0]));
        }

        throw new InvalidOperationError(token, "Expected a string or list.");
    }

    private static Value ExecuteClone(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Clone);
        }

        return value.Clone();
    }

    private static Value ExecuteLines(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Lines);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateList(value.GetString().Split(Environment.NewLine, StringSplitOptions.None).Select(x => Value.CreateString(x)).ToList());
    }

    private static Value ExecuteSplit(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Split);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var s = value.GetString();

        if (string.IsNullOrEmpty(s))
        {
            return Value.CreateList([]);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        var delim = args[0].GetString();

        if (string.IsNullOrEmpty(delim))
        {
            var chars = s.ToCharArray().Select(x => Value.CreateString(x.ToString())).ToList();
            return Value.CreateList(chars);
        }
        else
        {
            var tokens = s.Split(delim, StringSplitOptions.None).Select(x => Value.CreateString(x)).ToList();
            return Value.CreateList(tokens);
        }
    }

    private static Value ExecuteTruthy(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Truthy);
        }

        return Value.CreateBoolean(BooleanOp.IsTruthy(value));
    }

    private static Value ExecuteGet(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Get);
        }

        if (value.IsList())
        {
            if (!args[0].IsInteger())
            {
                throw new IndexError(token);
            }

            var index = (int)args[0].GetInteger();

            if (index < 0 || index >= value.GetList().Count)
            {
                throw new IndexError(token);
            }

            return value.GetList()[index];
        }
        else if (value.IsHashmap())
        {
            if (value.GetHashmap().TryGetValue(args[0], out Value? v))
            {
                return v;
            }
            else
            {
                return Value.CreateNull();
            }
        }

        throw new InvalidOperationError(token, "Expected a list or hashmap.");
    }

    private static Value ExecuteSet(Token token, Value value, List<Value> args)
    {
        if (args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Set);
        }

        if (value.IsList())
        {
            if (!args[0].IsInteger())
            {
                throw new IndexError(token);
            }

            var index = (int)args[0].GetInteger();

            if (index < 0 || index >= value.GetList().Count)
            {
                throw new IndexError(token);
            }

            value.GetList()[index] = args[1];
            return value;
        }
        else if (value.IsHashmap())
        {
            value.GetHashmap()[args[0]] = args[1];
        }

        throw new InvalidOperationError(token, "Expected a list or hashmap.");
    }

    private static Value ExecuteRemove(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Remove);
        }

        if (value.IsList())
        {
            value.GetList().Remove(args[0]);
            return value;
        }
        else if (value.IsHashmap())
        {
            value.GetHashmap().Remove(args[0]);
            return value;
        }

        throw new InvalidOperationError(token, "Expected a list or hashmap.");
    }

    private static Value ExecuteRemoveAt(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.RemoveAt);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        if (!args[0].IsInteger())
        {
            throw new InvalidOperationError(token, "Expected an integer.");
        }

        value.GetList().RemoveAt((int)args[0].GetInteger());
        return value;
    }

    private static Value ExecuteReplace(Token token, Value value, List<Value> args)
    {
        if (args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Replace);
        }

        var find = args[0];
        var replace = args[1];

        if (value.IsString())
        {
            if (!find.IsString() && !replace.IsString())
            {
                throw new InvalidOperationError(token, "Expected strings.");
            }

            return Value.CreateString(value.GetString().Replace(find.GetString(), replace.GetString()));
        }
        else if (value.IsList())
        {
            List<Value> lst = [];

            foreach (var item in value.GetList())
            {
                if (item.Equals(find))
                {
                    lst.Add(replace);
                }
                else
                {
                    lst.Add(item);
                }
            }

            return Value.CreateList(lst);
        }

        throw new InvalidOperationError(token, "Expected a string or a list.");
    }

    private static Value ExecuteClear(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Clear);
        }

        if (value.IsString())
        {
            value.SetValue(string.Empty);
            return value;
        }
        else if (value.IsList())
        {
            value.GetList().Clear();
            return value;
        }
        else if (value.IsHashmap())
        {
            value.GetHashmap().Clear();
            return value;
        }

        throw new InvalidOperationError(token, "Expected a string, list, or hashmap.");
    }

    private static Value ExecuteFirst(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.First);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var lst = value.GetList();

        if (lst.Count != 0)
        {
            return lst[0];
        }

        return Value.CreateNull();
    }

    private static Value ExecuteLast(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Last);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var lst = value.GetList();

        if (lst.Count != 0)
        {
            return lst[^1];
        }

        return Value.CreateNull();
    }

    private static Value ExecutePush(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Push);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        value.GetList().Add(args[0]);
        return value;
    }

    private static Value ExecutePop(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Pop);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var lst = value.GetList();
        if (lst.Count == 0)
        {
            return Value.CreateNull();
        }

        var back = lst[^1].Clone();
        lst.RemoveAt(lst.Count - 1);
        return back;
    }

    private static Value ExecuteChars(Token token, Value value, List<Value> args)
    {
        if (args.Count > 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Chars);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string to split.");
        }

        var s = value.GetString();

        if (string.IsNullOrEmpty(s))
        {
            return Value.CreateList([]);
        }

        var chars = s.ToCharArray().Select(x => Value.CreateString(x.ToString())).ToList();
        return Value.CreateList(chars);
    }

    private static Value ExecuteHasKey(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.HasKey);
        }

        if (!value.IsHashmap())
        {
            throw new InvalidOperationError(token, "Expected a hashmap.");
        }

        return Value.CreateBoolean(value.GetHashmap().ContainsKey(args[0]));
    }

    private static Value ExecuteJoin(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0 && args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Join);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list to join.");
        }

        var joiner = string.Empty;

        if (args.Count == 1)
        {
            if (!args[0].IsString())
            {
                throw new InvalidOperationError(token, "Expected a string for the joiner.");
            }

            joiner = args[0].GetString();
        }

        System.Text.StringBuilder sv = new();

        var first = true;
        foreach (var item in value.GetList())
        {
            if (!first)
            {
                sv.Append(joiner);
            }
            else
            {
                first = false;
            }
            sv.Append(Serializer.Serialize(item));
        }

        return Value.CreateString(sv.ToString());
    }

    private static Value ExecuteSize(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Size);
        }

        if (value.IsString())
        {
            return Value.CreateInteger(value.GetString().Length);
        }
        else if (value.IsList())
        {
            return Value.CreateInteger(value.GetList().Count);
        }
        else if (value.IsHashmap())
        {
            return Value.CreateInteger(value.GetHashmap().Count);
        }

        throw new InvalidOperationError(token, "Expected a string, list, or hashmap.");
    }

    private static Value ExecuteEmpty(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0 && args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Empty);
        }

        var isEmpty = false;
        if (value.IsString())
        {
            isEmpty = string.IsNullOrEmpty(value.GetString());
        }
        else if (value.IsList())
        {
            isEmpty = value.GetList().Count == 0;
        }
        else if (value.IsHashmap())
        {
            isEmpty = value.GetHashmap().Count == 0;
        }
        else if (value.IsInteger())
        {
            isEmpty = value.GetInteger() == 0L;
        }
        else if (value.IsFloat())
        {
            isEmpty = value.GetFloat() == 0D;
        }
        else if (value.IsBoolean())
        {
            isEmpty = !value.GetBoolean();
        }
        else if (value.IsNull())
        {
            isEmpty = true;
        }
        else
        {
            throw new InvalidOperationError(token, "Type cannot be empty.");
        }

        if (args.Count == 0)
        {
            return Value.CreateBoolean(isEmpty);
        }

        if (isEmpty)
        {
            return args[0];
        }

        return value;
    }

    private static Value ExecuteLowercase(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Lowercase);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateString(value.GetString().ToLower());
    }

    private static Value ExecuteUppercase(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Uppercase);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateString(value.GetString().ToUpper());
    }

    private static Value ExecuteLeftTrim(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.LeftTrim);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateString(value.GetString().TrimStart());
    }

    private static Value ExecuteRightTrim(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.RightTrim);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateString(value.GetString().TrimEnd());
    }

    private static Value ExecuteTrim(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Trim);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateString(value.GetString().Trim());
    }

    private static Value ExecuteType(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Type);
        }

        return Value.CreateString(Serializer.GetTypenameString(value));
    }

    private static Value ExecuteBeginsWith(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.BeginsWith);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateBoolean(value.GetString().StartsWith(args[0].GetString()));
    }

    private static Value ExecuteContains(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.Contains);
        }

        if (value.IsString())
        {
            if (!args[0].IsString())
            {
                throw new InvalidOperationError(token, "Expected a string.");
            }

            return Value.CreateBoolean(value.GetString().Contains(args[0].GetString()));
        }
        else if (value.IsList())
        {
            return Value.CreateBoolean(value.GetList().Contains(args[0]));
        }

        throw new InvalidOperationError(token, "Expected a string or list.");
    }

    private static Value ExecuteEndsWith(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, KiwiBuiltin.EndsWith);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateBoolean(value.GetString().EndsWith(args[0].GetString()));
    }
}