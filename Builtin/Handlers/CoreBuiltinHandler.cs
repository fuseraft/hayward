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
            /*
            TokenName.Builtin_Kiwi_IsA => ExecuteIsA(token, value, args),
            TokenName.Builtin_Kiwi_RSplit => ExecuteRSplit(token, value, args),
            TokenName.Builtin_Kiwi_ToBytes => ExecuteToBytes(token, value, args),
            TokenName.Builtin_Kiwi_ToHex => ExecuteToHex(token, value, args),
            TokenName.Builtin_Kiwi_ToD => ExecuteToDouble(token, value, args),
            TokenName.Builtin_Kiwi_ToI => ExecuteToInteger(token, value, args),
            TokenName.Builtin_Kiwi_ToS => ExecuteToString(token, value, args),
            TokenName.Builtin_Kiwi_RReplace => ExecuteRReplace(token, value, args),
            TokenName.Builtin_Kiwi_Keys => ExecuteKeys(token, value, args),
            TokenName.Builtin_Kiwi_Merge => ExecuteMerge(token, value, args),
            TokenName.Builtin_Kiwi_Values => ExecuteValues(token, value, args),
            TokenName.Builtin_Kiwi_Enqueue => ExecuteEnqueue(token, value, args),
            TokenName.Builtin_Kiwi_Dequeue => ExecuteDequeue(token, value, args),
            TokenName.Builtin_Kiwi_Shift => ExecuteShift(token, value, args),
            TokenName.Builtin_Kiwi_Unshift => ExecuteUnshift(token, value, args),
            TokenName.Builtin_Kiwi_Rotate => ExecuteRotate(token, value, args),
            TokenName.Builtin_Kiwi_Insert => ExecuteInsert(token, value, args),
            TokenName.Builtin_Kiwi_Slice => ExecuteSlice(token, value, args),
            TokenName.Builtin_Kiwi_Swap => ExecuteSwap(token, value, args),
            TokenName.Builtin_Kiwi_Zip => ExecuteZip(token, value, args),
            TokenName.Builtin_Kiwi_Flatten => ExecuteFlatten(token, value, args),
            TokenName.Builtin_Kiwi_Pretty => ExecutePretty(token, value, args),
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

            for (int i = 0; i <= s.Length - target.Length; i++)
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