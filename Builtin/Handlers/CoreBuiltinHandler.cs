using citrus.Parsing;
using citrus.Tracing.Error;
using citrus.Typing;

namespace citrus.Builtin.Handlers;

public struct CoreBuiltinHandler
{

    public static Value Execute(Token token, TokenName builtin, Value value, List<Value> args)
    {
        return builtin switch
        {
            /*
            TokenName.Builtin_Kiwi_Chars => ExecuteChars(token, value, args),
            TokenName.Builtin_Kiwi_IsA => ExecuteIsA(token, value, args),
            TokenName.Builtin_Kiwi_Join => ExecuteJoin(token, value, args),
            TokenName.Builtin_Kiwi_Split => ExecuteSplit(token, value, args),
            TokenName.Builtin_Kiwi_RSplit => ExecuteRSplit(token, value, args),
            TokenName.Builtin_Kiwi_Get => ExecuteGet(token, value, args),
            TokenName.Builtin_Kiwi_Set => ExecuteSet(token, value, args),
            TokenName.Builtin_Kiwi_First => ExecuteFirst(token, value, args),
            TokenName.Builtin_Kiwi_Last => ExecuteLast(token, value, args),
            TokenName.Builtin_Kiwi_LeftTrim => ExecuteLeftTrim(token, value, args),
            TokenName.Builtin_Kiwi_RightTrim => ExecuteRightTrim(token, value, args),
            TokenName.Builtin_Kiwi_Trim => ExecuteTrim(token, value, args),
            TokenName.Builtin_Kiwi_Type => ExecuteType(token, value, args),
            TokenName.Builtin_Kiwi_Size => ExecuteSize(token, value, args),
            TokenName.Builtin_Kiwi_ToBytes => ExecuteToBytes(token, value, args),
            TokenName.Builtin_Kiwi_ToHex => ExecuteToHex(token, value, args),
            TokenName.Builtin_Kiwi_ToD => ExecuteToDouble(token, value, args),
            TokenName.Builtin_Kiwi_ToI => ExecuteToInteger(token, value, args),
            TokenName.Builtin_Kiwi_ToS => ExecuteToString(token, value, args),
            TokenName.Builtin_Kiwi_BeginsWith => ExecuteBeginsWith(token, value, args),
            TokenName.Builtin_Kiwi_Contains => ExecuteContains(token, value, args),
            TokenName.Builtin_Kiwi_EndsWith => ExecuteEndsWith(token, value, args),
            TokenName.Builtin_Kiwi_Replace => ExecuteReplace(token, value, args),
            TokenName.Builtin_Kiwi_RReplace => ExecuteRReplace(token, value, args),
            TokenName.Builtin_Kiwi_Reverse => ExecuteReverse(token, value, args),
            TokenName.Builtin_Kiwi_IndexOf => ExecuteIndexOf(token, value, args),
            TokenName.Builtin_Kiwi_LastIndexOf => ExecuteLastIndexOf(token, value, args),
            TokenName.Builtin_Kiwi_Uppercase => ExecuteUppercase(token, value, args),
            TokenName.Builtin_Kiwi_Lowercase => ExecuteLowercase(token, value, args),
            TokenName.Builtin_Kiwi_Empty => ExecuteEmpty(token, value, args),
            TokenName.Builtin_Kiwi_Keys => ExecuteKeys(token, value, args),
            TokenName.Builtin_Kiwi_HasKey => ExecuteHasKey(token, value, args),
            TokenName.Builtin_Kiwi_Merge => ExecuteMerge(token, value, args),
            TokenName.Builtin_Kiwi_Values => ExecuteValues(token, value, args),
            TokenName.Builtin_Kiwi_Push => ExecutePush(token, value, args),
            TokenName.Builtin_Kiwi_Pop => ExecutePop(token, value, args),
            TokenName.Builtin_Kiwi_Enqueue => ExecuteEnqueue(token, value, args),
            TokenName.Builtin_Kiwi_Dequeue => ExecuteDequeue(token, value, args),
            TokenName.Builtin_Kiwi_Shift => ExecuteShift(token, value, args),
            TokenName.Builtin_Kiwi_Unshift => ExecuteUnshift(token, value, args),
            TokenName.Builtin_Kiwi_Clear => ExecuteClear(token, value, args),
            TokenName.Builtin_Kiwi_Substring => ExecuteSubstring(token, value, args),
            TokenName.Builtin_Kiwi_Remove => ExecuteRemove(token, value, args),
            TokenName.Builtin_Kiwi_RemoveAt => ExecuteRemoveAt(token, value, args),
            TokenName.Builtin_Kiwi_Rotate => ExecuteRotate(token, value, args),
            TokenName.Builtin_Kiwi_Insert => ExecuteInsert(token, value, args),
            TokenName.Builtin_Kiwi_Slice => ExecuteSlice(token, value, args),
            TokenName.Builtin_Kiwi_Swap => ExecuteSwap(token, value, args),
            TokenName.Builtin_Kiwi_Concat => ExecuteConcat(token, value, args),
            TokenName.Builtin_Kiwi_Unique => ExecuteUnique(token, value, args),
            TokenName.Builtin_Kiwi_Count => ExecuteCount(token, value, args),
            TokenName.Builtin_Kiwi_Flatten => ExecuteFlatten(token, value, args),
            TokenName.Builtin_Kiwi_Zip => ExecuteZip(token, value, args),
            TokenName.Builtin_Kiwi_Clone => ExecuteClone(token, value, args),
            TokenName.Builtin_Kiwi_Pretty => ExecutePretty(token, value, args),
            TokenName.Builtin_Kiwi_Find => ExecuteFind(token, value, args),
            TokenName.Builtin_Kiwi_Match => ExecuteMatch(token, value, args),
            TokenName.Builtin_Kiwi_Matches => ExecuteMatches(token, value, args),
            TokenName.Builtin_Kiwi_MatchesAll => ExecuteMatchesAll(token, value, args),
            TokenName.Builtin_Kiwi_Scan => ExecuteScan(token, value, args),
            TokenName.Builtin_Kiwi_Truthy => ExecuteTruthy(token, value, args),
            TokenName.Builtin_Kiwi_Lines => ExecuteLines(token, value, args),
            TokenName.Builtin_Kiwi_Tokens => ExecuteTokens(token, value, args),
            */
            _ => throw new FunctionUndefinedError(token, token.Text),
        };
    }
}