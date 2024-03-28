using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class Token
{
    public enum TokenType
    {
        Integer,
        Plus,
        Minus,
        Multiply,
        Divide,
        Equals,
        LeftParenthesis,
        RightParenthesis,
        EOF // End of file
    }

    public TokenType Type { get; }
    public string Value { get; }

    public Token(TokenType type, string value = null)
    {
        Type = type;
        Value = value;
    }

    public override string ToString()
    {
        if (Value != null)
            return $"Token({Type}, '{Value}')";
        else
            return $"Token({Type})";
    }
}

public class Lexer
{
    private readonly string _input;
    private int _position;

    public Lexer(string input)
    {
        _input = input;
        _position = 0;
    }

    private char CurrentChar => _position < _input.Length ? _input[_position] : '\0';

    private void Advance()
    {
        _position++;
    }

    private void SkipWhitespace()
    {
        while (char.IsWhiteSpace(CurrentChar))
        {
            Advance();
        }
    }

    private Token ParseInteger()
    {
        string result = "";

        while (char.IsDigit(CurrentChar))
        {
            result += CurrentChar;
            Advance();
        }

        return new Token(Token.TokenType.Integer, result);
    }

    private Token ParseSymbol()
    {
        switch (CurrentChar)
        {
            case '+':
                Advance();
                return new Token(Token.TokenType.Plus);
            case '-':
                Advance();
                return new Token(Token.TokenType.Minus);
            case '*':
                Advance();
                return new Token(Token.TokenType.Multiply);
            case '/':
                Advance();
                return new Token(Token.TokenType.Divide);
            case '=':
                Advance();
                return new Token(Token.TokenType.Equals);
            case '(':
                Advance();
                return new Token(Token.TokenType.LeftParenthesis);
            case ')':
                Advance();
                return new Token(Token.TokenType.RightParenthesis);
            default:
                throw new ArgumentException($"Invalid character: '{CurrentChar}'");
        }
    }

    public Token GetNextToken()
    {
        while (_position < _input.Length)
        {
            if (char.IsWhiteSpace(CurrentChar))
            {
                SkipWhitespace();
                continue;
            }

            if (char.IsDigit(CurrentChar))
            {
                return ParseInteger();
            }

            return ParseSymbol();
        }

        return new Token(Token.TokenType.EOF);
    }
}
