using System;
using Lab5;
using Lab6;
using RegexGeneratorApp;

class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Enter lab:");
        var labInput = Console.ReadLine();

        switch (labInput)
        {
            case "lab1":
                RunLab1();
                break;

            case "lab2":
                RunLab2();
                break;

            case "lab3":
                RunLab3();
                break;

            case "lab4":  
                RunLab4();
                break;

            case "lab5":
                RunLab5();
                break;

            case "lab6":
                RunLab6();
                break;

            default:
                Console.WriteLine("Coming soon");
                break;
        }
    }

    private static void RunLab1()
    {
        var grammar = new Grammar();
        for (var i = 0; i < 5; i++)
        {
            Console.WriteLine(grammar.GenerateString());
        }

        var fa = new FiniteAutomaton();
        Console.WriteLine("Enter a string to check:");
        var inputString = Console.ReadLine();

        var isValid = fa.IsStringAccepted(inputString);
        if (isValid)
        {
            Console.WriteLine("The string is accepted by the automaton.");
        }
        else
        {
            Console.WriteLine("The string is not accepted by the automaton.");
        }
    }

    private static void RunLab2()
    {
        try
        {
            ChomskyGrammar myGrammar = new ChomskyGrammar();
            Console.WriteLine($"Grammar Classification: {myGrammar.Classify()}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in lab2: {ex.Message}");
        }
    }

    private static void RunLab3()
    {
        try
        {
            Console.WriteLine("Enter an arithmetic expression:");
            string input = Console.ReadLine();
            Lexer lexer = new Lexer(input);

            Token token;
            do
            {
                token = lexer.GetNextToken();
                Console.WriteLine(token);
            } while (token.Type != Token.TokenType.EOF);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in lab3: {ex.Message}");
        }
    }

    private static void RunLab4()
    {
        Console.WriteLine("Generating strings for regex 1:");
        Console.WriteLine(RegexGenerator.GenerateFromRegex("(S|T)(U|V)W*Y+24"));

        Console.WriteLine("Generating strings for regex 2:");
        Console.WriteLine(RegexGenerator.GenerateFromRegex("L(M|N)O^3P*Q(2|3)"));

        Console.WriteLine("Generating strings for regex 3:");
        Console.WriteLine(RegexGenerator.GenerateFromRegex("R*S(T|U|V)W(X|Y|Z)^2"));

        Console.WriteLine("Describe regex processing for next regex:");
        RegexGenerator.DescribeRegexProcessing("R*S(T|U|V)W(X|Y|Z)^2");
    }

    private static void RunLab5()
    {
        var nonTerminals = new List<string> { "S", "A", "B", "C", "D" };
        var terminals = new List<string> { "a", "b" };
        var productions = new Dictionary<string, List<string>>
        {
            { "S", new List<string> { "aB", "DA" } },
            { "A", new List<string> { "a", "BD", "aDADB" } },
            { "B", new List<string> { "b", "ASB" } },
            { "D", new List<string> { "ε", "BA" } },
            { "C", new List<string> { "BA" } }
        };

        var normalizer = new CNFNormalizer(nonTerminals, terminals, productions, "S");
        var cnfProductions = normalizer.NormalizeGrammar();

        foreach (var production in cnfProductions)
        {
            Console.WriteLine($"{production.Key} -> {string.Join(" | ", production.Value)}");
        }
    }

    private static void RunLab6()
    {
        Console.Write("Enter: ");
        string input = Console.ReadLine();

        var lexer = new Lab6.Lexer(input);
        List<Lab6.Token> tokens = lexer.Tokenize();
        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }

        var parser = new Lab6.Parser(tokens);
        Lab6.ASTNode ast = parser.Parse();
        PrintAST(ast, 0);
    }

    private static void PrintAST(ASTNode node, int indent)
    {
        if (node is BinaryOperatorNode binOpNode)
        {
            PrintIndent(indent);
            Console.WriteLine($"BinaryOperatorNode: {binOpNode.Operator.Value}");
            PrintAST(binOpNode.Left, indent + 2);
            PrintAST(binOpNode.Right, indent + 2);
        }
        else if (node is UnaryOperatorNode unOpNode)
        {
            PrintIndent(indent);
            Console.WriteLine($"UnaryOperatorNode: {unOpNode.Operator.Value}");
            PrintAST(unOpNode.Node, indent + 2);
        }
        else if (node is NumberNode numberNode)
        {
            PrintIndent(indent);
            Console.WriteLine($"NumberNode: {numberNode.Token.Value}");
        }
        else if (node is VariableNode variableNode)
        {
            PrintIndent(indent);
            Console.WriteLine($"VariableNode: {variableNode.Token.Value}");
        }
        else if (node is AssignmentNode assignNode)
        {
            PrintIndent(indent);
            Console.WriteLine($"AssignmentNode:");
            PrintIndent(indent + 2);
            Console.WriteLine($"Variable: {assignNode.Variable.Token.Value}");
            PrintAST(assignNode.Expression, indent + 2);
        }
    }

    static void PrintIndent(int indent)
    {
        for (int i = 0; i < indent; i++)
        {
            Console.Write(" ");
        }
    }
}
