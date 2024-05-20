using System;
using System.Collections.Generic;
using static Token;

namespace Lab6
{
    class Parser
    {
        private readonly List<Token> _tokens;
        private int _currentPosition = 0;

        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
        }

        private Token CurrentToken()
        {
            return _currentPosition < _tokens.Count ? _tokens[_currentPosition] : new Token(TokenType.EOF, "");
        }

        private void Check(TokenType type)
        {
            if (CurrentToken().Type == type)
            {
                _currentPosition++;
            }
            else
            {
                throw new Exception($"Unexpected token: {CurrentToken()}, expected: {type}");
            }
        }

        private ASTNode Expression()
        {
            ASTNode result = Term();
            while (CurrentToken().Type == TokenType.Operator && (CurrentToken().Value == "+" || CurrentToken().Value == "-"))
            {
                Token op = CurrentToken();
                Check(TokenType.Operator);
                result = new BinaryOperatorNode(result, op, Term());
            }
            return result;
        }

        private ASTNode Term()
        {
            ASTNode result = Factor();
            while (CurrentToken().Type == TokenType.Operator && (CurrentToken().Value == "*" || CurrentToken().Value == "/"))
            {
                Token op = CurrentToken();
                Check(TokenType.Operator);
                result = new BinaryOperatorNode(result, op, Factor());
            }
            return result;
        }

        private ASTNode Factor()
        {
            Token token = CurrentToken();
            if (token.Type == TokenType.Integer)
            {
                Check(TokenType.Integer);
                return new NumberNode(token);
            }
            if (token.Type == TokenType.Identifier)
            {
                Check(TokenType.Identifier);
                return new VariableNode(token);
            }
            if (token.Type == TokenType.Operator && token.Value == "(")
            {
                Check(TokenType.Operator);
                ASTNode node = Expression();
                Check(TokenType.Operator);
                return node;
            }
            throw new Exception($"Unexpected token: {token}");
        }

        private ASTNode Assignment()
        {
            VariableNode variable = new VariableNode(CurrentToken());
            Check(TokenType.Identifier);
            Check(TokenType.Operator);
            ASTNode expr = Expression();
            return new AssignmentNode(variable, expr);
        }

        public ASTNode Parse()
        {
            if (CurrentToken().Type == TokenType.Identifier && _tokens[_currentPosition + 1].Value == "=")
            {
                return Assignment();
            }
            else
            {
                return Expression();
            }
        }
    }
}
