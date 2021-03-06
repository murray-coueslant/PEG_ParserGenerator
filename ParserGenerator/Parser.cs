using System;
using System.Collections.Generic;

namespace ParserGenerator
{
    public class Parser
    {
        private Tokenizer _tokenizer;
        private Memoizer _memoizer;
        private Dictionary<int, Memo> _memos;
        public string _parsing;
        public int _lineCount;
        public Tokenizer Tokenizer { get { return _tokenizer; } }

        public Dictionary<int, Memo> Memos { get => _memos; }
        public Memoizer Memoizer { get => _memoizer; }

        public Parser()
        {
            _tokenizer = new Tokenizer();
            _memoizer = new Memoizer();
            _memos = new Dictionary<int, Memo>();
        }

        public int Mark()
        {
            int _pos = Tokenizer.Mark();
            System.Console.WriteLine("Marking at [ " + _pos.ToString() + " ]");
            return _pos;
        }

        public void Reset(int pos)
        {
            System.Console.WriteLine("Resetting parser to position [ " + pos.ToString() + " ]");
            Tokenizer.Reset(pos);
        }

        public Token Expect(string argument)
        {
            Token _token = Tokenizer.PeekToken();

            while (_token.Type.ToString() != argument && _token.String != argument && (_token.Type.ToString() == "WHITESPACE" || string.IsNullOrWhiteSpace(_token.String)))
            {
                _token = Tokenizer.PeekToken();
                if (_token.Type.ToString() == "WHITESPACE" || string.IsNullOrWhiteSpace(_token.String))
                {
                    _token = Tokenizer.NextToken();
                }
            }

            if (_token.Type.ToString() == argument || _token.String == argument)
            {
                System.Console.WriteLine("Expected [ " + argument + " ] and saw [ type: " + _token.Type.ToString() + " string: " + _token.String + " ] while parsing [ " + _parsing + " ]");
                Tokenizer.NextToken();
                return _token;
            }
            // did not see expected token
            System.Console.WriteLine("Expected [ " + argument + " ] but saw [ type: " + _token.Type.ToString() + " string: " + _token.String + " ] while parsing [ " + _parsing + " ]");
            return null;
        }

        public Object Memoize(Func<Object> f)
        {
            return Memoizer.Memoize(this, f);
        }

        // public Object MemoizeLeftRecWrapper(Func<Object> f)
        // {
        //     return MemoizeLeftRec(f, Tuple.Create(new Object()));
        // }

        // private Object MemoizeLeftRec(Func<Object> f, Tuple<Object> args)
        // {
        //     int _pos = Mark();
        //     Memo _memo;
        //     Object _result;
        //     int _endPos;
        //     if (!(_memos.TryGetValue(_pos, out _memo)))
        //     {
        //         Memo _newMemo = new Memo();
        //         _memos.Add(_pos, _newMemo);
        //         _memo = _newMemo;
        //     }
        //     Tuple<Func<Object>, Tuple<Object>> _key = Tuple.Create(f, args);
        //     if (_memo.Memos.ContainsKey(_key))
        //     {
        //         Tuple<Object, int> _memoEntry;
        //         _memo.Memos.TryGetValue(_key, out _memoEntry);
        //         _result = _memoEntry.Item1;
        //         _endPos = _memoEntry.Item2;
        //         Reset(_endPos);
        //     }
        //     else
        //     {
        //         _memo.Memos.Add(_key, Tuple.Create(new Object(), _endPos));
        //         _result = f();
        //         _endPos = Mark();

        //     }
        //     return _result;
        // }
    }
}