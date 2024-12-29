using citrus.Builtin;
using citrus.Parsing.AST;
using citrus.Typing;
namespace citrus.Runtime;

public class Interpreter
{
    const int SafemodeMaxIterations = 1000000;

    public KContext Context { get; private set; } = new();
    private Stack<StackFrame> CallStack { get; set; } = [];
    private Stack<string> PackageStack { get; set; } = [];
    private Stack<string> StructStack { get; set; } = [];
    private Stack<string> FuncStack { get; set; } = [];
    private Dictionary<string, string> CliArgs { get; set; } = [];
    private static Value DefaultValue = Value.CreateNull();

    public void SetContext(KContext context) => Context = context;

    public Value Interpret(ASTNode? node)
    {
        if (node == null)
        {
            return DefaultValue;
        }

        Value result = DefaultValue;
        bool sigCheck = false;

        switch (node.Type)
        {
            case ASTNodeType.Program:
                result = Visit((ProgramNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.Self:
                result = Visit((SelfNode)node);
                break;

            case ASTNodeType.Package:
                result = Visit((PackageNode)node);
                break;

            case ASTNodeType.Struct:
                result = Visit((StructNode)node);
                break;

            case ASTNodeType.Import:
                result = Visit((ImportNode)node);
                break;

            case ASTNodeType.Export:
                result = Visit((ExportNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.Exit:
                Visit((ExitNode)node);
                break;

            case ASTNodeType.Throw:
                result = Visit((ThrowNode)node);
                break;

            case ASTNodeType.Assignment:
                result = Visit((AssignmentNode)node);
                break;

            case ASTNodeType.ConstAssignment:
                result = Visit((ConstAssignmentNode)node);
                break;

            case ASTNodeType.IndexAssignment:
                result = Visit((IndexAssignmentNode)node);
                break;

            case ASTNodeType.MemberAssignment:
                result = Visit((MemberAssignmentNode)node);
                break;

            case ASTNodeType.PackAssignment:
                result = Visit((PackAssignmentNode)node);
                break;

            case ASTNodeType.MemberAccess:
                result = Visit((MemberAccessNode)node);
                break;

            case ASTNodeType.Literal:
                result = Visit((LiteralNode)node);
                break;

            case ASTNodeType.ListLiteral:
                result = Visit((ListLiteralNode)node);
                break;

            case ASTNodeType.RangeLiteral:
                result = Visit((RangeLiteralNode)node);
                break;

            case ASTNodeType.HashLiteral:
                result = Visit((HashLiteralNode)node);
                break;

            case ASTNodeType.Identifier:
                result = Visit((IdentifierNode)node);
                break;

            case ASTNodeType.Print:
                Visit((PrintNode)node);
                break;

            case ASTNodeType.PrintXy:
                result = Visit((PrintXyNode)node);
                break;

            case ASTNodeType.TernaryOperation:
                result = Visit((TernaryOperationNode)node);
                break;

            case ASTNodeType.BinaryOperation:
                result = Visit((BinaryOperationNode)node);
                break;

            case ASTNodeType.UnaryOperation:
                result = Visit((UnaryOperationNode)node);
                break;

            case ASTNodeType.If:
                result = Visit((IfNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.Case:
                result = Visit((CaseNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.ForLoop:
                result = Visit((ForLoopNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.WhileLoop:
                result = Visit((WhileLoopNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.RepeatLoop:
                result = Visit((RepeatLoopNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.Break:
                result = Visit((BreakNode)node);
                break;

            case ASTNodeType.Next:
                result = Visit((NextNode)node);
                break;

            case ASTNodeType.Try:
                result = Visit((TryNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.Lambda:
                result = Visit((LambdaNode)node);
                break;

            case ASTNodeType.LambdaCall:
                result = Visit((LambdaCallNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.Function:
                result = Visit((FunctionNode)node);
                break;

            case ASTNodeType.Variable:
                result = Visit((VariableNode)node);
                break;

            case ASTNodeType.FunctionCall:
                result = Visit((FunctionCallNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.MethodCall:
                result = Visit((MethodCallNode)node);
                sigCheck = true;
                break;

            case ASTNodeType.Return:
                result = Visit((ReturnNode)node);
                break;

            case ASTNodeType.Index:
                result = Visit((IndexingNode)node);
                break;

            case ASTNodeType.Slice:
                result = Visit((SliceNode)node);
                break;

            case ASTNodeType.Parse:
                //result = Visit((ParseNode)node);
                break;

            case ASTNodeType.NoOp:
                break;

            case ASTNodeType.Spawn:
                //result = Visit((SpawnNode)node);
                break;

            default:
                node.Print();
                break;
        }

        if (sigCheck)
        {
            // handlePendingSignals(node->token);
        }

        return result;
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
    private void Visit(ExitNode node)
    {
        if (node.Condition == null || BooleanFunc.IsTruthy(Interpret(node.Condition)))
        {
            var exitValue = Interpret(node.ExitValue);
            long exitCode;
            
            if (exitValue.IsInteger())
            {
                exitCode = exitValue.GetInteger();
            }
            else
            {
                exitCode = 1L;
            }

            Environment.Exit(Convert.ToInt32(exitCode));
        }
    }

    private Value Visit(ThrowNode node) => throw new NotImplementedException();
    private Value Visit(AssignmentNode node) => throw new NotImplementedException();
    private Value Visit(ConstAssignmentNode node) => throw new NotImplementedException();
    private Value Visit(IndexAssignmentNode node) => throw new NotImplementedException();
    private Value Visit(MemberAssignmentNode node) => throw new NotImplementedException();
    private Value Visit(PackAssignmentNode node) => throw new NotImplementedException();
    private Value Visit(MemberAccessNode node) => throw new NotImplementedException();

    private static Value Visit(LiteralNode node) => node.Value;

    private Value Visit(RangeLiteralNode node) => throw new NotImplementedException();

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

    private void Visit(PrintNode node)
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
    }

    private Value Visit(PrintXyNode node) => throw new NotImplementedException();
    private Value Visit(TernaryOperationNode node) => throw new NotImplementedException();
    private Value Visit(BinaryOperationNode node) => throw new NotImplementedException();
    private Value Visit(UnaryOperationNode node) => throw new NotImplementedException();
    private Value Visit(IfNode node) => throw new NotImplementedException();
    private Value Visit(CaseNode node) => throw new NotImplementedException();
    private Value Visit(ForLoopNode node) => throw new NotImplementedException();
    private Value Visit(WhileLoopNode node) => throw new NotImplementedException();
    private Value Visit(RepeatLoopNode node) => throw new NotImplementedException();
    private Value Visit(BreakNode node) => throw new NotImplementedException();
    private Value Visit(NextNode node) => throw new NotImplementedException();
    private Value Visit(TryNode node) => throw new NotImplementedException();
    private Value Visit(LambdaNode node) => throw new NotImplementedException();
    private Value Visit(FunctionNode node) => throw new NotImplementedException();
    private Value Visit(VariableNode node) => throw new NotImplementedException();
    private Value Visit(LambdaCallNode node) => throw new NotImplementedException();
    private Value Visit(FunctionCallNode node) => throw new NotImplementedException();
    private Value Visit(MethodCallNode node) => throw new NotImplementedException();
    
    private Value Visit(ReturnNode node)
    {
        var returnValue = Value.Default();
        
        if (node.Condition == null || BooleanFunc.IsTruthy(Interpret(node.Condition)))
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

    private bool ShouldUpdateFrameVariables(string varName, StackFrame nextFrame) => nextFrame.HasVariable(varName);

    private void UpdateVariablesInCallerFrame(Dictionary<string, Value> variables, StackFrame callerFrame)
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