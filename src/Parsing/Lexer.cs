using System.Threading.Channels;
using hayward.Parsing.Keyword;
using hayward.Tracing;
using hayward.Typing;

namespace hayward.Parsing;

public class Lexer : IDisposable
{
    private readonly Stream stream;
    private readonly int File;
    private int LineNumber = 1;
    private int Position = 1;

    public Lexer(string path)
    {
        stream = System.IO.File.OpenRead(path);
        File = FileRegistry.Instance.RegisterFile(path);
    }

    public Lexer(int fileId, string code)
    {
        File = fileId;
        stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(code));
    }

    public TokenStream GetTokenStream()
    {
        return new TokenStream(GetTokens());
    }

    public void Dispose()
    {
        stream?.Close();
    }

    private List<Token> GetTokens()
    {
        List<Token> tokens = [];
        stream.Seek(0, SeekOrigin.Begin);

        while (stream.CanRead)
        {
            var token = GetToken();

            if (token.Type == TokenType.String)
            {
                TokenizeStringInterpolation(ref tokens, token);
            }
            else
            {
                tokens.Add(token);
            }

            if (token.Type == TokenType.Eof)
            {
                break;
            }
        }

        return tokens;
    }

    private Token GetToken()
    {
        SkipWhitespace();

        var span = CreateSpan();
        var ch = GetChar();

        if (ch == null)
        {
            return CreateToken(TokenType.Eof, span, string.Empty);
        }

        var c = ch.Value;

        if (char.IsAsciiLetter(c) || c == '_')
        {
            return TokenizeKeywordOrIdentifier(span, c);
        }
        else if (char.IsDigit(c))
        {
            return TokenizeNumericLiteral(span, c);
        }
        else if (c == '"')
        {
            return TokenizeString(span);
        }

        return TokenizeSymbol(span, c);
    }

    private char? PeekChar()
    {
        if (!stream.CanRead)
        {
            return null;
        }

        long origin = stream.Position;
        int byteValue = stream.ReadByte();

        if (byteValue == -1)
        {
            return null;
        }

        stream.Position = origin;

        return (char)byteValue;
    }

    private char? GetChar()
    {
        if (!stream.CanRead)
        {
            return null;
        }

        int byteValue = stream.ReadByte();
        if (byteValue == -1)
        {
            return null;
        }

        char c = (char)byteValue;
        ++Position;

        // for Windows
        if (c == '\r')
        {
            // '\r\n'
            char? next = PeekChar();
            if (next == '\n')
            {
                stream.ReadByte();
                c = '\n';
                ++Position;
            }
            else
            {
                c = '\n';
            }
        }

        return c;
    }

    private void SkipWhitespace()
    {
        int byteValue;
        while ((byteValue = stream.ReadByte()) != -1)
        {
            var c = (char)byteValue;

            if (c == '\n' || c == '\r')
            {
                ++LineNumber;
                Position = 1;
            }
            else
            {
                ++Position;
            }

            if (!char.IsWhiteSpace(c) && c != '\n')
            {
                --stream.Position;
                --Position;
                break;
            }
        }
    }

    private static void TokenizeStringInterpolation(ref List<Token> tokens, Token token)
    {
        var span = token.Span;
        var text = token.Text;
        System.Text.StringBuilder sv = new();

        if (!text.Contains("${"))
        {
            tokens.Add(token);
            return;
        }

        List<Token> interpTokens = [];
        var interpolate = false;

        for (int i = 0, braces = 0; i < text.Length; ++i)
        {
            var c = text[i];
            switch (c)
            {
                case '$':
                    {
                        if (i + 1 < text.Length && text[i + 1] == '{')
                        {
                            ++i; // skip "${"
                            ++braces;

                            var s = sv.ToString();
                            sv.Clear();

                            var st = CreateStringLiteralToken(span, s, Value.CreateString(s));
                            interpTokens.Add(st);
                            interpolate = true;
                        }
                    }
                    break;

                case '{':
                    if (braces > 0)
                    {
                        ++braces;
                    }

                    sv.Append(c);
                    break;

                case '}':
                    {
                        if (braces > 0)
                        {
                            --braces;
                        }

                        if (braces > 0 || !interpolate)
                        {
                            sv.Append(c);
                            break;
                        }

                        var code = sv.ToString();
                        Lexer lex = new(token.Span.File, code);
                        sv.Clear();

                        var tmpTokens = lex.GetTokens();
                        if (tmpTokens == null || tmpTokens.Count == 0)
                        {
                            // empty interpolation
                            break;
                        }

                        // string: "your name repeated ${repeater} time(s) is ${name * repeater}"
                        // interp: "your name repeated " + (repeater) + " time(s) is " + (name * repeater)

                        // if we aren't at the beginning of the string, concatenate.
                        if (interpTokens.Count > 0)
                        {
                            interpTokens.Add(CreateToken(TokenType.Operator, span, "+", TokenName.Ops_Add));
                        }

                        // wrap inner tokens in parentheses
                        interpTokens.Add(CreateToken(TokenType.LParen, span, "("));
                        foreach (var tmpToken in tmpTokens)
                        {
                            if (tmpToken.Type == TokenType.Eof)
                            {
                                continue;
                            }

                            tmpToken.SetSpan(span);
                            interpTokens.Add(tmpToken);
                        }
                        interpTokens.Add(CreateToken(TokenType.RParen, span, ")"));

                        // if we aren't at the end of the string, concatenate
                        if (i + 1 < text.Length)
                        {
                            interpTokens.Add(CreateToken(TokenType.Operator, span, "+", TokenName.Ops_Add));
                        }

                        interpolate = false;
                    }
                    break;

                default:
                    sv.Append(c);
                    break;
            }
        }

        if (sv.Length > 0)
        {
            if (interpTokens.Count > 0 && interpTokens.Last().Name != TokenName.Ops_Add)
            {
                interpTokens.Add(CreateToken(TokenType.Operator, span, "+", TokenName.Ops_Add));
            }
            var s = sv.ToString();
            interpTokens.Add(CreateStringLiteralToken(span, s, Value.CreateString(s)));
        }

        tokens.AddRange(interpTokens);
    }

    private Token TokenizeSymbol(TokenSpan span, char c)
    {
        return c switch
        {
            '\n' => CreateToken(TokenType.Newline, span, Environment.NewLine),
            ',' => CreateToken(TokenType.Comma, span, ","),
            '@' => CreateToken(TokenType.Keyword, span, "@", TokenName.KW_This),
            ':' when PeekChar() == ':' => CreateToken(TokenType.Qualifier, span, $":{GetChar()}"),
            '.' when PeekChar() == '.' => CreateToken(TokenType.Range, span, $".{GetChar()}"),
            ':' => CreateToken(TokenType.Colon, span, ":"),
            '.' => CreateToken(TokenType.Dot, span, "."),
            '\\' => TokenizeEscaped(span),
            '(' or ')' => TokenizeParen(span, c),
            '[' or ']' => TokenizeBracket(span, c),
            '{' or '}' => TokenizeBrace(span, c),
            '#' => TokenizeComment(span),
            '?' => CreateToken(TokenType.Question, span, "?"),
            '/' when PeekChar() == '#' => TokenizeBlockComment(span),
            ';' => CreateToken(TokenType.Keyword, span, ";", TokenName.KW_End),
            _ => TokenizeOperator(span, c),
        };
    }

    private Token TokenizeEscaped(TokenSpan span)
    {
        var c = GetChar();
        var text = c switch
        {
            '\\' => "\\",
            'b' => "\b",
            'r' => "\r",
            'n' => "\n",
            't' => "\t",
            'f' => "\f",
            '"' => "\"",
            _ => $"\\{c}",
        };
        return CreateToken(TokenType.Escape, span, text);
    }

    private Token TokenizeOperator(TokenSpan span, char c)
    {
        var text = string.Empty + c;

        if (stream.CanRead)
        {
            char? nc = PeekChar();
            bool isArithmeticOpChar = c is '+' or '*' or '-' or '/' or '%';
            bool isBooleanOpChar = c is '|' or '&' or '=' or '!' or '<' or '>';
            bool isArithmeticOp = (nc == '=' && (isArithmeticOpChar || isBooleanOpChar)) ||
                (c == '*' && nc == '*');
            bool isBooleanOp = (nc == '|' || nc == '&') && isBooleanOpChar;
            bool isBitwiseOp =
                ((c == '|' || c == '&' || c == '^' || c == '~') && nc == '=') ||
                (c == '<' && nc == '<') ||
                (c == '>' && nc == '>');

            if (isArithmeticOp || isBooleanOp || isBitwiseOp)
            {
                text += nc;
                nc = GetChar();

                if ((nc == '=' && (text == "**" || text == "||" || text == "&&" || text == "<<" || text == ">>")) ||
                    (nc == '>' && text == ">>"))
                {
                    text += nc;
                    nc = PeekChar();

                    if (nc == '=' && text == ">>>")
                    {
                        text += nc;
                        GetChar();
                    }
                }
            }
        }

        var name = GetOperatorName(text);

        return CreateToken(TokenType.Operator, span, text, name);
    }

    private Token TokenizeString(TokenSpan span)
    {
        var text = string.Empty;
        bool escape = false;
        char? c;

        while ((c = PeekChar()) != null)
        {
            if (escape)
            {
                text += c switch
                {
                    'n' => '\n',
                    'r' => '\r',
                    't' => '\t',
                    '\\' => '\\',
                    'b' => '\b',
                    'f' => '\f',
                    '"' => '"',
                    _ => $"\\{c}",
                };

                escape = false;
                GetChar();
            }
            else if (c == '\\')
            {
                escape = true;
                GetChar();
            }
            else if (c == '"')
            {
                GetChar();  // Move past the closing quote
                break;      // End of string
            }
            /*else if (c == '$' && PeekChar() == '{')
            {
                GetChar();  // Skip '$'
                GetChar();  // Skip '{'
                text += TokenizeInterpolation();
                continue;
            }*/
            else
            {
                text += c;
                GetChar();
            }
        }

        if (escape)
        {
            text += '\\';
        }

        return CreateStringLiteralToken(span, text, Value.CreateString(text));
    }

    private Token TokenizeBlockComment(TokenSpan span)
    {
        var text = string.Empty;
        GetChar(); // Skip '#'

        SkipWhitespace();
        char? ch;

        while ((ch = GetChar()) != null)
        {
            if (ch == '#' && PeekChar() == '/')
            {
                GetChar();
                break;
            }

            text += ch;
        }

        return CreateToken(TokenType.Comment, span, text);
    }

    private Token TokenizeComment(TokenSpan span)
    {
        var text = string.Empty;

        SkipWhitespace();
        char? ch;

        while ((ch = GetChar()) != null)
        {
            if (ch == '\n')
            {
                ++LineNumber;
                break;
            }

            text += ch;
        }

        return CreateToken(TokenType.Comment, span, text);
    }

    private static bool MatchChar(char? c, string within)
    {
        if (c == null)
        {
            return false;
        }

        return within.Contains((char)c);
    }

    private Token TokenizeNumericLiteral(TokenSpan span, char c)
    {
        var peek = PeekChar();
        
        if (c == '0' && MatchChar(peek, "xX"))
        {
            return TokenizeHexLiteral(span);
        }
        else if (c == '0' && MatchChar(peek, "bB"))
        {
            return TokenizeBinaryLiteral(span);
        }
        else if (c == '0' && MatchChar(peek, "oO"))
        {
            return TokenizeOctalLiteral(span);
        }
        
        return TokenizeLiteral(span, c);
    }

    private Token TokenizeHexLiteral(TokenSpan span)
    {
        var hexLiteral = string.Empty;
        GetChar();  // Move past 'x'

        char? ch;
        while ((ch = PeekChar()) != null && char.IsAsciiHexDigit((char)ch))
        {
            hexLiteral += GetChar();
        }

        var result = int.Parse(hexLiteral, System.Globalization.NumberStyles.HexNumber, System.Globalization.CultureInfo.InvariantCulture);

        return CreateLiteralToken(span, hexLiteral, Value.CreateInteger(result));
    }

    private Token TokenizeBinaryLiteral(TokenSpan span)
    {
        var binaryLiteral = string.Empty;
        GetChar();  // Move past 'b'

        char? ch;
        while ((ch = PeekChar()) != null && (ch == '0' || ch == '1'))
        {
            binaryLiteral += GetChar();
        }

        if (string.IsNullOrEmpty(binaryLiteral))
        {
            return CreateLiteralToken(span, binaryLiteral, Value.Default);
        }

        var result = int.Parse(binaryLiteral, System.Globalization.NumberStyles.BinaryNumber, System.Globalization.CultureInfo.InvariantCulture);

        return CreateLiteralToken(span, binaryLiteral, Value.CreateInteger(result));
    }

    private Token TokenizeOctalLiteral(TokenSpan span) 
    {
        var octalLiteral = string.Empty;
        GetChar();  // Move past 'o'

        char? ch;
        while ((ch = PeekChar()) != null && ch >= '0' && ch <= '7')
        {
            octalLiteral += GetChar();
        }

        if (string.IsNullOrEmpty(octalLiteral))
        {
            return CreateLiteralToken(span, octalLiteral, Value.Default);
        }

        var result = Convert.ToInt32(octalLiteral, 8);

        return CreateLiteralToken(span, octalLiteral, Value.CreateInteger(result));
    }

    private Token TokenizeLiteral(TokenSpan span, char c)
    {
        var text = string.Empty + c;
        var lastChar = '\0';
        char? ch;

        while ((ch = PeekChar()) != null && (char.IsDigit(ch.Value) || ch == '.' || ch == 'e' || ch == 'E'))
        {
            c = ch.Value;

            if (c == '.' && lastChar == '.')
            {
                text = text[..^1];
                --stream.Position;
                break;
            }

            text += c;
            lastChar = c;
            GetChar();
        }

        var dots = text.Count(c => c == '.');
        var es = text.ToLower().Count(c => c == 'e');

        if (dots > 1 || es > 1)
        {
            return CreateToken(TokenType.Error, span, text);
        }

        if (dots == 0)
        {
            return CreateLiteralToken(span, text, Value.CreateInteger(Convert.ToInt64(text)));
        }

        return CreateLiteralToken(span, text, Value.CreateFloat(Convert.ToDouble(text)));
    }

    private Token TokenizeKeywordOrIdentifier(TokenSpan span, char c)
    {
        var text = string.Empty + c;
        char? ch;

        while ((ch = PeekChar()) != null)
        {
            c = ch.Value;

            if (!char.IsAsciiLetterOrDigit(c) && c != '_')
            {
                break;
            }

            text += c;
            GetChar();
        }

        if (IsKeyword(text, out TokenName kw))
        {
            return CreateToken(TokenType.Keyword, span, text, kw);
        }
        else if (IsTypenameKeyword(text, out kw))
        {
            var typeName = CreateToken(TokenType.Typename, span, text, kw);
            typeName.Value = Value.CreateString(text);
            return typeName;
        }
        else if (IsLogicalKeyword(text, out kw))
        {
            return CreateToken(TokenType.Operator, span, text, kw);
        }
        else if (IsConditionalKeyword(text, out kw))
        {
            return CreateToken(TokenType.Conditional, span, text, kw);
        }
        else if (IsLiteralKeyword(text, out kw))
        {
            return text switch
            {
                "null" => CreateLiteralToken(span, text, Value.CreateNull(), kw),
                "true" => CreateLiteralToken(span, text, Value.CreateBoolean(true), kw),
                "false" => CreateLiteralToken(span, text, Value.CreateBoolean(false), kw),
                _ => CreateToken(TokenType.Error, span, text)
            };
        }
        else if (IsLambdaKeyword(text, out kw))
        {
            return CreateToken(TokenType.Lambda, span, text, kw);
        }
        else if (CoreBuiltin.IsBuiltin(text))
        {
            return TokenizeCoreBuiltin(span, text);
        }
        else if (CoreBuiltin.IsBuiltinMethod(text))
        {
            return TokenizeBuiltinMethod(span, text);
        }

        return CreateToken(TokenType.Identifier, span, text);
    }

    private static Token TokenizeBuiltinMethod(TokenSpan span, string builtin)
    {
        if (ConsoleBuiltin.Map.TryGetValue(builtin, out TokenName name)) { }
        else if (EnvBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (FileIOBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (LoggingBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (ListBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (MathBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (TaskBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (SysBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (TimeBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (WebServerBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (HttpBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (EncoderBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (SerializerBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (ReflectorBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (FFIBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (SocketBuiltin.Map.TryGetValue(builtin, out name)) { }
        else if (SignalBuiltin.Map.TryGetValue(builtin, out name)) { }

        return CreateToken(TokenType.Identifier, span, builtin, name);
    }

    private static Token TokenizeCoreBuiltin(TokenSpan span, string builtin)
    {
        if (CoreBuiltin.Map.TryGetValue(builtin, out TokenName name)) { }
        else if (ListBuiltin.Map.TryGetValue(builtin, out name)) { }

        return CreateToken(TokenType.Identifier, span, builtin, name);

    }

    private static TokenName GetOperatorName(string text)
    {
        return text switch
        {
            "=" => TokenName.Ops_Assign,
            "!" => TokenName.Ops_Not,
            "==" => TokenName.Ops_Equal,
            "!=" => TokenName.Ops_NotEqual,
            "<" => TokenName.Ops_LessThan,
            "<=" => TokenName.Ops_LessThanOrEqual,
            ">" => TokenName.Ops_GreaterThan,
            ">=" => TokenName.Ops_GreaterThanOrEqual,
            "+" => TokenName.Ops_Add,
            "+=" => TokenName.Ops_AddAssign,
            "-" => TokenName.Ops_Subtract,
            "-=" => TokenName.Ops_SubtractAssign,
            "*" => TokenName.Ops_Multiply,
            "*=" => TokenName.Ops_MultiplyAssign,
            "**" => TokenName.Ops_Exponent,
            "**=" => TokenName.Ops_ExponentAssign,
            "%" => TokenName.Ops_Modulus,
            "%=" => TokenName.Ops_ModuloAssign,
            "/" => TokenName.Ops_Divide,
            "/=" => TokenName.Ops_DivideAssign,
            "&&" => TokenName.Ops_And,
            "&&=" => TokenName.Ops_AndAssign,
            "||" => TokenName.Ops_Or,
            "||=" => TokenName.Ops_OrAssign,
            "^" => TokenName.Ops_BitwiseXor,
            "^=" => TokenName.Ops_BitwiseXorAssign,
            "~" => TokenName.Ops_BitwiseNot,
            "~=" => TokenName.Ops_BitwiseNotAssign,
            "|" => TokenName.Ops_BitwiseOr,
            "|=" => TokenName.Ops_BitwiseOrAssign,
            "&" => TokenName.Ops_BitwiseAnd,
            "&=" => TokenName.Ops_BitwiseAndAssign,
            "<<" => TokenName.Ops_BitwiseLeftShift,
            "<<=" => TokenName.Ops_BitwiseLeftShiftAssign,
            ">>" => TokenName.Ops_BitwiseRightShift,
            ">>=" => TokenName.Ops_BitwiseRightShiftAssign,
            ">>>" => TokenName.Ops_BitwiseUnsignedRightShift,
            ">>>=" => TokenName.Ops_BitwiseUnsignedRightShiftAssign,
            _ => TokenName.Default,
        };
    }

    private static bool IsTypenameKeyword(string text, out TokenName name)
    {
        name = TokenName.Default;

        switch (text)
        {
            case "integer":
            case "Integer":
                name = TokenName.Types_Integer;
                break;

            case "float":
            case "Float":
                name = TokenName.Types_Float;
                break;

            case "boolean":
            case "Boolean":
                name = TokenName.Types_Boolean;
                break;

            case "date":
            case "Date":
                name = TokenName.Types_Date;
                break;

            case "any":
            case "Any":
                name = TokenName.Types_Any;
                break;

            case "hashmap":
            case "Hashmap":
                name = TokenName.Types_Hashmap;
                break;

            case "lambda":
            case "Lambda":
                name = TokenName.Types_Lambda;
                break;

            case "string":
            case "String":
                name = TokenName.Types_String;
                break;

            case "list":
            case "List":
                name = TokenName.Types_List;
                break;

            case "object":
            case "Object":
                name = TokenName.Types_Object;
                break;

            case "none":
            case "None":
                name = TokenName.Types_None;
                break;

            case "pointer":
            case "Pointer":
                name = TokenName.Types_Pointer;
                break;
        }

        return name != TokenName.Default;
    }

    private static bool IsLambdaKeyword(string text, out TokenName name)
    {
        name = TokenName.Default;

        if ("with".Equals(text))
        {
            name = TokenName.KW_Lambda;
        }

        return name != TokenName.Default;
    }

    private static bool IsLiteralKeyword(string text, out TokenName name)
    {
        name = text switch
        {
            "null" => TokenName.KW_Null,
            "true" => TokenName.KW_True,
            "false" => TokenName.KW_False,
            _ => TokenName.Default,
        };

        return name != TokenName.Default;
    }

    private static bool IsLogicalKeyword(string text, out TokenName name)
    {
        name = text switch
        {
            "and" => TokenName.Ops_And,
            "or" => TokenName.Ops_Or,
            "not" => TokenName.Ops_Not,
            _ => TokenName.Default,
        };

        return name != TokenName.Default;
    }

    private static bool IsConditionalKeyword(string text, out TokenName name)
    {
        name = text switch
        {
            "if" => TokenName.KW_If,
            "elsif" => TokenName.KW_ElseIf,
            "else" => TokenName.KW_Else,
            "end" => TokenName.KW_End,
            "case" => TokenName.KW_Case,
            _ => TokenName.Default,
        };

        return name != TokenName.Default;
    }

    private static bool IsKeyword(string text, out TokenName name)
    {
        name = text switch
        {
            "abstract" => TokenName.KW_Abstract,
            "as" => TokenName.KW_As,
            "break" => TokenName.KW_Break,
            "catch" => TokenName.KW_Catch,
            "const" => TokenName.KW_Const,
            "do" => TokenName.KW_Do,
            "eprint" => TokenName.KW_EPrint,
            "eprintln" => TokenName.KW_EPrintLn,
            "exit" => TokenName.KW_Exit,
            "export" => TokenName.KW_Export,
            "finally" => TokenName.KW_Finally,
            "for" => TokenName.KW_For,
            "spawn" => TokenName.KW_Spawn,
            "yield" => TokenName.KW_Yield,
            "resume" => TokenName.KW_Resume,
            "import" => TokenName.KW_Import,
            "in" => TokenName.KW_In,
            "interface" => TokenName.KW_Interface,
            "def" => TokenName.KW_Method,
            "fn" => TokenName.KW_Method,
            "package" => TokenName.KW_Package,
            "next" => TokenName.KW_Next,
            "override" => TokenName.KW_Override,
            "parse" => TokenName.KW_Parse,
            "pass" => TokenName.KW_Pass,
            "print" => TokenName.KW_Print,
            "println" => TokenName.KW_PrintLn,
            "printxy" => TokenName.KW_PrintXy,
            "private" => TokenName.KW_Private,
            "repeat" => TokenName.KW_Repeat,
            "return" => TokenName.KW_Return,
            "static" => TokenName.KW_Static,
            "struct" => TokenName.KW_Struct,
            "throw" => TokenName.KW_Throw,
            "try" => TokenName.KW_Try,
            "var" => TokenName.KW_Var,
            "when" => TokenName.KW_When,
            "while" => TokenName.KW_While,
            _ => TokenName.Default,
        };

        return name != TokenName.Default;
    }

    private static Token TokenizeParen(TokenSpan span, char c) => CreateToken(c == '(' ? TokenType.LParen : TokenType.RParen, span, $"{c}");

    private static Token TokenizeBracket(TokenSpan span, char c) => CreateToken(c == '[' ? TokenType.LBracket : TokenType.RBracket, span, $"{c}");

    private static Token TokenizeBrace(TokenSpan span, char c) => CreateToken(c == '{' ? TokenType.LBrace : TokenType.RBrace, span, $"{c}");

    private static Token CreateStringLiteralToken(TokenSpan span, string text, Value value, TokenName name = TokenName.Default) => new(TokenType.String, name, span, text, value);

    private static Token CreateLiteralToken(TokenSpan span, string text, Value value, TokenName name = TokenName.Default) => new(TokenType.Literal, name, span, text, value);

    private static Token CreateToken(TokenType type, TokenSpan span, string text, TokenName name = TokenName.Default) => new(type, name, span, text, Value.Default);

    private TokenSpan CreateSpan() => new(File, LineNumber, Position);
}