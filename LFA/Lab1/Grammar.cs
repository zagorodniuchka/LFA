using System;
using System.Collections.Generic;

class Grammar
{
    private readonly Dictionary<char, List<string>> rules = new Dictionary<char, List<string>>();
    private readonly Random random = new Random();
    private readonly HashSet<string> generatedStrings = new HashSet<string>();

    public Grammar()
    {
        InitializeRules();
    }

    private void InitializeRules()
    {
        rules['S'] = new List<string> { "aB" };
        rules['B'] = new List<string> { "bS", "aC", "c" };
        rules['C'] = new List<string> { "bD" };
        rules['D'] = new List<string> { "c", "aC" };
    }

    public string GenerateString()
    {
        string generatedString;
        do
        {
            generatedString = Expand('S');
        } while (generatedStrings.Contains(generatedString));

        generatedStrings.Add(generatedString);
        return generatedString;
    }

    private string Expand(char symbol)
    {
        var result = "";
        if (rules.ContainsKey(symbol))
        {
            var possibleProductions = rules[symbol];
            var production = possibleProductions[random.Next(possibleProductions.Count)];
            foreach (var nextSymbol in production)
            {
                result += Expand(nextSymbol);
            }
        }
        else
        {
            result += symbol;
        }
        return result;
    }
}
