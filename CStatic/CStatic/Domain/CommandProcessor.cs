using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CStatic.Domain
{
    public class CommandProcessor
    {
        /// <summary>
        /// Extracts command name and arguments out of the regex match
        /// </summary>
        public static CommandMatch ParseCommandMatches(Match m)
        {
            var val = m.Value;
            int start = val.LastIndexOf("{");
            int end = val.IndexOf("}");
            val = val.Substring(start + 1, (end - 1) - start);
            string[] parts = null;
            int indexOfEqual = val.IndexOf('=');
            int indexOfSpace = val.IndexOf(' ');

            if (indexOfEqual == -1)
                parts = new string[] { val.Remove(indexOfSpace), val.Substring(indexOfSpace + 1) };
            if (indexOfSpace == -1)
                parts = new string[] { val.Remove(indexOfEqual), val.Substring(indexOfEqual + 1) };

            IEnumerable<string> args = new List<string>();
            if (parts.Length > 0)
            {
                try
                {
                    args = parts[1].Split(',').Select(i => i.Trim());
                    return new CommandMatch()
                    {
                        Args = args,
                        CommandName = parts[0].ToLower(),
                        Match = m
                    };
                }
                catch (Exception e)
                {
                    throw new ApplicationException("error parsing the command " + val, e);
                }
            }
            return null;
        }


        public static IEnumerable<Match> GetRegexMatches(string text)
        {
            foreach (Match m in Regex.Matches(text, @"(\<\![-]*\s*\{\{)[^{]*(\}\}\s*[-]*\>)"))
                yield return m;

            yield break;
        }

        public static IEnumerable<CommandMatch> GetCommandMatchesFromText(string text)
        {
            return GetRegexMatches(text).Select(ParseCommandMatches);
        }
    }
}
