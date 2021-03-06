using System;
using System.Collections.Generic;
using System.Text;
namespace ParserGenerator
{
    public class TokenGenerator
    {
        private string _inputPEG;
        private int _pos;

        private HashSet<char> _symbolSet;
        private HashSet<char> _OPSet;
        public TokenGenerator(string inputPEG)
        {
            _inputPEG = inputPEG;
            _symbolSet = (new Symbols()).GetSymbols();
            _OPSet = (new Ops()).OPs;
            _pos = 0;
        }
        public Token NextToken()
        {
            Token _token = ConsumeToken();
            if (_token == null)
            {
                // raise token exception
                return new Token(TokenKind.NULLTOKEN, "");
            }
            return _token;
        }

        private Token ConsumeToken()
        {
            char _testChar = (char)Current();
            if (_testChar == '"' || _testChar == '\'')
            {
                return ConsumeString();
            }

            if (char.IsLetter(_testChar))
            {
                return ConsumeName();
            }

            if (char.IsDigit(_testChar))
            {
                return ConsumeNumber();
            }

            if (_testChar == '\n')
            {
                Token _token = new Token(TokenKind.NEWLINE, "NEWLINE");
                Next();
                return _token;
            }

            if (_symbolSet.Contains(_testChar))
            {
                return ConsumeSymbol();
            }

            if (_OPSet.Contains(_testChar))
            {
                Token _token = new Token(TokenKind.OP, (_testChar).ToString());
                Next();
                return _token;
            }

            if (char.IsWhiteSpace(_testChar))
            {
                Token _token = HandleIndent();
                if (_token == null)
                {
                    _token = new Token(TokenKind.WHITESPACE, (_testChar).ToString());
                }
                Next();
                return _token;
            }



            if (Current() == -1) // denotes the end of the file
            {
                return new Token(TokenKind.ENDMARKER, "ENDMARKER");
            }

            // failed to consume token
            System.Console.WriteLine("Failed to consume token [ " + (_testChar).ToString() + " ] at position [ " + _pos + " ]");
            return null;
        }

        private Token ConsumeString()
        {
            StringBuilder _string = new StringBuilder();
            _string.Append((char)Current());
            char _quoteType = (char)Current();
            Next();
            while ((char)Current() != _quoteType)
            {
                if ((char)Current() == '\\')
                {
                    _string.Append((char)Current());
                    Next();
                    _string.Append((char)Current());
                    Next();
                }
                else
                {
                    _string.Append((char)Current());
                    Next();
                }
            }
            _string.Append((char)Current());
            Next();
            return new Token(TokenKind.STRING, _string.ToString());
        }

        private Token ConsumeName()
        {
            StringBuilder _string = new StringBuilder();
            _string.Append((char)Current());
            Next();
            while (char.IsLetter((char)Current()) || (char)Current() == '_')
            {
                _string.Append((char)Current());
                Next();
            }

            return new Token(TokenKind.NAME, _string.ToString());
        }

        private Token ConsumeNumber()
        {
            StringBuilder _string = new StringBuilder();
            _string.Append((char)Current());
            Next();
            while (char.IsDigit((char)Current()))
            {
                _string.Append((char)Current());
                Next();
            }

            return new Token(TokenKind.NUMBER, _string.ToString());
        }

        private Token ConsumeSymbol()
        {
            TokenKind _kind;
            switch ((char)Current())
            {
                case '(':
                    _kind = TokenKind.LPAREN;
                    break;
                case ')':
                    _kind = TokenKind.RPAREN;
                    break;
                case '[':
                    _kind = TokenKind.LSQUARE;
                    break;
                case ']':
                    _kind = TokenKind.RSQUARE;
                    break;
                case ',':
                    _kind = TokenKind.COMMA;
                    break;
                case ';':
                    _kind = TokenKind.SEMICOLON;
                    break;
                case '-':
                    _kind = TokenKind.MINUS;
                    break;
                case '/':
                    _kind = TokenKind.SLASH;
                    break;
                case '<':
                    _kind = TokenKind.LESSTHAN;
                    break;
                case '>':
                    _kind = TokenKind.GREATERTHAN;
                    break;
                case '=':
                    _kind = TokenKind.EQUAL;
                    break;
                case '.':
                    _kind = TokenKind.DOT;
                    break;
                case '%':
                    _kind = TokenKind.PERCENT;
                    break;
                case '{':
                    _kind = TokenKind.LBRACE;
                    break;
                case '}':
                    _kind = TokenKind.RBRACE;
                    break;
                case '~':
                    _kind = TokenKind.TILDE;
                    break;
                case '^':
                    _kind = TokenKind.CIRCUMFLEX;
                    break;
                case '@':
                    _kind = TokenKind.AT;
                    break;
                default:
                    _kind = TokenKind.NULLTOKEN;
                    break;
            }
            if (_kind != TokenKind.NULLTOKEN)
            {
                // TODO: work out a way to use the above here, instead of just SYMBOL (mainly a problem in action parsing)
                Token _token = new Token(TokenKind.SYMBOL, ((char)Current()).ToString());
                Next();
                return _token;
            }
            // failed to consume a symbol
            return null;
        }

        private Token HandleIndent()
        {
            int _pos = Mark();
            if ((char)Current() == ' ')
            {
                while ((char)Current() == ' ')
                {
                    if ((Mark() - _pos) == 3) // could be 3 or 4 needs testing
                    {
                        return new Token(TokenKind.INDENT, "    ");
                    }
                    Next();
                }
            }

            if ((char)Current() == '\t')
            {
                return new Token(TokenKind.INDENT, "\t");
            }

            Reset(_pos);
            return null;
        }

        private int Current()
        {
            if (_pos < _inputPEG.Length)
            {
                return (int)_inputPEG[_pos];
            }
            return -1;
        }

        private void Next()
        {
            _pos++;
        }

        private int Mark()
        {
            return _pos;
        }

        private void Reset(int pos)
        {
            _pos = pos;
        }
    }
}