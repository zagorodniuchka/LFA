using System;

namespace Lab6
{
    abstract class ASTNode
    {
        // Abstract class for all AST nodes
    }

    class BinaryOperatorNode : ASTNode
    {
        public ASTNode Left { get; }
        public ASTNode Right { get; }
        public Token Operator { get; }

        public BinaryOperatorNode(ASTNode left, Token @operator, ASTNode right)
        {
            Left = left;
            Operator = @operator;
            Right = right;
        }
    }

    class UnaryOperatorNode : ASTNode
    {
        public Token Operator { get; }
        public ASTNode Node { get; }

        public UnaryOperatorNode(Token @operator, ASTNode node)
        {
            Operator = @operator;
            Node = node;
        }
    }

    class NumberNode : ASTNode
    {
        public Token Token { get; }

        public NumberNode(Token token)
        {
            Token = token;
        }
    }

    class VariableNode : ASTNode
    {
        public Token Token { get; }

        public VariableNode(Token token)
        {
            Token = token;
        }
    }

    class AssignmentNode : ASTNode
    {
        public VariableNode Variable { get; }
        public ASTNode Expression { get; }

        public AssignmentNode(VariableNode variable, ASTNode expression)
        {
            Variable = variable;
            Expression = expression;
        }
    }
}
