class FiniteAutomaton
{
    private readonly HashSet<string> states = new HashSet<string> { "S", "B", "C", "D" };
    private readonly HashSet<char> alphabet = new HashSet<char> { 'a', 'b', 'c' };
    private readonly Dictionary<Tuple<string, char>, string> transitions = new Dictionary<Tuple<string, char>, string>
    {
        { Tuple.Create("S", 'a'), "B" },
        { Tuple.Create("B", 'b'), "S" },
        { Tuple.Create("B", 'a'), "C" },
        { Tuple.Create("B", 'c'), "c" },
        { Tuple.Create("C", 'b'), "D" },
        { Tuple.Create("D", 'c'), "c" },
        { Tuple.Create("D", 'a'), "C" }
    };
    private string currentState = "S";
    private readonly HashSet<string> acceptingStates = new HashSet<string> { "c" };

    public bool Transition(char symbol)
    {
        var key = Tuple.Create(currentState, symbol);

        if (transitions.ContainsKey(key))
        {
            currentState = transitions[key];
            return true;
        }
        return false;
    }

    public bool IsStringAccepted(string inputString)
    {
        foreach (var symbol in inputString)
        {
            if (!alphabet.Contains(symbol) || !Transition(symbol))
            {
                return false;
            }
        }
        return acceptingStates.Contains(currentState);
    }
}
