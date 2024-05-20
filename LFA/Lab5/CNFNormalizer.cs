using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab5
{
    public class CNFNormalizer
    {
        private List<string> nonTerminals;
        private List<string> terminals;
        private Dictionary<string, List<string>> productions;
        private string startSymbol;

        public CNFNormalizer(List<string> nonTerminals, List<string> terminals,
                             Dictionary<string, List<string>> productions, string startSymbol)
        {
            this.nonTerminals = nonTerminals;
            this.terminals = terminals;
            this.productions = productions;
            this.startSymbol = startSymbol;
        }

        public Dictionary<string, List<string>> NormalizeGrammar()
        {
            EliminateEpsilonProductions();
            EliminateRenaming();
            EliminateNonProductiveSymbols();
            EliminateInaccessibleSymbols();
            ConvertToCNF();
            return productions;
        }

        private void EliminateEpsilonProductions()
        {
            var nullable = new HashSet<string>();
            foreach (var nonTerminal in productions.Keys)
            {
                foreach (var production in productions[nonTerminal])
                {
                    if (production == "ε")
                    {
                        nullable.Add(nonTerminal);
                        break;
                    }
                }
            }

            bool changes;
            do
            {
                changes = false;
                foreach (var nonTerminal in productions.Keys)
                {
                    foreach (var production in productions[nonTerminal])
                    {
                        if (production.All(c => nullable.Contains(c.ToString())))
                        {
                            if (nullable.Add(nonTerminal))
                            {
                                changes = true;
                            }
                        }
                    }
                }
            } while (changes);

            var newProductions = new Dictionary<string, List<string>>();
            foreach (var nonTerminal in productions.Keys.ToList())
            {
                var updatedProductions = new List<string>();
                foreach (var production in productions[nonTerminal])
                {
                    if (production != "ε")
                    {
                        var sb = new StringBuilder(production);
                        for (int i = 0; i < sb.Length; i++)
                        {
                            var symbol = sb[i].ToString();
                            if (nullable.Contains(symbol))
                            {
                                sb.Remove(i, 1);
                                i--;
                                if (sb.Length > 0)
                                {
                                    updatedProductions.Add(sb.ToString());
                                }
                            }
                        }
                        updatedProductions.Add(production);
                    }
                }
                newProductions[nonTerminal] = updatedProductions.Distinct().ToList();
            }
            productions = newProductions;
            foreach (var productionList in productions.Values)
            {
                productionList.Remove("ε");
            }
        }

        private void EliminateRenaming()
        {
            bool changesMade;
            do
            {
                changesMade = false;
                var newProductions = new Dictionary<string, List<string>>();

                foreach (var nonTerminal in productions.Keys)
                {
                    var currentProductions = new List<string>(productions[nonTerminal]);
                    var toAdd = new List<string>();

                    foreach (var production in currentProductions)
                    {
                        if (production.Length == 1 && nonTerminals.Contains(production))
                        {
                            foreach (var redirectedProduction in productions[production])
                            {
                                if (!currentProductions.Contains(redirectedProduction) && !toAdd.Contains(redirectedProduction))
                                {
                                    toAdd.Add(redirectedProduction);
                                    changesMade = true;
                                }
                            }
                        }
                        else
                        {
                            toAdd.Add(production);
                        }
                    }
                    newProductions[nonTerminal] = toAdd.Distinct().ToList();
                }
                productions = newProductions;
            } while (changesMade);
        }

        private void EliminateNonProductiveSymbols()
        {
            var productive = new HashSet<string>(terminals);
            bool changes;
            do
            {
                changes = false;
                foreach (var nonTerminal in productions.Keys)
                {
                    foreach (var production in productions[nonTerminal])
                    {
                        if (production.All(c => productive.Contains(c.ToString()) || nonTerminals.Contains(c.ToString())))
                        {
                            if (productive.Add(nonTerminal))
                            {
                                changes = true;
                            }
                        }
                    }
                }
            } while (changes);

            var newProductions = new Dictionary<string, List<string>>();
            foreach (var entry in productions)
            {
                if (productive.Contains(entry.Key))
                {
                    var newProductionList = entry.Value.Where(production => production.All(c => productive.Contains(c.ToString()))).ToList();
                    if (newProductionList.Any())
                    {
                        newProductions[entry.Key] = newProductionList;
                    }
                }
            }
            productions = newProductions;
        }

        private void EliminateInaccessibleSymbols()
        {
            var accessible = new HashSet<string> { startSymbol };
            var toProcess = new HashSet<string> { startSymbol };

            while (toProcess.Any())
            {
                var nextRound = new HashSet<string>();
                foreach (var symbol in toProcess)
                {
                    foreach (var production in productions.GetValueOrDefault(symbol, new List<string>()))
                    {
                        foreach (var c in production)
                        {
                            var strC = c.ToString();
                            if (nonTerminals.Contains(strC) && accessible.Add(strC))
                            {
                                nextRound.Add(strC);
                            }
                        }
                    }
                }
                toProcess = nextRound;
            }

            productions = productions.Where(entry => accessible.Contains(entry.Key)).ToDictionary(entry => entry.Key, entry => entry.Value);
        }

        private void ConvertToCNF()
        {
            var terminalReplacements = new Dictionary<string, string>();
            var productionReplacements = new Dictionary<string, string>();
            var newNonTerminals = new List<string> { "Ш", "Щ", "Ч", "Ц", "Ж", "Й", "Ъ", "Э", "Ю", "Б", "Ь", "Г", "Ы", "П", "Я", "И" };
            int newNonTerminalIndex = 0;

            var newProductions = new Dictionary<string, List<string>>();
            foreach (var entry in productions)
            {
                var modifiedProductions = new List<string>();
                foreach (var production in entry.Value)
                {
                    if (production.Length == 1 && terminals.Contains(production))
                    {
                        modifiedProductions.Add(production);
                        continue;
                    }

                    var newProduction = new StringBuilder();
                    foreach (var c in production)
                    {
                        var symbol = c.ToString();
                        if (terminals.Contains(symbol))
                        {
                            if (!terminalReplacements.ContainsKey(symbol))
                            {
                                terminalReplacements[symbol] = newNonTerminals[newNonTerminalIndex++ % newNonTerminals.Count];
                            }
                            newProduction.Append(terminalReplacements[symbol]);
                        }
                        else
                        {
                            newProduction.Append(symbol);
                        }
                    }
                    modifiedProductions.Add(newProduction.ToString());
                }
                newProductions[entry.Key] = modifiedProductions;
            }

            foreach (var replacement in terminalReplacements)
            {
                newProductions[replacement.Value] = new List<string> { replacement.Key };
            }

            var finalProductions = new Dictionary<string, List<string>>();
            foreach (var entry in newProductions)
            {
                var updatedProductions = new List<string>();
                foreach (var production in entry.Value)
                {
                    if (production.Length > 2)
                    {
                        var remainingProduction = production;
                        while (remainingProduction.Length > 2)
                        {
                            var firstTwoSymbols = remainingProduction.Substring(0, 2);
                            remainingProduction = remainingProduction.Substring(2);

                            if (!productionReplacements.ContainsKey(firstTwoSymbols))
                            {
                                var newSymbol = newNonTerminals[newNonTerminalIndex++ % newNonTerminals.Count];
                                productionReplacements[firstTwoSymbols] = newSymbol;
                                finalProductions[newSymbol] = new List<string> { firstTwoSymbols };
                            }

                            remainingProduction = productionReplacements[firstTwoSymbols] + remainingProduction;
                        }
                        updatedProductions.Add(remainingProduction);
                    }
                    else
                    {
                        updatedProductions.Add(production);
                    }
                }
                finalProductions[entry.Key] = updatedProductions;
            }

            productions = finalProductions;
        }
    }
}
