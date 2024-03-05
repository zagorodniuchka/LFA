using System;
using System.Collections.Generic;

class ChomskyGrammar
{
    public HashSet<char> VN { get; } = new HashSet<char> { 'S', 'A', 'B', 'C' };
    public HashSet<char> VT { get; } = new HashSet<char> { 'a', 'b' };
    public List<ProductionRule> P { get; } = new List<ProductionRule>
    {
        new ProductionRule('S', "aA"),
        new ProductionRule('A', "bS"),
        new ProductionRule('A', "aB"),
        new ProductionRule('B', "bC"),
        new ProductionRule('C', "aA"),
        new ProductionRule('B', "aB"),
        new ProductionRule('C', "b")
    };

    public string Classify()
    {
        if (IsType3())
        {
            return "Type 3 (Regular)";
        }
        else if (IsType2())
        {
            return "Type 2 (Context-Free)";
        }
        else if (IsType1())
        {
            return "Type 1 (Context-Sensitive)";
        }
        else if (IsType0())
        {
            return "Type 0 (Unrestricted)";
        }
        else
        {
            return "Invalid grammar";
        }
    }

    private bool IsType0()
    {
        // All grammars are Type 0
        return true;
    }

    private bool IsType1()
    {
        // Check if all production rules are of the form α -> β where |α| <= |β|
        foreach (ProductionRule rule in P)
        {
            if (rule.LeftSide.ToString().Length > rule.RightSide.Length)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsType2()
    {
        // Check if all production rules are of the form A -> γ where A is a non-terminal and γ is a string of terminals or non-terminals
        foreach (ProductionRule rule in P)
        {
            if (rule.RightSide.Length == 0 || !VN.Contains(rule.LeftSide) || !IsStringOfTerminalsOrNonTerminals(rule.RightSide))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsType3()
    {
        // Check if all production rules are of the form A -> aB or A -> a where A and B are non-terminals and a is a terminal
        foreach (ProductionRule rule in P)
        {
            if (rule.RightSide.Length == 0 || !VN.Contains(rule.LeftSide) || (rule.RightSide.Length == 1 && !VT.Contains(rule.RightSide[0])))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsStringOfTerminalsOrNonTerminals(string str)
    {
        foreach (char ch in str)
        {
            if (!VT.Contains(ch) && !VN.Contains(ch))
            {
                return false;
            }
        }
        return true;
    }
}

class ProductionRule
{
    public char LeftSide { get; }
    public string RightSide { get; }

    public ProductionRule(char leftSide, string rightSide)
    {
        LeftSide = leftSide;
        RightSide = rightSide;
    }
}
