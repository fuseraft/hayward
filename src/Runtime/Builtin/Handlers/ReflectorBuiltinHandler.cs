using hayward.Parsing;
using hayward.Parsing.Keyword;
using hayward.Runtime.Builtin.Operation;
using hayward.Tracing.Error;
using hayward.Typing;

namespace hayward.Runtime.Builtin.Handlers;

public class ReflectorBuiltinHandler
{
    public static Value Execute(Token token, TokenName op, List<Value> args, KContext context, Stack<StackFrame> callStack, Stack<string> funcStack)
    {
        return op switch
        {
            TokenName.Builtin_Reflector_RList => RList(token, args, context, callStack),
            TokenName.Builtin_Reflector_RObject => RObject(token, args, callStack),
            TokenName.Builtin_Reflector_RStack => RStack(token, args, funcStack),
            TokenName.Builtin_Reflector_RFFlags => RFFlags(token, args, callStack),
            TokenName.Builtin_Reflector_RRetVal => RRetVal(token, args, callStack),
            _ => throw new InvalidOperationError(token, "Come back later."),
        };
    }

    private static Value RFFlags(Token token, List<Value> args, Stack<StackFrame> callStack)
    {
        if (args.Count > 1)
        {
            throw new ParameterCountMismatchError(token, ReflectorBuiltin.RFFlags);
        }

        List<Value> list = [];
        int fromTop = 0;

        if (args.Count == 1)
        {
            fromTop = (int)ConversionOp.GetInteger(token, args[0]);

            if (fromTop < 0)
            {
                throw new InvalidOperationError(token, "Expected a positive integer for first parameter of `{ReflectorBuiltin.RFFlags}`.");
            }
        }

        Stack<StackFrame> tempStack = new([.. callStack]);

        if (fromTop > tempStack.Count)
        {
            throw new InvalidOperationError(token, $"Too many frames. Expected < {tempStack.Count} but instead received: {fromTop}");
        }

        int frameCounter = 0;

        while (tempStack.Count > 0)
        {
            var frame = tempStack.Peek();

            if (frameCounter - fromTop == 0)
            {
                if (frame.IsFlagSet(FrameFlags.Break))
                {
                    list.Add(Value.CreateInteger(FrameFlags.Break));
                }

                if (frame.IsFlagSet(FrameFlags.InLambda))
                {
                    list.Add(Value.CreateInteger(FrameFlags.InLambda));
                }

                if (frame.IsFlagSet(FrameFlags.InLoop))
                {
                    list.Add(Value.CreateInteger(FrameFlags.InLoop));
                }

                if (frame.IsFlagSet(FrameFlags.InObject))
                {
                    list.Add(Value.CreateInteger(FrameFlags.InObject));
                }

                if (frame.IsFlagSet(FrameFlags.InTry))
                {
                    list.Add(Value.CreateInteger(FrameFlags.InTry));
                }

                if (frame.IsFlagSet(FrameFlags.Next))
                {
                    list.Add(Value.CreateInteger(FrameFlags.Next));
                }

                if (frame.IsFlagSet(FrameFlags.None))
                {
                    list.Add(Value.CreateInteger(FrameFlags.None));
                }

                if (frame.IsFlagSet(FrameFlags.Return))
                {
                    list.Add(Value.CreateInteger(FrameFlags.Return));
                }

                if (frame.IsFlagSet(FrameFlags.SubFrame))
                {
                    list.Add(Value.CreateInteger(FrameFlags.SubFrame));
                }

                break;
            }

            frameCounter++;
            tempStack.Pop();
        }

        return Value.CreateList(list);
    }

    private static Value RRetVal(Token token, List<Value> args, Stack<StackFrame> callStack)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, ReflectorBuiltin.RRetVal);
        }

        if (callStack.Count == 0)
        {
            return Value.Default;
        }

        return callStack.Peek().ReturnValue ?? Value.Default;
    }

    private static Value RList(Token token, List<Value> args, KContext ctx, Stack<StackFrame> callStack)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, ReflectorBuiltin.RList);
        }

        Dictionary<Value, Value> rlist = [];
        List<Value> rlistPackages = [];
        List<Value> rlistStructs = [];
        List<Value> rlistFunctions = [];
        List<Value> rlistStack = [];

        var variablesKey = Value.CreateString("variables");
        var packagesKey = Value.CreateString("packages");
        var structsKey = Value.CreateString("structs");
        var functionsKey = Value.CreateString("functions");
        var stackKey = Value.CreateString("stack");

        foreach (var m in ctx.Functions)
        {
            rlistFunctions.Add(Value.CreateString(m.Key));
        }

        foreach (var p in ctx.Packages)
        {
            rlistPackages.Add(Value.CreateString(p.Key));
        }

        foreach (var c in ctx.Structs)
        {
            rlistStructs.Add(Value.CreateString(c.Key));
        }

        Stack<StackFrame> tempStack = new ([.. callStack]);

        while (tempStack.Count > 0)
        {
            var outerFrame = tempStack.Peek();
            var frameVariables = outerFrame.Variables;

            Dictionary<Value, Value> rlistStackFrame = [];
            List<Value> rlistStackFrameVariables = [];

            foreach (var v in frameVariables)
            {
                Dictionary<Value, Value> rlistStackFrameVariable = [];
                rlistStackFrameVariable[Value.CreateString(v.Key)] = v.Value;
                rlistStackFrameVariables.Add(Value.CreateHashmap(rlistStackFrameVariable));
            }

            rlistStackFrameVariables.Sort();

            rlistStackFrame[variablesKey] = Value.CreateList(rlistStackFrameVariables);
            rlistStack.Add(Value.CreateHashmap(rlistStackFrame));

            tempStack.Pop();
        }

        rlistPackages.Sort();
        rlistStructs.Sort();
        rlistFunctions.Sort();
        rlistStack.Reverse();

        rlist[packagesKey] = Value.CreateList(rlistPackages);
        rlist[structsKey] = Value.CreateList(rlistStructs);
        rlist[functionsKey] = Value.CreateList(rlistFunctions);
        rlist[stackKey] = Value.CreateList(rlistStack);

        return Value.CreateHashmap(rlist);
    }

    private static Value RObject(Token token, List<Value> args, Stack<StackFrame> callStack)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, ReflectorBuiltin.RObject);
        }

        if (callStack.Count == 0)
        {
            return Value.EmptyString;
        }

        var frame = callStack.Peek();
        if (frame.InObjectContext())
        {
            var obj = frame.GetObjectContext() ?? throw new NullObjectError(token);
            var serializedObject = Serializer.BasicSerializeObject(obj);
            return Value.CreateString(serializedObject);
        }

        return Value.EmptyString;
    }

    private static Value RStack(Token token, List<Value> args, Stack<string> funcStack)
    {
        if (args.Count != 0)
        {
            throw new ParameterCountMismatchError(token, ReflectorBuiltin.RStack);
        }

        Stack<string> tempStack = new ([.. funcStack]);
        List<Value> stackNames = [];

        while (tempStack.Count > 0)
        {
            stackNames.Add(Value.CreateString(tempStack.Peek()));
            tempStack.Pop();
        }

        return Value.CreateList(stackNames);
    }
}