using System;
using System.Text;
using System.Text.RegularExpressions;

namespace RegexGeneratorApp
{
    public class RegexGenerator
    {
        // Initialize a Random object for generating random numbers.
        private static readonly Random random = new Random();

        public static string GenerateFromRegex(string regex)
        {
            var result = new StringBuilder();
            // Store the last appended sequence or character
            string lastAppended = "";
            // Iterate through each character of the regex pattern.
            for (int i = 0; i < regex.Length; i++)
            {
                char c = regex[i];
                switch (c)
                {
                    case '(':
                        // Find the matching ')'
                        int closingParenthesis = regex.IndexOf(')', i);
                        // Check if followed by '*'.
                        bool asterisk = closingParenthesis + 1 < regex.Length && regex[closingParenthesis + 1] == '*';
                        // Split options within the group.
                        string[] options = regex.Substring(i + 1, closingParenthesis - i - 1).Split('|');

                        // case '*'
                        if (asterisk)
                        {
                            // If followed by '*', decide to append an option and possibly repeat it.
                            if (random.Next(2) == 0)
                            {
                                int option = random.Next(options.Length);
                                lastAppended = options[option];
                                result.Append(lastAppended);

                                // Decide on repeating the chosen option 0 to 4 more times
                                int repetitions = random.Next(5);
                                for (int j = 0; j < repetitions; j++)
                                {
                                    result.Append(lastAppended);
                                }
                            }
                        }
                        else
                        {
                            // If not followed by '*', select and append a random option.
                            lastAppended = options[random.Next(options.Length)];
                            result.Append(lastAppended);
                        }

                        // Skip to ')'
                        i = closingParenthesis;
                        if (asterisk)
                        {
                            // Skip also the '*' character
                            i++;
                        }
                        break;
                    case '+':
                        // Append 1 to 4 repetitions of the previous character/group
                        int newRepetitions = 1 + random.Next(4);
                        for (int j = 0; j < newRepetitions; j++)
                        {
                            result.Append(lastAppended);
                        }
                        break;
                    case '^':
                        // Find the next '.' to get the repetition count
                        int dot = regex.IndexOf('.', i);
                        if (dot != -1)
                        {
                            // Extract the number between '^' and '.' for repetition count
                            string numStr = regex.Substring(i + 1, dot - i - 1);
                            int repeatCount;
                            if (int.TryParse(numStr, out repeatCount))
                            {
                                // Repeat the previous character/group as specified. Subtract 1 because it already exists once
                                for (int j = 0; j < repeatCount - 1; j++)
                                {
                                    result.Append(lastAppended);
                                }
                                // Move the index to the position of the dot
                                i = dot;
                            }
                            else
                            {
                                lastAppended = c.ToString();
                                // If no '.' is found, treat '^' as a literal
                                result.Append(c);
                            }
                        }
                        break;
                    default:
                        // Directly append literal characters and fixed sequences
                        lastAppended = c.ToString();
                        result.Append(c);
                        break;
                }
            }
            // Return generated string
            return result.ToString();
        }

        public static void DescribeRegexProcessing(string regex)
        {
            var description = new StringBuilder();
            int stepCounter = 1;

            description.Append("Processing sequence for regex: ").Append(regex).Append("\n");

            for (int i = 0; i < regex.Length; i++)
            {
                char c = regex[i];
                switch (c)
                {
                    case '(':
                        int closingParenthesis = regex.IndexOf(')', i);
                        bool asterisk = closingParenthesis + 1 < regex.Length && regex[closingParenthesis + 1] == '*';
                        description.Append(stepCounter++).Append(". Found a group '(").Append(regex.Substring(i + 1, closingParenthesis - i - 1)).Append(")'.\n");
                        if (asterisk)
                        {
                            description.Append(stepCounter++).Append(". This group is followed by '*', indicating it can be chosen zero or more times.\n");
                        }
                        else
                        {
                            description.Append(stepCounter++).Append(". This group will be chosen exactly once.\n");
                        }
                        i = closingParenthesis;
                        break;
                    case '+':
                        description.Append(stepCounter++).Append(". Found a '+', indicating the previous character/group will be repeated one to four times.\n");
                        break;
                    case '^':
                        int dot = regex.IndexOf('.', i);
                        if (dot != -1)
                        {
                            string numStr = regex.Substring(i + 1, dot - i - 1);
                            int repeatCount = 1;
                            if (int.TryParse(numStr, out repeatCount))
                            {
                                description.Append(stepCounter++).Append(". Found a '^', specifying to repeat the previous character/group ").Append(repeatCount).Append(" times.\n");
                                i = dot;
                            }
                            else
                            {
                                description.Append(stepCounter++).Append(". Found a '^' but no following numerical repetition specification, treating as literal.\n");
                            }
                        }
                        break;
                    default:
                        description.Append(stepCounter++).Append(". Found a literal character '").Append(c).Append("'.\n");
                        break;
                }
            }

            description.Append(stepCounter).Append(". End of processing.\n");
            Console.WriteLine(description);
        }
    }
}
