using System.Formats.Asn1;
using System.Reflection.Metadata.Ecma335;

namespace citrus.Parsing;

public class Lexer(int file, string path) : IDisposable
{
    private readonly FileStream stream = File.OpenRead(path);
    private readonly int file = file;
    private int line = 0;
    private int pos = 0;

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

            tokens.Add(token);

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

        char? ch = GetChar();
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
            return TokenizeLiteral(span, c);
        }
        else if (c == '"')
        {
            return TokenizeString(span);
        }

        return TokenizeSymbol(span, c);
    }

    private Token TokenizeSymbol(TokenSpan span, char c)
    {
        switch (c)
        {
            case '\n':
                return CreateToken(TokenType.Newline, span, Environment.NewLine);
            case ',':
                return CreateToken(TokenType.Comma, span, ",");
            case '@':
                return CreateToken(TokenType.Keyword, span, "@", TokenName.KW_This);
            case ':' when PeekChar() == ':':
                GetChar();
                return CreateToken(TokenType.Qualifier, span, "::");
            case '.' when PeekChar() == '.':
                GetChar();
                return CreateToken(TokenType.Range, span, "..");
            case ':':
                return CreateToken(TokenType.Colon, span, ":");
            case '.':
                return CreateToken(TokenType.Dot, span, ".");
            case '\\':
                return TokenizeEscaped(span);
            case '(' or ')':
                return TokenizeParen(span, c);
            case '[' or ']':
                return TokenizeBracket(span, c);
            case '{' or '}':
                return TokenizeBrace(span, c);
            case '#':
                return TokenizeComment(span);
            case '/' when PeekChar() == '#':
                return TokenizeBlockComment(span);
        }

        return TokenizeOperator(span, c);
    }

    private Token TokenizeEscaped(TokenSpan span)
    {
        char? c = GetChar();

        string? text;
        switch (c)
        {
            case '\\':
                text = "\\";
                break;
            case 'b':
                text = "\b";
                break;
            case 'r':
                text = "\r";
                break;
            case 'n':
                text = "\n";
                break;
            case 't':
                text = "\t";
                break;
            case 'f':
                text = "\f";
                break;
            case '"':
                text = "\"";
                break;
            default:
                text = $"\\{c}";
                break;
        }

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

        return CreateToken(TokenType.String, span, text);
    }

    private TokenName GetOperatorName(string text)
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
                break;
            }

            text += ch;
        }

        return CreateToken(TokenType.Comment, span, text);
    }

    private Token TokenizeParen(TokenSpan span, char c)
    {
        var text = string.Empty + c;
        return CreateToken(c == '(' ? TokenType.LParen : TokenType.RParen, span, text);
    }

    private Token TokenizeBracket(TokenSpan span, char c)
    {
        var text = string.Empty + c;
        return CreateToken(c == '[' ? TokenType.LBracket : TokenType.RBracket, span, text);
    }

    private Token TokenizeBrace(TokenSpan span, char c)
    {
        var text = string.Empty + c;
        return CreateToken(c == '{' ? TokenType.LBrace : TokenType.RBrace, span, text);
    }

    private Token TokenizeLiteral(TokenSpan span, char c)
    {
        var text = string.Empty + c;
        char? ch;
        char lastChar = '\0';

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

        if (text.Count(c => c == '.') > 1 || text.ToLower().Count(c => c == 'e') > 1)
        {
            return CreateToken(TokenType.Error, span, text);
        }

        return CreateToken(TokenType.Literal, span, text);
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
        else if (IsConditionalKeyword(text, out TokenName kwCond))
        {
            return CreateToken(TokenType.Conditional, span, text, kwCond);
        }
        else if (IsLiteralKeyword(text, out TokenName kwLiteral))
        {
            return CreateToken(TokenType.Literal, span, text, kwLiteral);
        }
        else if (IsLambdaKeyword(text, out TokenName kwLambda))
        {
            return CreateToken(TokenType.Lambda, span, text, kwLambda);
        }
        else if (IsTypenameKeyword(text, out TokenName kwTypename))
        {
            return CreateToken(TokenType.Typename, span, text, kwTypename);
        }

        return CreateToken(TokenType.Identifier, span, text);
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

            case "any":
            case "Any":
                name = TokenName.Types_Any;
                break;

            case "hashmap":
            case "Hashmap":
                name = TokenName.Types_Hash;
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
        name = TokenName.Default;

        switch (text)
        {
            case "null":
                name = TokenName.KW_Null;
                break;

            case "true":
                name = TokenName.KW_True;
                break;

            case "false":
                name = TokenName.KW_False;
                break;
        }

        return name != TokenName.Default;
    }

    private static bool IsConditionalKeyword(string text, out TokenName name)
    {
        name = TokenName.Default;

        switch (text)
        {
            case "if":
                name = TokenName.KW_If;
                break;
            case "elsif":
                name = TokenName.KW_ElseIf;
                break;
            case "else":
                name = TokenName.KW_Else;
                break;
            case "end":
                name = TokenName.KW_End;
                break;
            case "case":
                name = TokenName.KW_Case;
                break;
        }

        return name != TokenName.Default;
    }

    private static bool IsKeyword(string text, out TokenName name)
    {
        name = TokenName.Default;

        switch (text)
        {
            case "abstract":
                name = TokenName.KW_Abstract;
                break;

            case "as":
                name = TokenName.KW_As;
                break;

            case "break":
                name = TokenName.KW_Break;
                break;

            case "catch":
                name = TokenName.KW_Catch;
                break;

            case "const":
                name = TokenName.KW_Const;
                break;

            case "do":
                name = TokenName.KW_Do;
                break;

            case "eprint":
                name = TokenName.KW_EPrint;
                break;

            case "eprintln":
                name = TokenName.KW_EPrintLn;
                break;

            case "exit":
                name = TokenName.KW_Exit;
                break;

            case "export":
                name = TokenName.KW_Export;
                break;

            case "finally":
                name = TokenName.KW_Finally;
                break;

            case "for":
                name = TokenName.KW_For;
                break;

            case "spawn":
                name = TokenName.KW_Spawn;
                break;

            case "import":
                name = TokenName.KW_Import;
                break;

            case "in":
                name = TokenName.KW_In;
                break;

            case "interface":
                name = TokenName.KW_Interface;
                break;

            case "def":
            case "fn":
                name = TokenName.KW_Method;
                break;

            case "package":
                name = TokenName.KW_Package;
                break;

            case "next":
                name = TokenName.KW_Next;
                break;

            case "override":
                name = TokenName.KW_Override;
                break;

            case "parse":
                name = TokenName.KW_Parse;
                break;

            case "pass":
                name = TokenName.KW_Pass;
                break;

            case "print":
                name = TokenName.KW_Print;
                break;

            case "println":
                name = TokenName.KW_PrintLn;
                break;

            case "printxy":
                name = TokenName.KW_PrintXy;
                break;

            case "private":
                name = TokenName.KW_Private;
                break;

            case "repeat":
                name = TokenName.KW_Repeat;
                break;

            case "return":
                name = TokenName.KW_Return;
                break;

            case "static":
                name = TokenName.KW_Static;
                break;

            case "struct":
                name = TokenName.KW_Struct;
                break;

            case "throw":
                name = TokenName.KW_Throw;
                break;

            case "try":
                name = TokenName.KW_Try;
                break;

            case "var":
                name = TokenName.KW_Var;
                break;

            case "when":
                name = TokenName.KW_When;
                break;

            case "while":
                name = TokenName.KW_While;
                break;

            default:
                break;
        }

        return name != TokenName.Default;
    }

    private char? PeekChar()
    {
        long origin = stream.Position;

        int byteValue = stream.ReadByte();
        if (byteValue == -1)
        {
            return null;
        }

        char c = (char)byteValue;
        stream.Position = origin;
        return c;
    }

    private char? GetChar()
    {
        int byteValue = stream.ReadByte();
        if (byteValue == -1)
        {
            return null;
        }

        var c = (char)byteValue;

        if (c == '\r' && PeekChar() == '\n')
        {
            c = (char)stream.ReadByte();
        }

        if (c == '\n')
        {
            ++line;
            pos = 0;
        }
        else
        {
            ++pos;
        }

        return c;
    }

    private void SkipWhitespace()
    {
        int byteValue;
        while ((byteValue = stream.ReadByte()) != -1)
        {
            char c = (char)byteValue;
            if (!char.IsWhiteSpace(c) && c != '\n')
            {
                --stream.Position;
                break;
            }
        }
    }

    private Token CreateToken(TokenType type, TokenSpan span, string text, TokenName name = TokenName.Default)
    {
        return new Token(type, name, span, text);
    }

    private TokenSpan CreateSpan()
    {
        return new TokenSpan(file, line, pos);
    }
}