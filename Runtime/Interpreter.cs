using citrus.Builtin;
using citrus.Builtin.Operation;
using citrus.Parsing;
using citrus.Parsing.AST;
using citrus.Typing;
using citrus.Tracing.Error;
using citrus.Runtime;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Reflection.Metadata;
using System;
namespace citrus.Runtime;

public class Interpreter
{
    const int SafemodeMaxIterations = 1000000;

    public bool Safemode { get; set; } = false;
    public KContext Context { get; private set; } = new();
    private Stack<StackFrame> CallStack { get; set; } = [];
    private Stack<string> PackageStack { get; set; } = [];
    private Stack<string> StructStack { get; set; } = [];
    private Stack<string> FuncStack { get; set; } = [];
    private Dictionary<string, string> CliArgs { get; set; } = [];
    private static Value DefaultValue = Value.Default();

    public void SetContext(KContext context) => Context = context;

    public Value Interpret(ASTNode? node)
    {
        if (node == null)
        {
            return Value.Default();
        }

        bool sigCheck = RequiresSigCheck(node.Type);

        Value result = node.Type switch
        {
            ASTNodeType.Program => Visit((ProgramNode)node),
            ASTNodeType.Self => Visit((SelfNode)node),
            ASTNodeType.Package => Visit((PackageNode)node),
            ASTNodeType.Struct => Visit((StructNode)node),
            ASTNodeType.Import => Visit((ImportNode)node),
            ASTNodeType.Export => Visit((ExportNode)node),
            ASTNodeType.Exit => Visit((ExitNode)node),
            ASTNodeType.Throw => Visit((ThrowNode)node),
            ASTNodeType.Assignment => Visit((AssignmentNode)node),
            ASTNodeType.ConstAssignment => Visit((ConstAssignmentNode)node),
            ASTNodeType.IndexAssignment => Visit((IndexAssignmentNode)node),
            ASTNodeType.MemberAssignment => Visit((MemberAssignmentNode)node),
            ASTNodeType.PackAssignment => Visit((PackAssignmentNode)node),
            ASTNodeType.MemberAccess => Visit((MemberAccessNode)node),
            ASTNodeType.Literal => Visit((LiteralNode)node),
            ASTNodeType.ListLiteral => Visit((ListLiteralNode)node),
            ASTNodeType.RangeLiteral => Visit((RangeLiteralNode)node),
            ASTNodeType.HashLiteral => Visit((HashLiteralNode)node),
            ASTNodeType.Identifier => Visit((IdentifierNode)node),
            ASTNodeType.Print => Visit((PrintNode)node),
            ASTNodeType.PrintXy => Visit((PrintXyNode)node),
            ASTNodeType.TernaryOperation => Visit((TernaryOperationNode)node),
            ASTNodeType.BinaryOperation => Visit((BinaryOperationNode)node),
            ASTNodeType.UnaryOperation => Visit((UnaryOperationNode)node),
            ASTNodeType.If => Visit((IfNode)node),
            ASTNodeType.Case => Visit((CaseNode)node),
            ASTNodeType.ForLoop => Visit((ForLoopNode)node),
            ASTNodeType.WhileLoop => Visit((WhileLoopNode)node),
            ASTNodeType.RepeatLoop => Visit((RepeatLoopNode)node),
            ASTNodeType.Break => Visit((BreakNode)node),
            ASTNodeType.Next => Visit((NextNode)node),
            ASTNodeType.Try => Visit((TryNode)node),
            ASTNodeType.Lambda => Visit((LambdaNode)node),
            ASTNodeType.LambdaCall => Visit((LambdaCallNode)node),
            ASTNodeType.Function => Visit((FunctionNode)node),
            ASTNodeType.Variable => Visit((VariableNode)node),
            ASTNodeType.FunctionCall => Visit((FunctionCallNode)node),
            ASTNodeType.MethodCall => Visit((MethodCallNode)node),
            ASTNodeType.Return => Visit((ReturnNode)node),
            ASTNodeType.Index => Visit((IndexingNode)node),
            ASTNodeType.Slice => Visit((SliceNode)node),
            ASTNodeType.Parse => Value.Default(),
            ASTNodeType.NoOp => Value.Default(),
            ASTNodeType.Spawn => Value.Default(),
            _ => PrintNode(node),
        };

        if (sigCheck)
        {
            // HandlePendingSignals(node.Token);
        }

        return result;
    }

    private static bool RequiresSigCheck(ASTNodeType type)
    {
        return type is ASTNodeType.Program
            or ASTNodeType.Export
            or ASTNodeType.Try or ASTNodeType.If or ASTNodeType.Case or
            ASTNodeType.ForLoop or ASTNodeType.WhileLoop or ASTNodeType.RepeatLoop or
            ASTNodeType.LambdaCall or ASTNodeType.FunctionCall or ASTNodeType.MethodCall;
    }

    private static Value PrintNode(ASTNode node)
    {
        node.Print();
        return Value.Default();
    }

    /// <summary>
    /// The entry point of the program.
    /// </summary>
    /// <param name="node">The root node.</param>
    /// <returns></returns>
    private Value Visit(ProgramNode node)
    {
        // This is the program root
        if (!node.IsEntryPoint)
        {
            StackFrame programFrame = new()
            {
                Name = "citrus"
            };
            programFrame.Variables["global"] = Value.CreateHashmap();

            PushFrame(programFrame);
        }

        Value result = DefaultValue;

        foreach (var stmt in node.Statements)
        {
            result = Interpret(stmt);
        }

        if (result.IsInteger())
        {
            return result;
        }

        return Value.CreateInteger(0L);
    }

    private Value Visit(SelfNode node) => throw new NotImplementedException();
    private Value Visit(PackageNode node) => throw new NotImplementedException();
    private Value Visit(StructNode node) => throw new NotImplementedException();
    private Value Visit(ExportNode node) => throw new NotImplementedException();
    private Value Visit(ImportNode node) => throw new NotImplementedException();

    /// <summary>
    /// Handle the exit node.
    /// </summary>
    /// <param name="node">The node.</param>
    private Value Visit(ExitNode node)
    {
        if (node.Condition == null || BooleanOp.IsTruthy(Interpret(node.Condition)))
        {
            var exitValue = Interpret(node.ExitValue);
            long exitCode = exitValue.IsInteger() ? exitValue.GetInteger() : 1L;
            Environment.Exit(Convert.ToInt32(exitCode));
        }

        return Value.Default();
    }

    private Value Visit(ThrowNode node)
    {
        const string DefaultErrorType = "KiwiError";

        if (node.Condition == null || BooleanOp.IsTruthy(Interpret(node.Condition)))
        {
            string errorType = DefaultErrorType;
            string errorMessage = string.Empty;
            if (node.ErrorValue != null)
            {
                var errorValue = Interpret(node.ErrorValue);

                if (errorValue.IsHashmap())
                {
                    var errorHash = errorValue.GetHashmap();
                    var errorKey = Value.CreateString("error");
                    var messageKey = Value.CreateString("message");

                    if (errorHash.ContainsKey(errorKey) && errorHash[errorKey].IsString())
                    {
                        errorType = errorHash[errorKey].GetString();
                    }
                    if (errorHash.ContainsKey(messageKey) && errorHash[messageKey].IsString())
                    {
                        errorMessage = errorHash[messageKey].GetString();
                    }
                }
                else if (errorValue.IsString())
                {
                    errorMessage = errorValue.GetString();
                }
            }

            throw new KiwiError(node.Token, errorType, errorMessage);
        }

        return Value.Default();
    }

    private Value Visit(AssignmentNode node)
    {
        const string Global = "global";

        var frame = CallStack.Peek();
        var value = Interpret(node.Initializer);
        var type = node.Op;
        var name = node.Name;

        if (type == TokenName.Ops_Assign)
        {
            if (Global.Equals(name) || Context.HasConstant(name))
            {
                throw new IllegalNameError(node.Token, name);
            }

            if (value.IsLambda())
            {
                // WIP: need to work on this.
                var lambdaId = value.GetLambda().Identifier;
                Context.Lambdas.Add(name, Context.Lambdas[lambdaId]);
                return value;
            }
            else
            {
                if (frame.InObjectContext() && (node.Left?.Type == ASTNodeType.Self || name[0] == '@'))
                {
                    var obj = frame.GetObjectContext();
                    if (obj != null)
                    {
                        obj.InstanceVariables[name] = value;
                        return obj.InstanceVariables[name];
                    }
                }

                if (value.IsObject())
                {
                    var obj = value.GetObject();
                    obj.Identifier = name;
                }

                frame.Variables[name] = value;
            }
        }
        else
        {
            if (Context.HasConstant(name))
            {
                throw new IllegalNameError(node.Token, name);
            }

            if (frame.HasVariable(name))
            {
                var oldValue = frame.Variables[name];

                if (type == TokenName.Ops_BitwiseNotAssign)
                {
                    frame.Variables[name] = BitwiseOp.Not(node.Token, ref oldValue);
                }
                else
                {
                    frame.Variables[name] = OpDispatch.DoBinary(node.Token, type, ref oldValue, ref value);
                }

                return frame.Variables[name];
            }
            else if (frame.InObjectContext())
            {
                var obj = frame.GetObjectContext();

                if (obj == null)
                {
                    return Value.Default();
                }

                if (!obj.HasVariable(name))
                {
                    throw new VariableUndefinedError(node.Token, name);
                }

                var oldValue = obj.InstanceVariables[name];

                if (type == TokenName.Ops_BitwiseNotAssign)
                {
                    obj.InstanceVariables[name] = BitwiseOp.Not(node.Token, ref oldValue);
                }
                else
                {
                    obj.InstanceVariables[name] = OpDispatch.DoBinary(node.Token, type, ref oldValue, ref value);
                }

                return obj.InstanceVariables[name];
            }

            throw new VariableUndefinedError(node.Token, name);
        }

        return frame.Variables[name];
    }

    private Value Visit(ConstAssignmentNode node)
    {

        var frame = CallStack.Peek();
        var name = node.Name;
        var value = Interpret(node.Initializer);

        if (PackageStack.Count > 0)
        {
            Stack<string> tmpStack = PackageStack;
            var prefix = string.Empty;
            while (tmpStack.Count > 0)
            {
                prefix += tmpStack.Peek() + ".";
                tmpStack.Pop();
            }
            name = prefix + name;
        }

        if (Context.HasConstant(name))
        {
            throw new IllegalNameError(node.Token, name);
        }

        Context.Constants.Add(name, value);

        return Value.Default();
    }

    private Value Visit(IndexAssignmentNode node) => throw new NotImplementedException();
    private Value Visit(MemberAssignmentNode node)
    {

        var obj = Interpret(node.Object);
        var memberName = node.MemberName;
        var initializer = Interpret(node.Initializer);

        if (obj.IsHashmap())
        {
            var hash = obj.GetHashmap();
            var memberKey = Value.CreateString(memberName);

            if (node.Op == TokenName.Ops_Assign)
            {
                hash.Add(memberKey, initializer);
            }
            else if (hash.TryGetValue(memberKey, out Value? value))
            {
                var newValue = OpDispatch.DoBinary(node.Token, node.Op, ref value, ref initializer);
                hash.Add(memberKey, newValue);
            }
            else
            {
                throw new HashKeyError(node.Token, memberName);
            }
        }

        return Value.Default();
    }

    private Value Visit(PackAssignmentNode node)
    {
        var frame = CallStack.Peek();

        List<Value> rhsValues = [];
        foreach (var rhs in node.Right)
        {
            rhsValues.Add(Interpret(rhs));
        }

        var rhsPosition = 0;
        var lhsLength = node.Left.Count;

        // unpack
        if (rhsValues.Count == 1)
        {
            List<Value> unpacked = [];
            var rhsValue = rhsValues.First();
            if (!rhsValue.IsList())
            {
                throw new InvalidOperationError(node.Token, "Expected a list to unpack.");
            }

            foreach (var rhs in rhsValue.GetList())
            {
                unpacked.Add(rhs);
            }

            rhsValues.Clear();
            rhsValues = unpacked;
        }

        foreach (var lhs in node.Left)
        {
            if (lhs == null)
            {
                continue;
            }

            var identifierName = Id(lhs);
            if (rhsValues.Count == lhsLength)
            {
                frame.Variables[identifierName] = rhsValues[rhsPosition++];
            }
            else
            {
                frame.Variables[identifierName] = Value.CreateNull();
            }
        }

        return Value.Default();
    }

    private Value Visit(MemberAccessNode node) => throw new NotImplementedException();

    private static Value Visit(LiteralNode node) => node.Value;

    private Value Visit(RangeLiteralNode node)
    {
        var startValue = Interpret(node.RangeStart);
        var stopValue = Interpret(node.RangeEnd);

        if (!startValue.IsInteger() || !stopValue.IsInteger())
        {
            throw new RangeError(node.Token, "Range value must be an integer.");
        }

        var start = startValue.GetInteger();
        var stop = stopValue.GetInteger();
        var step = (stop < start) ? -1 : 1;

        List<Value> list = [];

        for (var i = start; i != stop; i += step)
        {
            list.Add(Value.CreateInteger(i));
        }

        list.Add(Value.CreateInteger(stop));

        return Value.CreateList(list);
    }

    private Value Visit(ListLiteralNode node)
    {
        List<Value> list = [];

        foreach (var element in node.Elements)
        {
            var value = Interpret(element);
            list.Add(value);
        }

        return Value.CreateList(list);
    }

    private Value Visit(HashLiteralNode node)
    {
        Dictionary<Value, Value> hash = [];

        foreach (var pair in node.Elements)
        {
            var key = Interpret(pair.Key);
            var value = Interpret(pair.Value);
            hash[key] = value;
        }

        return Value.CreateHashmap(hash);
    }

    private Value Visit(IdentifierNode node)
    {
        var frame = CallStack.Peek();
        var name = node.Name;

        if (frame.InObjectContext() && name[0] == '@')
        {
            return frame.GetObjectContext()?.InstanceVariables[name] ?? Value.CreateNull();
        }

        if (frame.HasVariable(name))
        {
            return frame.Variables[name];
        }
        else if (Context.HasStruct(name))
        {
            return Value.CreateStruct(new StructRef { Identifier = name });
        }
        else if (Context.HasLambda(name))
        {
            return Value.CreateLambda(new LambdaRef { Identifier = name });
        }
        else if (Context.HasMappedLambda(name))
        {
            var id = Context.LambdaTable[name];

            if (Context.HasLambda(id))
            {
                return Value.CreateLambda(new LambdaRef { Identifier = id });
            }
        }
        else if (Context.HasConstant(name))
        {
            return Context.Constants[name];
        }

        return Value.CreateNull();
    }

    private Value Visit(PrintNode node)
    {
        var value = Interpret(node.Expression);
        var serializedValue = Serializer.Serialize(value);

        if (node.PrintStdError)
        {
            if (node.PrintNewline)
            {
                Console.Error.WriteLine(serializedValue);
            }
            else
            {
                Console.Error.Write(serializedValue);
            }
        }
        else
        {
            if (node.PrintNewline)
            {
                Console.WriteLine(serializedValue);
            }
            else
            {
                Console.Write(serializedValue);
            }
        }

        return Value.Default();
    }

    private Value Visit(PrintXyNode node) => throw new NotImplementedException();

    private Value Visit(TernaryOperationNode node)
    {
        var eval = Interpret(node.EvalExpression);

        if (BooleanOp.IsTruthy(eval))
        {
            return Interpret(node.TrueExpression);
        }

        return Interpret(node.FalseExpression);
    }

    private Value Visit(BinaryOperationNode node)
    {
        var left = Interpret(node.Left);

        if (node.Op == TokenName.Ops_And && !BooleanOp.IsTruthy(left))
        {
            return Value.CreateBoolean(false);
        }
        else if (node.Op == TokenName.Ops_Or && BooleanOp.IsTruthy(left))
        {
            return Value.CreateBoolean(true);
        }

        var right = Interpret(node.Right);
        return OpDispatch.DoBinary(node.Token, node.Op, ref left, ref right);
    }

    private Value Visit(UnaryOperationNode node)
    {
        var right = Interpret(node.Operand);
        return OpDispatch.DoUnary(node.Token, node.Op, ref right);
    }

    private Value Visit(IfNode node)
    {
        var conditionValue = Interpret(node.Condition);
        var frame = CallStack.Peek();
        var result = Value.Default();

        if (BooleanOp.IsTruthy(conditionValue))
        {
            foreach (var stmt in node.Body)
            {
                result = Interpret(stmt);
                if (frame.IsFlagSet(FrameFlags.Return))
                {
                    break;
                }
            }
        }
        else
        {
            bool executed = false;
            foreach (var elsifNode in node.ElsifNodes)
            {
                var elsifCondition = Interpret(elsifNode?.Condition);

                if (BooleanOp.IsTruthy(elsifCondition))
                {
                    if (elsifNode == null)
                    {
                        continue;
                    }

                    foreach (var stmt in elsifNode.Body)
                    {
                        result = Interpret(stmt);
                        if (frame.IsFlagSet(FrameFlags.Return))
                        {
                            break;
                        }
                    }
                    executed = true;
                    break;
                }
            }

            if (!executed && node.ElseBody.Count > 0)
            {
                foreach (var stmt in node.ElseBody)
                {
                    result = Interpret(stmt);
                    if (frame.IsFlagSet(FrameFlags.Return))
                    {
                        break;
                    }
                }
            }
        }

        return result;
    }

    private Value Visit(CaseNode node)
    {
        var testValue = Interpret(node.TestValue);
        var result = Value.Default();

        foreach (var whenNode in node.WhenNodes)
        {
            Value whenCondition = Interpret(whenNode);

            if (ComparisonOp.Equal(ref testValue, ref whenCondition))
            {
                var frame = CallStack.Peek();
                foreach (var stmt in whenNode.Body)
                {
                    result = Interpret(stmt);
                    if (frame.IsFlagSet(FrameFlags.Return))
                    {
                        result = frame.ReturnValue ?? result;
                        break;
                    }
                }

                return result;
            }
        }

        if (node.ElseBody.Count > 0)
        {
            var frame = CallStack.Peek();

            foreach (var stmt in node.ElseBody)
            {
                result = Interpret(stmt);

                if (frame.IsFlagSet(FrameFlags.Return))
                {
                    return frame.ReturnValue ?? result;
                }
            }
        }

        return result;
    }

    private Value Visit(ForLoopNode node)
    {
        var dataSetValue = Interpret(node.DataSet);

        if (dataSetValue.IsList())
        {
            return ListLoop(node, dataSetValue.GetList());
        }
        else if (dataSetValue.IsHashmap())
        {
            return HashmapLoop(node, dataSetValue.GetHashmap());
        }

        throw new InvalidOperationError(node.Token, "Expected a list value in for-loop.");
    }

    private Value Visit(WhileLoopNode node)
    {
        var result = Value.Default();
        var frame = CallStack.Peek();
        frame.SetFlag(FrameFlags.InLoop);

        var fallOut = false;
        var iterations = 0;

        while (BooleanOp.IsTruthy(Interpret(node.Condition)))
        {
            if (Safemode)
            {
                ++iterations;

                if (iterations == SafemodeMaxIterations)
                {
                    throw new InfiniteLoopError(node.Token, "Detected an infinite loop in safemode.");
                }
            }

            foreach (var stmt in node.Body)
            {
                if (stmt == null)
                {
                    continue;
                }

                if (stmt.Type != ASTNodeType.Next && stmt.Type != ASTNodeType.Break)
                {
                    result = Interpret(stmt);

                    if (frame.IsFlagSet(FrameFlags.Break))
                    {
                        frame.ClearFlag(FrameFlags.Break);
                        fallOut = true;
                        break;
                    }

                    if (frame.IsFlagSet(FrameFlags.Next))
                    {
                        frame.ClearFlag(FrameFlags.Next);
                        break;
                    }
                }

                if (frame.IsFlagSet(FrameFlags.Return))
                {
                    fallOut = true;
                    break;
                }

                if (stmt.Type == ASTNodeType.Next)
                {
                    var condition = ((NextNode)stmt).Condition;
                    if (condition == null || BooleanOp.IsTruthy(Interpret(condition)))
                    {
                        break;
                    }
                }
                else if (stmt.Type == ASTNodeType.Break)
                {
                    var condition = ((BreakNode)stmt).Condition;
                    if (condition == null || BooleanOp.IsTruthy(Interpret(condition)))
                    {
                        fallOut = true;
                        break;
                    }
                }
            }

            if (fallOut)
            {
                break;
            }
        }

        frame.ClearFlag(FrameFlags.InLoop);

        return result;
    }

    private Value Visit(RepeatLoopNode node)
    {
        var countValue = Interpret(node.Count);

        if (!countValue.IsInteger())
        {
            throw new InvalidOperationError(node.Token, "Repeat loop count must be an integer.");
        }

        var count = countValue.GetInteger();
        var aliasName = string.Empty;
        var result = Value.Default();
        var aliasValue = Value.Default();
        bool hasAlias = false;

        if (node.Alias != null)
        {
            aliasName = Id(node.Alias);
            hasAlias = true;
        }

        var frame = CallStack.Peek();
        frame.SetFlag(FrameFlags.InLoop);

        var variables = frame.Variables;
        ASTNodeType statement = ASTNodeType.NoOp;

        var fallOut = false;
        for (int i = 1; i <= count; ++i)
        {
            if (fallOut)
            {
                break;
            }

            if (hasAlias)
            {
                aliasValue.SetValue(i);
                variables[aliasName] = aliasValue;
            }

            foreach (var stmt in node.Body)
            {
                if (stmt == null)
                {
                    continue;
                }

                statement = stmt.Type;
                if (statement != ASTNodeType.Next && statement != ASTNodeType.Break)
                {
                    result = Interpret(stmt);

                    if (frame.IsFlagSet(FrameFlags.Break))
                    {
                        frame.ClearFlag(FrameFlags.Break);
                        fallOut = true;
                        break;
                    }

                    if (frame.IsFlagSet(FrameFlags.Next))
                    {
                        frame.ClearFlag(FrameFlags.Next);
                        break;
                    }
                }

                if (frame.IsFlagSet(FrameFlags.Return))
                {
                    break;
                }

                if (statement == ASTNodeType.Next)
                {
                    var condition = ((NextNode)stmt).Condition;
                    if (condition == null || BooleanOp.IsTruthy(Interpret(condition)))
                    {
                        break;
                    }
                }
                else if (statement == ASTNodeType.Break)
                {
                    var condition = ((BreakNode)stmt).Condition;
                    if (condition == null || BooleanOp.IsTruthy(Interpret(condition)))
                    {
                        fallOut = true;
                        break;
                    }
                }
            }
        }

        if (hasAlias)
        {
            variables.Remove(aliasName);
        }

        frame.ClearFlag(FrameFlags.InLoop);

        return result;
    }

    private Value Visit(BreakNode node)
    {
        var condition = node.Condition;
        if (condition == null || BooleanOp.IsTruthy(Interpret(condition)))
        {
            var frame = CallStack.Peek();
            frame.SetFlag(FrameFlags.Break);
        }

        return Value.Default();
    }

    private Value Visit(NextNode node)
    {
        var condition = node.Condition;
        if (condition == null || BooleanOp.IsTruthy(Interpret(condition)))
        {
            var frame = CallStack.Peek();
            frame.SetFlag(FrameFlags.Next);
        }

        return Value.Default();
    }

    private Value Visit(TryNode node)
    {
        var returnValue = Value.Default();
        bool requireDrop = false;
        bool setReturnValue = false;

        try
        {
            var tryFrame = CreateFrame("try");
            tryFrame.SetFlag(FrameFlags.InTry);
            requireDrop = PushFrame(tryFrame);

            foreach (var stmt in node.TryBody)
            {
                Interpret(stmt);
                if (tryFrame.IsFlagSet(FrameFlags.Return))
                {
                    returnValue = tryFrame.ReturnValue ?? returnValue;
                    setReturnValue = true;
                    break;
                }
            }

            DropFrame();
        }
        catch (KiwiError e)
        {
            if (requireDrop)
            {
                DropFrame();
            }

            if (node.CatchBody.Count > 0)
            {
                var catchFrame = CreateFrame("catch");
                requireDrop = false;

                var errorTypeName = string.Empty;
                var errorMessageName = string.Empty;

                try
                {
                    if (node.ErrorType != null)
                    {
                        errorTypeName = Id(node.ErrorType);
                        catchFrame.Variables[errorTypeName] = Value.CreateString(e.Type);
                    }

                    if (node.ErrorMessage != null)
                    {
                        errorMessageName = Id(node.ErrorMessage);
                        catchFrame.Variables[errorMessageName] = Value.CreateString(e.Message);
                    }

                    requireDrop = PushFrame(catchFrame);

                    foreach (var stmt in node.CatchBody)
                    {
                        Interpret(stmt);
                        if (catchFrame.IsFlagSet(FrameFlags.Return))
                        {
                            returnValue = catchFrame.ReturnValue ?? returnValue;
                            setReturnValue = true;
                            break;
                        }
                    }

                    if (node.ErrorType != null)
                    {
                        catchFrame.Variables.Remove(errorTypeName);
                    }

                    if (node.ErrorMessage != null)
                    {
                        catchFrame.Variables.Remove(errorMessageName);
                    }

                    DropFrame();
                }
                catch (KiwiError)
                {
                    if (requireDrop && InTry())
                    {
                        DropFrame();
                    }
                    throw;
                }
            }
        }

        if (node.FinallyBody.Count > 0)
        {
            var frame = CallStack.Peek();
            foreach (var stmt in node.FinallyBody)
            {
                Interpret(stmt);
                if (frame.IsFlagSet(FrameFlags.Return))
                {
                    returnValue = frame.ReturnValue ?? returnValue;
                    setReturnValue = true;
                    break;
                }
            }
        }

        if (setReturnValue)
        {
            var frame = CallStack.Peek();

            if (!frame.IsFlagSet(FrameFlags.InLambda))
            {
                frame.SetFlag(FrameFlags.Return);
                frame.ReturnValue = returnValue;
            }
        }

        return returnValue;
    }

    private Value Visit(LambdaNode node) => throw new NotImplementedException();
    private Value Visit(FunctionNode node) => throw new NotImplementedException();
    private Value Visit(VariableNode node)
    {
        var typeHints = node.TypeHints;

        // we're going to be injecting these into the stack frame
        var frame = CallStack.Peek();

        // for each declared variable
        foreach (var pair in node.Variables)
        {
            var value = Value.Default(); // a variable value
            var name = pair.Key;  // grab the name
                                  // a flag to determine if a variable has an initializer
            bool hasDefaultValue = pair.Value != null;

            // if there is a default value, grab it
            if (!hasDefaultValue)
            {
                // default to null
                value = Value.CreateNull();
            }
            else
            {
                value = Interpret(pair.Value);

                // if the value is a lambda, register to the lambda map
                if (value.IsLambda())
                {
                    var lambdaName = value.GetLambda().Identifier;
                    Context.AddMappedLambda(name, lambdaName);
                }
            }

            // check for a type-hint
            if (typeHints.TryGetValue(name, out TokenName expectedType))
            {
                // if a default value was supplied, expect it to match the type
                if (hasDefaultValue && !Serializer.AssertTypematch(value, expectedType))
                {
                    throw new TypeError(node.Token, $"Expected type `{Serializer.GetTypenameString(expectedType)}` for variable  `{ASTTracer.Unmangle(name)}` but received `{Serializer.GetTypenameString(value)}`.");
                }
                else if (!hasDefaultValue)
                {
                    // give it a default value based on the type-hint
                    switch (expectedType)
                    {
                        case TokenName.Types_Boolean:
                            value.SetValue(false);
                            break;

                        case TokenName.Types_Integer:
                            value.SetValue(0);
                            break;

                        case TokenName.Types_Float:
                            value.SetValue(0.0);
                            break;

                        case TokenName.Types_String:
                            value.SetValue("");
                            break;

                        case TokenName.Types_List:
                            value.SetValue(Value.CreateList());
                            break;

                        case TokenName.Types_Hash:
                            value.SetValue(Value.CreateHashmap());
                            break;

                        default:
                            break;
                    }
                }
            }

            // inject the variable
            frame.Variables[name] = value;
        }

        return Value.Default();
    }

    private Value Visit(LambdaCallNode node) => throw new NotImplementedException();

    private Value Visit(FunctionCallNode node)
    {
        var result = Value.Default();

        var callableType = GetCallable(node.Token, node.FunctionName);
        var requireDrop = false;

        try
        {
            switch (callableType)
            {
                case CallableType.Builtin:
                    result = CallBuiltin(node);
                    break;

                case CallableType.Method:
                    result = ExecuteInstanceMethod(node, ref requireDrop);
                    break;

                case CallableType.Function:
                    result = CallFunction(node, ref requireDrop);
                    break;

                case CallableType.Lambda:
                    result = CallLambda(node.Token, node.FunctionName, node.Arguments, ref requireDrop);
                    break;
            }

            if (requireDrop)
            {
                DropFrame();
            }
        }
        catch (KiwiError)
        {
            if (requireDrop)
            {
                DropFrame();
            }

            throw;
        }

        return result;
    }

    private Value Visit(MethodCallNode node) => throw new NotImplementedException();

    private Value Visit(ReturnNode node)
    {
        var returnValue = Value.Default();

        if (node.Condition == null || BooleanOp.IsTruthy(Interpret(node.Condition)))
        {
            if (node.ReturnValue != null)
            {
                returnValue = Interpret(node.ReturnValue);
            }

            var frame = CallStack.Peek();
            frame.SetFlag(FrameFlags.Return);
            frame.ReturnValue = returnValue;
            return returnValue;
        }

        return returnValue;
    }

    private Value Visit(IndexingNode node) => throw new NotImplementedException();
    private Value Visit(SliceNode node) => throw new NotImplementedException();
    /*
    private Value Visit(ParseNode node) => throw new NotImplementedException();
    private Value Visit(SpawnNode node) => throw new NotImplementedException();
    */

    private CallableType GetCallable(Token token, string name)
    {
        if (Context.HasFunction(name))
        {
            return CallableType.Function;
        }
        else if (Context.HasLambda(name))
        {
            return CallableType.Lambda;
        }
        /*else if (KiwiBuiltins.is_builtin_method(name))
        {
            return CallableType.Builtin;
        }*/

        if (Context.HasMappedLambda(name))
        {
            return CallableType.Lambda;
        }

        var frame = CallStack.Peek();

        if (frame.InObjectContext())
        {
            var obj = frame.GetObjectContext();

            if (obj == null)
            {
                throw new NullObjectError(token);
            }

            var struc = Context.Structs[obj.StructName];
            var strucMethods = struc.Methods;

            if (strucMethods.ContainsKey(name))
            {
                return CallableType.Method;
            }

            // check the base
            if (!string.IsNullOrEmpty(struc.BaseStruct))
            {
                var baseStruct = Context.Structs[struc.BaseStruct];
                var baseStructMethods = baseStruct.Methods;

                if (baseStructMethods.ContainsKey(name))
                {
                    return CallableType.Method;
                }
            }

            throw new UnimplementedMethodError(token, struc.Name, name);
        }

        throw new FunctionUndefinedError(token, name);
    }

    private Value CallBuiltin(FunctionCallNode node)
    {
        return Value.Default();
        /*
        var args = GetMethodCallArguments(node.Arguments);
        var op = node.Op;

        if (SerializerBuiltins.is_builtin(op))
        {
            return InterpretSerializerBuiltin(node.Token, op, args);
        }
        else if (ReflectorBuiltins.is_builtin(op))
        {
            return InterpretReflectorBuiltin(node.Token, op, args);
        }
        else if (WebServerBuiltins.is_builtin(op))
        {
            return InterpretWebServerBuiltin(node.Token, op, args);
        }
        else if (SignalBuiltins.is_builtin(op))
        {
            return InterpretSignalBuiltin(node.Token, op, args);
        }
        else if (FFIBuiltins.is_builtin(op))
        {
            return BuiltinDispatch.Execute(ffimgr, node.Token, op, args);
        }
        else if (SocketBuiltins.is_builtin(op))
        {
            return BuiltinDispatch.Execute(sockmgr, node.Token, op, args);
        }
        else if (TaskBuiltins.is_builtin(op))
        {
            return BuiltinDispatch.Execute(taskmgr, node.Token, op, args);
        }

        return BuiltinDispatch.Execute(node.Token, op, args, CliArgs);
        */
    }

    private Value CallFunction(FunctionCallNode node, ref bool requireDrop)
    {
        var functionName = node.FunctionName;
        var func = Context.Functions[functionName];
        var typeHints = func.TypeHints;
        var returnTypeHint = func.ReturnTypeHint;
        var defaultParameters = func.DefaultParameters;
        var functionFrame = CreateFrame(functionName);
        var result = Value.Default();

        PrepareFunctionCall(func, node, defaultParameters, typeHints, functionFrame);

        requireDrop = PushFrame(functionFrame);

        var decl = func.Decl.Body;
        foreach (var stmt in decl)
        {
            result = Interpret(stmt);
            if (functionFrame.IsFlagSet(FrameFlags.Return))
            {
                result = functionFrame.ReturnValue ?? result;
                break;
            }
        }

        if (!Serializer.AssertTypematch(result, returnTypeHint))
        {
            throw new TypeError(node.Token, $"Expected type `{Serializer.GetTypenameString(returnTypeHint)}` for return type of `{functionName}` but received `{Serializer.GetTypenameString(result)}`.");
        }

        return result;
    }

    private Value CallFunction(KFunction function, List<ASTNode> args, Token token, string functionName)
    {
        var defaultParameters = function.DefaultParameters;
        var functionFrame = CreateFrame(functionName);
        var typeHints = function.TypeHints;
        var returnTypeHint = function.ReturnTypeHint;

        var result = Value.Default();
        bool requireDrop = false;

        try
        {
            for (int i = 0; i < function.Parameters.Count; ++i)
            {
                var param = function.Parameters[i];
                var argValue = Value.Default();
                if (i < args.Count)
                {
                    var arg = args[i];
                    argValue = Interpret(arg);
                }
                else if (defaultParameters.Contains(param.Key))
                {
                    argValue = param.Value;
                }
                else
                {
                    throw new ParameterCountMismatchError(token, functionName);
                }

                PrepareFunctionVariables(typeHints, param, ref argValue, token, i, functionName, functionFrame);
            }

            requireDrop = PushFrame(functionFrame);

            result = ExecuteFunctionBody(function);
            DropFrame();
        }
        catch (KiwiError)
        {
            if (requireDrop)
            {
                DropFrame();
            }
            throw;
        }

        if (!Serializer.AssertTypematch(result, returnTypeHint))
        {
            throw new TypeError(token, $"Expected type `{Serializer.GetTypenameString(returnTypeHint)}` for return type of `{functionName}` but received `{Serializer.GetTypenameString(result)}`.");
        }

        return result;
    }

    private Value CallLambda(Token token, string lambdaName, List<ASTNode?> args, ref bool requireDrop)
    {
        var lambdaFrame = CreateFrame(lambdaName);
        var targetLambda = lambdaName;
        var result = Value.Default();

        if (!Context.HasLambda(targetLambda))
        {
            if (!Context.HasMappedLambda(targetLambda))
            {
                throw new CallableError(token, $"Could not find target lambda `{targetLambda}`");
            }

            targetLambda = Context.LambdaTable[targetLambda];
        }

        var func = Context.Lambdas[targetLambda];
        var typeHints = func.TypeHints;
        var returnTypeHint = func.ReturnTypeHint;
        var defaultParameters = func.DefaultParameters;

        PrepareLambdaCall(func, args, defaultParameters, token, targetLambda, typeHints, lambdaName, lambdaFrame);

        lambdaFrame.SetFlag(FrameFlags.InLambda);
        requireDrop = PushFrame(lambdaFrame);

        var decl = func.Decl.Body;
        foreach (var stmt in decl)
        {
            result = Interpret(stmt);
            if (lambdaFrame.IsFlagSet(FrameFlags.Return))
            {
                result = lambdaFrame.ReturnValue ?? result;
                break;
            }
        }

        if (!Serializer.AssertTypematch(result, returnTypeHint))
        {
            throw new TypeError(
                token, $"Expected type `{Serializer.GetTypenameString(returnTypeHint)}` for return type of `{lambdaName}` but received `{Serializer.GetTypenameString(result)}`.");
        }

        return result;
    }

    private void PrepareFunctionCall(KFunction func, FunctionCallNode node, HashSet<string> defaultParameters, Dictionary<string, TokenName> typeHints, StackFrame functionFrame)
    {
        var parms = func.Parameters;
        var nodeArguments = node.Arguments;

        for (int i = 0; i < parms.Count; ++i)
        {
            var param = parms[i];
            var argValue = Value.Default();

            if (i < nodeArguments.Count)
            {
                var arg = nodeArguments[i];
                argValue = Interpret(arg);
            }
            else if (defaultParameters.Contains(param.Key))
            {
                argValue = param.Value;
            }
            else
            {
                throw new ParameterCountMismatchError(node.Token, node.FunctionName);
            }

            if (typeHints.ContainsKey(param.Key))
            {
                var expectedType = typeHints[param.Key];
                if (!Serializer.AssertTypematch(argValue, expectedType))
                {
                    throw new TypeError(node.Token, $"Expected type `{Serializer.GetTypenameString(expectedType)}` for parameter {(1 + i)} of `{node.FunctionName}` but received `{Serializer.GetTypenameString(argValue)}`.");
                }
            }

            if (argValue.IsLambda())
            {
                Context.AddMappedLambda(param.Key, argValue.GetLambda().Identifier);
            }
            else
            {
                functionFrame.Variables[param.Key] = argValue;
            }
        }
    }

    private void PrepareFunctionVariables(Dictionary<string, TokenName> typeHints, KeyValuePair<string, Value> param, ref Value argValue, Token token, int i, string functionName, StackFrame functionFrame)
    {
        if (typeHints.TryGetValue(param.Key, out TokenName expectedType) && !Serializer.AssertTypematch(argValue, expectedType))
        {
            throw new TypeError(token, $"Expected type `{Serializer.GetTypenameString(expectedType)}` for parameter {(1 + i)} of `{functionName}` but received `{Serializer.GetTypenameString(argValue)}`.");
        }

        if (argValue.IsLambda())
        {
            var lambdaId = argValue.GetLambda().Identifier;
            Context.AddMappedLambda(param.Key, lambdaId);
        }
        else
        {
            functionFrame.Variables[param.Key] = argValue;
        }
    }

    private void PrepareLambdaCall(KLambda func, List<ASTNode?> args, HashSet<string> defaultParameters, Token token, string targetLambda, Dictionary<string, TokenName> typeHints, string lambdaName, StackFrame lambdaFrame)
    {
        var parms = func.Parameters;
        for (int i = 0; i < parms.Count; ++i)
        {
            var param = parms[i];
            var argValue = Value.Default();
            if (i < args.Count)
            {
                argValue = Interpret(args[i]);
            }
            else if (defaultParameters.Contains(param.Key))
            {
                argValue = param.Value;
            }
            else
            {
                throw new ParameterCountMismatchError(token, targetLambda);
            }

            PrepareLambdaVariables(typeHints, param, ref argValue, token, i, lambdaName, lambdaFrame);
        }
    }

    private void PrepareLambdaCall(KLambda func, List<Value> args, HashSet<string> defaultParameters, Token token, string targetLambda, Dictionary<string, TokenName> typeHints, string lambdaName, StackFrame lambdaFrame)
    {
        var parms = func.Parameters;
        for (int i = 0; i < parms.Count; ++i)
        {
            var param = parms[i];
            var argValue = Value.Default();
            if (i < args.Count)
            {
                argValue = args[i];
            }
            else if (defaultParameters.Contains(param.Key))
            {
                argValue = param.Value;
            }
            else
            {
                throw new ParameterCountMismatchError(token, targetLambda);
            }

            if (typeHints.TryGetValue(param.Key, out TokenName expectedType) && !Serializer.AssertTypematch(argValue, expectedType))
            {
                throw new TypeError(token, $"Expected type `{Serializer.GetTypenameString(expectedType)}` for parameter {(1 + i)} of `{lambdaName}` but received `{Serializer.GetTypenameString(argValue)}`.");
            }

            if (argValue.IsLambda())
            {
                Context.AddMappedLambda(param.Key, argValue.GetLambda().Identifier);
            }
            else
            {
                lambdaFrame.Variables[param.Key] = argValue;
            }
        }
    }

    private void PrepareLambdaVariables(Dictionary<string, TokenName> typeHints, KeyValuePair<string, Value> param, ref Value argValue,
        Token token, int i, string lambdaName, StackFrame lambdaFrame)
    {
        if (typeHints.TryGetValue(param.Key, out TokenName expectedType))
        {

            if (!Serializer.AssertTypematch(argValue, expectedType))
            {
                throw new TypeError(token, $"Expected type `{Serializer.GetTypenameString(expectedType)}` for parameter {(1 + i)} of `{lambdaName}` but received `{Serializer.GetTypenameString(argValue)}`.");
            }
        }

        if (argValue.IsLambda())
        {
            var lambdaId = argValue.GetLambda().Identifier;
            Context.AddMappedLambda(param.Key, lambdaId);
        }
        else
        {
            lambdaFrame.Variables[param.Key] = argValue;
        }
    }

    private Value ExecuteFunctionBody(KFunction function)
    {
        var result = Value.Default();
        var decl = function.Decl;

        foreach (var stmt in decl.Body)
        {
            result = Interpret(stmt);

            if (CallStack.Peek().IsFlagSet(FrameFlags.Return))
            {
                result = CallStack.Peek().ReturnValue ?? result;
                break;
            }
        }

        return result;
    }

    private Value ExecuteInstanceMethod(FunctionCallNode node, ref bool requireDrop)
    {
        var frame = CallStack.Peek();
        if (!frame.InObjectContext())
        {
            throw new InvalidContextError(node.Token);
        }

        var obj = frame.GetObjectContext();
        if (obj == null)
        {
            throw new NullObjectError(node.Token);
        }

        var struc = Context.Structs[obj.StructName];
        var strucMethods = struc.Methods;
        var functionName = node.FunctionName;

        if (!strucMethods.ContainsKey(functionName))
        {
            // check the base
            if (string.IsNullOrEmpty(struc.BaseStruct))
            {
                throw new UnimplementedMethodError(node.Token, struc.Name, functionName);
            }

            var baseStruct = Context.Structs[struc.BaseStruct];
            var baseStructMethods = baseStruct.Methods;

            if (!baseStructMethods.ContainsKey(functionName))
            {
                throw new UnimplementedMethodError(node.Token, struc.Name, functionName);
            }

            return ExecuteInstanceMethodFunction(baseStructMethods, node, ref requireDrop);
        }

        return ExecuteInstanceMethodFunction(strucMethods, node, ref requireDrop);
    }

    private Value ExecuteInstanceMethodFunction(Dictionary<string, KFunction> strucMethods, FunctionCallNode node, ref bool requireDrop)
    {
        var functionName = node.FunctionName;
        var func = strucMethods[functionName];
        var defaultParameters = func.DefaultParameters;
        var functionFrame = CreateFrame(functionName);
        var result = Value.Default();

        var typeHints = func.TypeHints;
        var returnTypeHint = func.ReturnTypeHint;

        PrepareFunctionCall(func, node, defaultParameters, typeHints, functionFrame);

        requireDrop = PushFrame(functionFrame);

        var decl = func.Decl.Body;
        foreach (var stmt in decl)
        {
            result = Interpret(stmt);
            if (functionFrame.IsFlagSet(FrameFlags.Return))
            {
                result = functionFrame.ReturnValue ?? result;
                break;
            }
        }

        if (!Serializer.AssertTypematch(result, returnTypeHint))
        {
            throw new TypeError(node.Token, $"Expected type `{Serializer.GetTypenameString(returnTypeHint)}` for return type of `{functionName}` but received `{Serializer.GetTypenameString(result)}`.");
        }

        return result;
    }

    private void ImportPackage(Token token, Value packageName)
    {
        if (!packageName.IsString())
        {
            throw new InvalidOperationError(token, "Expected the name of a package to import.");
        }

        var packageNameValue = packageName.GetString();

        if (!Context.HasPackage(packageNameValue))
        {
            // Check if external package.
            if (FileUtil.IsScript(token, packageNameValue))
            {
                ImportExternal(token, packageNameValue);
                return;
            }

            throw new PackageUndefinedError(token, packageNameValue);
        }

        PackageStack.Push(packageNameValue);
        var package = Context.Packages[packageNameValue];
        var decl = package.Decl;

        foreach (var stmt in decl.Body)
        {
            Interpret(stmt);
        }

        PackageStack.Pop();
    }

    private void ImportExternal(Token token, string packageName)
    {
        var packagePath = FileUtil.TryGetExtensionless(token, packageName);
        var content = FileUtil.ReadFile(token, packagePath);
        if (string.IsNullOrEmpty(content))
        {
            return;
        }

        var path = FileUtil.GetAbsolutePath(token, packagePath);
        Lexer lexer = new(1, path);

        Parser p = new(true);
        var tokenStream = lexer.GetTokenStream();
        var ast = p.ParseTokenStream(tokenStream, true);

        Interpret(ast);

        return;
    }

    private Value ListLoop(ForLoopNode node, List<Value> list)
    {
        var frame = CallStack.Peek();
        var variables = frame.Variables;
        frame.SetFlag(FrameFlags.InLoop);

        var result = Value.Default();
        var valueIteratorName = string.Empty;
        var indexIteratorName = string.Empty;
        bool hasIndexIterator = false;

        valueIteratorName = Id(node.ValueIterator);

        if (node.IndexIterator != null)
        {
            indexIteratorName = Id(node.IndexIterator);
            hasIndexIterator = true;
        }

        bool fallOut = false;
        var iteratorValue = Value.Default();
        var iteratorIndex = Value.Default();

        for (int i = 0; i < list.Count; ++i)
        {
            if (fallOut)
            {
                break;
            }

            iteratorValue.SetValue(list[i]);
            variables[valueIteratorName] = iteratorValue;

            if (hasIndexIterator)
            {
                iteratorIndex.SetValue((long)i);
                variables[indexIteratorName] = iteratorIndex;
            }

            foreach (var stmt in node.Body)
            {
                if (stmt == null)
                {
                    continue;
                }

                ASTNodeType statement = stmt.Type;
                if (statement != ASTNodeType.Next && statement != ASTNodeType.Break)
                {
                    result = Interpret(stmt);

                    if (frame.IsFlagSet(FrameFlags.Break))
                    {
                        frame.ClearFlag(FrameFlags.Break);
                        fallOut = true;
                        break;
                    }

                    if (frame.IsFlagSet(FrameFlags.Next))
                    {
                        frame.ClearFlag(FrameFlags.Next);
                        break;
                    }
                }

                if (frame.IsFlagSet(FrameFlags.Return))
                {
                    break;
                }

                if (statement == ASTNodeType.Next)
                {
                    var condition = ((NextNode)stmt).Condition;
                    if (condition == null || BooleanOp.IsTruthy(Interpret(condition)))
                    {
                        break;
                    }
                }
                else if (statement == ASTNodeType.Break)
                {
                    var condition = ((BreakNode)stmt).Condition;
                    if (condition == null || BooleanOp.IsTruthy(Interpret(condition)))
                    {
                        fallOut = true;
                        break;
                    }
                }
            }
        }

        variables.Remove(valueIteratorName);
        if (hasIndexIterator)
        {
            variables.Remove(indexIteratorName);
        }

        frame.ClearFlag(FrameFlags.InLoop);

        return result;
    }

    private Value HashmapLoop(ForLoopNode node, Dictionary<Value, Value> hash)
    {
        var frame = CallStack.Peek();
        frame.SetFlag(FrameFlags.InLoop);
        var indexIteratorName = string.Empty;
        bool hasIndexIterator = false;

        string? valueIteratorName = Id(node.ValueIterator);

        if (node.IndexIterator != null)
        {
            indexIteratorName = Id(node.IndexIterator);
            hasIndexIterator = true;
        }

        bool fallOut = false;
        var result = Value.Default();

        foreach (var key in hash.Keys)
        {
            if (fallOut)
            {
                break;
            }

            frame.Variables[valueIteratorName] = key;

            if (hasIndexIterator)
            {
                frame.Variables[indexIteratorName] = hash[key];
            }

            foreach (var stmt in node.Body)
            {
                if (stmt == null)
                {
                    continue;
                }

                if (stmt.Type != ASTNodeType.Next && stmt.Type != ASTNodeType.Break)
                {
                    result = Interpret(stmt);

                    if (frame.IsFlagSet(FrameFlags.Break))
                    {
                        frame.ClearFlag(FrameFlags.Break);
                        fallOut = true;
                        break;
                    }

                    if (frame.IsFlagSet(FrameFlags.Next))
                    {
                        frame.ClearFlag(FrameFlags.Next);
                        break;
                    }
                }

                if (frame.IsFlagSet(FrameFlags.Return))
                {
                    break;
                }

                if (stmt.Type == ASTNodeType.Next)
                {
                    var condition = ((NextNode)stmt).Condition;
                    if (condition == null || BooleanOp.IsTruthy(Interpret(condition)))
                    {
                        break;
                    }
                }
                else if (stmt.Type == ASTNodeType.Break)
                {
                    var condition = ((BreakNode)stmt).Condition;
                    if (condition == null || BooleanOp.IsTruthy(Interpret(condition)))
                    {
                        fallOut = true;
                        break;
                    }
                }
            }
        }

        frame.Variables.Remove(valueIteratorName);
        if (hasIndexIterator)
        {
            frame.Variables.Remove(indexIteratorName);
        }

        frame.ClearFlag(FrameFlags.InLoop);

        return result;
    }

    private bool PushFrame(StackFrame frame)
    {
        CallStack.Push(frame);
        FuncStack.Push(frame.Name);
        return true;
    }

    private bool InTry()
    {
        if (CallStack.Count == 0)
        {
            return false;
        }

        return CallStack.Peek().IsFlagSet(FrameFlags.InTry);
    }

    private StackFrame CreateFrame(string name, bool isMethodInvocation = false)
    {
        StackFrame frame = CallStack.Peek();
        StackFrame subFrame = new();

        var subFrameVariables = subFrame.Variables;
        subFrame.Name = name;

        if (!isMethodInvocation)
        {
            var frameVariables = frame.Variables;
            foreach (var kvp in frameVariables)
            {
                subFrameVariables[kvp.Key] = kvp.Value;
            }
        }

        if (frame.InObjectContext())
        {
            var objectContext = frame.GetObjectContext();
            subFrame.SetObjectContext(objectContext);
        }

        if (frame.IsFlagSet(FrameFlags.InTry))
        {
            subFrame.SetFlag(FrameFlags.InTry);
        }

        if (frame.IsFlagSet(FrameFlags.SubFrame))
        {
            subFrame.SetFlag(FrameFlags.SubFrame);
        }

        return subFrame;
    }

    private void DropFrame()
    {
        if (CallStack.Count == 0)
        {
            return;
        }

        if (FuncStack.Count > 0)
        {
            FuncStack.Pop();
        }

        var frame = CallStack.Peek();
        var returnValue = frame.ReturnValue;
        var topVariables = frame.Variables;

        CallStack.Pop();

        if (CallStack.Count > 0)
        {
            var callerFrame = CallStack.Peek();

            callerFrame.ReturnValue = returnValue;

            if (callerFrame.IsFlagSet(FrameFlags.SubFrame))
            {
                callerFrame.SetFlag(FrameFlags.Return);
            }

            UpdateVariablesInCallerFrame(topVariables, callerFrame);
        }
    }

    private static string Id(ASTNode node) => ((IdentifierNode)node).Name;

    private static bool ShouldUpdateFrameVariables(string varName, StackFrame nextFrame) => nextFrame.HasVariable(varName);

    private static void UpdateVariablesInCallerFrame(Dictionary<string, Value> variables, StackFrame callerFrame)
    {
        var frameVariables = callerFrame.Variables;
        foreach (var v in variables)
        {
            if (!ShouldUpdateFrameVariables(v.Key, callerFrame))
            {
                continue;
            }

            frameVariables[v.Key] = v.Value;
        }
    }
}