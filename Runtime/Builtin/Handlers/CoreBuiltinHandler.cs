using citrus.Runtime.Builtin.Operation;
using citrus.Parsing;
using citrus.Parsing.Keyword;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Runtime.Builtin.Handlers;

public static class CoreBuiltinHandler
{
    public static Value Execute(Token token, TokenName builtin, Value value, List<Value> args)
    {
        return builtin switch
        {
            TokenName.Builtin_Citrus_Chars => Chars(token, value, args),
            TokenName.Builtin_Citrus_Join => Join(token, value, args),
            TokenName.Builtin_Citrus_HasKey => HasKey(token, value, args),
            TokenName.Builtin_Citrus_Size => Size(token, value, args),
            TokenName.Builtin_Citrus_Uppercase => Uppercase(token, value, args),
            TokenName.Builtin_Citrus_Lowercase => Lowercase(token, value, args),
            TokenName.Builtin_Citrus_BeginsWith => BeginsWith(token, value, args),
            TokenName.Builtin_Citrus_Contains => Contains(token, value, args),
            TokenName.Builtin_Citrus_EndsWith => EndsWith(token, value, args),
            TokenName.Builtin_Citrus_LeftTrim => LeftTrim(token, value, args),
            TokenName.Builtin_Citrus_RightTrim => RightTrim(token, value, args),
            TokenName.Builtin_Citrus_Trim => Trim(token, value, args),
            TokenName.Builtin_Citrus_Empty => Empty(token, value, args),
            TokenName.Builtin_Citrus_Type => Type(token, value, args),
            TokenName.Builtin_Citrus_First => First(token, value, args),
            TokenName.Builtin_Citrus_Last => Last(token, value, args),
            TokenName.Builtin_Citrus_Push => Push(token, value, args),
            TokenName.Builtin_Citrus_Pop => Pop(token, value, args),
            TokenName.Builtin_Citrus_Clear => Clear(token, value, args),
            TokenName.Builtin_Citrus_Replace => Replace(token, value, args),
            TokenName.Builtin_Citrus_Remove => Remove(token, value, args),
            TokenName.Builtin_Citrus_RemoveAt => RemoveAt(token, value, args),
            TokenName.Builtin_Citrus_Get => Get(token, value, args),
            TokenName.Builtin_Citrus_Set => Set(token, value, args),
            TokenName.Builtin_Citrus_Truthy => Truthy(token, value, args),
            TokenName.Builtin_Citrus_Split => Split(token, value, args),
            TokenName.Builtin_Citrus_Lines => Lines(token, value, args),
            TokenName.Builtin_Citrus_Clone => Clone(token, value, args),
            TokenName.Builtin_Citrus_Reverse => Reverse(token, value, args),
            TokenName.Builtin_Citrus_IndexOf => IndexOf(token, value, args),
            TokenName.Builtin_Citrus_LastIndexOf => LastIndexOf(token, value, args),
            TokenName.Builtin_Citrus_Substring => Substring(token, value, args),
            TokenName.Builtin_Citrus_Concat => Concat(token, value, args),
            TokenName.Builtin_Citrus_Unique => Unique(token, value, args),
            TokenName.Builtin_Citrus_Count => Count(token, value, args),
            TokenName.Builtin_Citrus_Enqueue => Enqueue(token, value, args),
            TokenName.Builtin_Citrus_Dequeue => Dequeue(token, value, args),
            TokenName.Builtin_Citrus_Shift => Shift(token, value, args),
            TokenName.Builtin_Citrus_Unshift => Unshift(token, value, args),
            TokenName.Builtin_Citrus_Insert => Insert(token, value, args),
            TokenName.Builtin_Citrus_Keys => Keys(token, value, args),
            TokenName.Builtin_Citrus_Values => Values(token, value, args),
            TokenName.Builtin_Citrus_Zip => Zip(token, value, args),
            TokenName.Builtin_Citrus_Flatten => Flatten(token, value, args),
            TokenName.Builtin_Citrus_Merge => Merge(token, value, args),
            TokenName.Builtin_Citrus_Rotate => Rotate(token, value, args),
            TokenName.Builtin_Citrus_Slice => Slice(token, value, args),
            TokenName.Builtin_Citrus_IsA => IsA(token, value, args),
            TokenName.Builtin_Citrus_ToBytes => ToBytes(token, value, args),
            TokenName.Builtin_Citrus_ToHex => ToHex(token, value, args),
            TokenName.Builtin_Citrus_ToFloat => ToFloat(token, value, args),
            TokenName.Builtin_Citrus_ToInteger => ToInteger(token, value, args),
            TokenName.Builtin_Citrus_ToString => ToString(token, value, args),
            TokenName.Builtin_Citrus_ToDate => ToDate(token, value, args),
            TokenName.Builtin_Citrus_Swap => Swap(token, value, args),
            TokenName.Builtin_Citrus_Pretty => Pretty(token, value, args),
            /*
            TokenName.Builtin_Citrus_RReplace => RReplace(token, value, args),
            TokenName.Builtin_Citrus_RSplit => RSplit(token, value, args),
            TokenName.Builtin_Citrus_Find => Find(token, value, args),
            TokenName.Builtin_Citrus_Match => Match(token, value, args),
            TokenName.Builtin_Citrus_Matches => Matches(token, value, args),
            TokenName.Builtin_Citrus_MatchesAll => MatchesAll(token, value, args),
            TokenName.Builtin_Citrus_Scan => Scan(token, value, args),
            TokenName.Builtin_Citrus_Tokens => Tokens(token, value, args),
            */
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }

    private static Value Pretty(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Pretty);
        }

        var pretty = Serializer.PrettySerialize(value);
        return Value.CreateString(pretty);
    }

    private static Value IsA(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1 && !args[0].IsString())
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.IsA);
        }

        if (!args[0].IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
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

    private static Value ToBytes(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.ToBytes);
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

    private static Value ToHex(Token token, Value value, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.ToHex);
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

    private static Value ToFloat(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.ToF);
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

    private static Value ToInteger(Token token, Value value, List<Value> args)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.ToI);
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
                throw new InvalidOperationError(token, "Base must be between 2 and 36, inclusive.");
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

    private static Value ToDate(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0 && args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.ToD);
        }

        try
        {
            if (args.Count == 0)
            {
                if (value.IsString())
                {
                    if (!DateTime.TryParse(value.GetString(), out DateTime dt))
                    {
                        return Value.CreateDate(default);
                    }

                    return Value.CreateDate(dt);
                }
                else if (value.IsInteger())
                {
                    var dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(value.GetInteger());
                    return Value.CreateDate(dateTimeOffset.UtcDateTime);
                }
            }
            else if (args.Count == 1 && value.IsString() && args[0].IsString())
            {
                if (!DateTime.TryParseExact(value.GetString(), args[0].GetString(), null, System.Globalization.DateTimeStyles.None, out DateTime dt))
                {
                    return Value.CreateDate(default);
                }

                return Value.CreateDate(dt);
            }
        }
        catch
        {
            throw new InvalidOperationError(token, "Invalid date expression.");
        }

        throw new InvalidOperationError(token, "Invalid date expression.");
    }

    private static Value ToString(Token token, Value value, List<Value> args)
    {
        // 0 or 1 argument is allowed
        if (args.Count != 0 && args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.ToS);
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

        if (needsNumeric)
        {
            return GetFormattedNumericValue(token, value, format);
        }
        else if (value.IsDate())
        {
            return Value.CreateString(value.GetDate().ToString(format));
        }

        throw new InvalidOperationError(token, $"Unable to format type: {Serializer.GetTypenameString(value)}");
    }

    private static Value GetFormattedNumericValue(Token token, Value value, string format)
    {
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


    private static Value Swap(Token token, Value value, List<Value> args)
    {
        if (args.Count != 2 && !args[0].IsInteger() && !args[1].IsInteger())
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Swap);
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

    private static Value Rotate(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1 || !args[0].IsInteger())
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Rotate);
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

    private static Value Slice(Token token, Value value, List<Value> args)
    {
        if (args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Slice);
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

    private static Value Merge(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Merge);
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

    private static Value Zip(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1 || !args[0].IsList())
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Zip);
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


    private static Value Flatten(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Flatten);
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

    private static Value Insert(Token token, Value value, List<Value> args)
    {
        if (args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Insert);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        var lst = value.GetList();
        var index = 0;

        // note: in kiwi, the second parameter of `.insert()` is the index.
        if (!args[0].IsInteger())
        {
            throw new InvalidOperationError(token, "Expected an integer.");
        }

        index = (int)args[0].GetInteger();

        if (index >= lst.Count)
        {
            throw new IndexError(token);
        }

        lst.Insert(index, args[1]);
        return value;
    }

    private static Value Keys(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Keys);
        }

        if (!value.IsHashmap())
        {
            throw new InvalidOperationError(token, "Expected a hashmap.");
        }

        var keys = value.GetHashmap().Keys.ToList();
        return Value.CreateList(keys);
    }

    private static Value Values(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Values);
        }

        if (!value.IsHashmap())
        {
            throw new InvalidOperationError(token, "Expected a hashmap.");
        }

        var values = value.GetHashmap().Values.ToList();
        return Value.CreateList(values);
    }

    private static Value Enqueue(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Enqueue);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        value.GetList().Add(args[0].Clone());
        return value;
    }

    private static Value Dequeue(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Dequeue);
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

    private static Value Shift(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Shift);
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

    private static Value Unshift(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Unshift);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        value.GetList().Insert(0, args[0].Clone());
        return value;
    }

    private static Value Concat(Token token, Value value, List<Value> args)
    {
        if (args.Count == 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Concat);
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
                    lst.AddRange(arg.Clone().GetList());
                }
                else
                {
                    lst.Add(arg.Clone());
                }
            }

            return value;
        }

        throw new InvalidOperationError(token, "Expected a string or list.");
    }

    private static Value Unique(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Unique);
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

    private static Value Count(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Count);
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


    private static Value Substring(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1 && args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Substring);
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

    private static Value Reverse(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Reverse);
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

    private static Value IndexOf(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.IndexOf);
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

    private static Value LastIndexOf(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.LastIndexOf);
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

    private static Value Clone(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Clone);
        }

        return value.Clone();
    }

    private static Value Lines(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Lines);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateList(value.GetString().Split(Environment.NewLine, StringSplitOptions.None).Select(x => Value.CreateString(x)).ToList());
    }

    private static Value Split(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Split);
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

    private static Value Truthy(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Truthy);
        }

        return Value.CreateBoolean(BooleanOp.IsTruthy(value));
    }

    private static Value Get(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Get);
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

    private static Value Set(Token token, Value value, List<Value> args)
    {
        if (args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Set);
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

            value.GetList()[index] = args[1].Clone();
            return value;
        }
        else if (value.IsHashmap())
        {
            value.GetHashmap()[args[0]] = args[1].Clone();
            return value;
        }

        throw new InvalidOperationError(token, "Expected a list or hashmap.");
    }

    private static Value Remove(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Remove);
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

    private static Value RemoveAt(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.RemoveAt);
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

    private static Value Replace(Token token, Value value, List<Value> args)
    {
        if (args.Count != 2)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Replace);
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

    private static Value Clear(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Clear);
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

    private static Value First(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.First);
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

    private static Value Last(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Last);
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

    private static Value Push(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Push);
        }

        if (!value.IsList())
        {
            throw new InvalidOperationError(token, "Expected a list.");
        }

        value.GetList().Add(args[0].Clone());
        return value;
    }

    private static Value Pop(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Pop);
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

    private static Value Chars(Token token, Value value, List<Value> args)
    {
        if (args.Count > 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Chars);
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

    private static Value HasKey(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.HasKey);
        }

        if (!value.IsHashmap())
        {
            throw new InvalidOperationError(token, "Expected a hashmap.");
        }

        return Value.CreateBoolean(value.GetHashmap().ContainsKey(args[0]));
    }

    private static Value Join(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0 && args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Join);
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

    private static Value Size(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Size);
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

    private static Value Empty(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0 && args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Empty);
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

    private static Value Lowercase(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Lowercase);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateString(value.GetString().ToLower());
    }

    private static Value Uppercase(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Uppercase);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateString(value.GetString().ToUpper());
    }

    private static Value LeftTrim(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.LeftTrim);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateString(value.GetString().TrimStart());
    }

    private static Value RightTrim(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.RightTrim);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateString(value.GetString().TrimEnd());
    }

    private static Value Trim(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Trim);
        }

        if (!value.IsString())
        {
            throw new InvalidOperationError(token, "Expected a string.");
        }

        return Value.CreateString(value.GetString().Trim());
    }

    private static Value Type(Token token, Value value, List<Value> args)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Type);
        }

        return Value.CreateString(Serializer.GetTypenameString(value));
    }

    private static Value BeginsWith(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.BeginsWith);
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

    private static Value Contains(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.Contains);
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

    private static Value EndsWith(Token token, Value value, List<Value> args)
    {
        if (args.Count != 1)
        {
            throw new ParameterCountMismatchError(token, CitrusBuiltin.EndsWith);
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