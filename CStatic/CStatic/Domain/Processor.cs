using CStatic.Domain.Commands;
using CStatic.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CStatic.Domain
{
    public class Processor
    {
        //todo dyn gen this based off /commands contents
        private static Dictionary<string, ICommand> _Commands = new Dictionary<string, ICommand>()
        {
            {"include",new IncludeCommand()},
            {"hi",new HiCommand()},
            {"placein",new PlaceInCommand()}
        };

        public string ProcessFile(SiteConfig sconfig, string fileName)
        {
            string subject = fileName;
            
            if (!string.IsNullOrEmpty(subject))
            {
                var cached = Cacher.Get(subject);
                if (cached != null)
                {
                    Console.WriteLine("Processing: serving {0} from cache", subject.Replace(sconfig.WorkingDir,""));
                    return cached.ToString();
                }
            }
            string text = File.ReadAllText(fileName);
            var sb = new StringBuilder(text);

            foreach (var match in GetMatches(text))
            {
                if (!string.IsNullOrEmpty(subject))
                {
                    Console.WriteLine("Processing {0} :: {1}", subject, match.Value);
                }

                var info = GetCommandInfoFromMatch(match);
                sb = ProcessMatch(sconfig, sb, match, info);
            }

            if (!string.IsNullOrEmpty(subject))
                Cacher.Set(subject, sb.ToString());
            return sb.ToString();
        }

        public string Process(SiteConfig sconfig, string text, string subject = null)
        {
            if (!string.IsNullOrEmpty(subject))
            {
                var cached = Cacher.Get(subject);
                if (cached != null)
                {
                    Console.WriteLine("Processing: serving {0} from cache", subject);
                    return cached.ToString();
                }
            }
            var sb = new StringBuilder(text);

            foreach (var match in GetMatches(text))
            {
                if (!string.IsNullOrEmpty(subject))
                {
                    Console.WriteLine("Processing {0} :: {1}", subject, match.Value);
                }
                var info = GetCommandInfoFromMatch(match);
                sb = ProcessMatch(sconfig, sb, match, info);
            }

            if (!string.IsNullOrEmpty(subject))
                Cacher.Set(subject, sb.ToString());
            return sb.ToString();
        }

        

        public static IEnumerable<Match> GetMatches(string text)
        {
            foreach (Match m in Regex.Matches(text, @"(\<\![-]*\s*\{\{)[^{]*(\}\}\s*[-]*\>)"))
                yield return m;

            yield break;
        }


       

        public static StringBuilder ProcessMatch(SiteConfig sconfig, StringBuilder sb, Match match, Tuple<string, IEnumerable<string>> cmdInfo)
        {
            if (cmdInfo != null)
            {
                if (_Commands.ContainsKey(cmdInfo.Item1.ToLower()))
                {
                    sb =  _Commands[cmdInfo.Item1].Run(sconfig, cmdInfo.Item2, sb, match);
                    return sb;
                }
                else
                {
                    Console.WriteLine("Processing: {0} does not match a registered command", cmdInfo.Item1);
                }
            }
            return sb;
        }


        public static Tuple<string, IEnumerable<string>> GetCommandInfoFromMatch(Match m)
        {
            var val = m.Value;
            int start = val.LastIndexOf("{");
            int end = val.IndexOf("}");
            val = val.Substring(start + 1, (end - 1) - start);
            var parts = val.Split('=').Select(i => i.Trim()).ToArray();
            IEnumerable<string> args = new List<string>();
            if (parts.Length > 0)
            {
                args = parts[1].Split(',').Select(i => i.Trim());
                return Tuple.Create(parts[0], args);
            }
            return null;
        }



    }
}
