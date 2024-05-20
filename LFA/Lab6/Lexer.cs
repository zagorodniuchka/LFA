using System;
using System.Collections.Generic;
using System.Text;

namespace Lab6
{
    enum TokenType
    {
        Integer,
        Operator,
        Punctuation,
        Whitespace,
        Separators,
        Identifier,
        EOF
    }

    class Token
    {
        public TokenType Type { get; }
        public string Value { get; }

        public Token(TokenType type, string value)
        {
            Type = type;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Type}('{Value}')";
        }
    }

    class Lexer
    {
        private readonly string _input;
        private int _pos = 0;
        private char _currentChar;

        public Lexer(string input)
        {
            _input = input + '\0';
            _currentChar = _input[_pos];
        }

        private void Advance()
        {
            _pos++;
            _currentChar = _pos >= _input.Length ? '\0' : _input[_pos];
        }

        private void SkipWhitespace()
        {
            while (_currentChar != '\0' && char.IsWhiteSpace(_currentChar))
            {
                Advance();
            }
        }

        private Token Integer()
        {
            var result = new StringBuilder();
            while (_currentChar != '\0' && char.IsDigit(_currentChar))
            {
                result.Append(_currentChar);
                Advance();
            }
            return new Token(TokenType.Integer, result.ToString());
        }

        private Token Identifier()
        {
            var result = new StringBuilder();
            while (_currentChar != '\0' && char.IsLetterOrDigit(_currentChar))
            {
                result.Append(_currentChar);
                Advance();
            }
            return new Token(TokenType.Identifier, result.ToString());
        }

        private Token Operator()
        {
            var token = new Token(TokenType.Operator, _currentChar.ToString());
            Advance();
            return token;
        }

        private Token Punctuation()
        {
            var token = new Token(TokenType.Punctuation, _currentChar.ToString());
            Advance();
            return token;
        }

        private Token Separators()
        {
            var token = new Token(TokenType.Separators, _currentChar.ToString());
            Advance();
            return token;
        }

        public List<Token> Tokenize()
        {
            var tokens = new List<Token>();

            while (_currentChar != '\0')
            {
                if (char.IsWhiteSpace(_currentChar))
                {
                    SkipWhitespace();
                    continue;
                }

                if (char.IsDigit(_currentChar))
                {
                    tokens.Add(Integer());
                    continue;
                }

                if (char.IsLetter(_currentChar))
                {
                    tokens.Add(Identifier());
                    continue;
                }

                if ("+-*/%=><!".Contains(_currentChar))
                {
                    tokens.Add(Operator());
                    continue;
                }

                if (".,?!:;".Contains(_currentChar))
                {
                    tokens.Add(Punctuation());
                    continue;
                }

                if (")({}][\"'".Contains(_currentChar))
                {
                    tokens.Add(Separators());
                    continue;
                }

                throw new Exception($"Unexpected character: {_currentChar}");
            }

            tokens.Add(new Token(TokenType.EOF, ""));
            return tokens;
        }
    }
}
