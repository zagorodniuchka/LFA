using System;

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
            Console.WriteLine($"Error in lab2: {ex.Message}");
        }
    }
}