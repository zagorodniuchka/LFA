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
}