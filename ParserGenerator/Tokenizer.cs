using System.Collections.Generic;
using System.Linq;
namespace ParserGenerator
{
    public class Tokenizer
    {
        private int _pos;
        private TokenGenerator _tokenGen;
        private List<Token> _tokens;

        public Tokenizer()
        {
            _pos = 0;
            _tokens = new List<Token>();
        }

        public Token NextToken()
        {
            Token _token = PeekToken();
            _pos++;
            return _token;
        }

        public Token PeekToken()
        {
            if (_pos == _tokens.Count)
            {
                _tokens.Add(_tokenGen.NextToken());
            }
            return _tokens[_pos];
        }

        public int Mark()
        {
            return _pos;
        }

        public void Reset(int position)
        {
            _pos = position;
        }

        public void SetTokenGenerator(TokenGenerator _tokenGenerator)
        {
            _tokenGen = _tokenGenerator;
        }

        public List<Token> Tokenize()
        {
            List<Token> _tokens = new List<Token>();

            while (PeekToken().Type != TokenKind.ENDMARKER)
            {
                _tokens.Add(NextToken());
            }

            return _tokens;
        }
    }
}