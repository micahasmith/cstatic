using CStatic.Domain.Commands;
using CStatic.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MoreLinq;

namespace CStatic.Domain
{
    public class Processor
    {
        //todo dyn gen this based off /commands contents
        private static Dictionary<string, ICommand> _Commands = new Dictionary<string, ICommand>()
        {
            {"include",new IncludeCommand()},
            {"hi",new HiCommand()},
            {"placein",new PlaceInCommand()},
            {"getvar",new GetVarCommand()},
        };

        public string ProcessFile(SiteConfig sconfig, ItemConfig item, string fileName, Dictionary<string,string> vars = null)
        {
            fileName = fileName.Replace("\\","/");
            
            //try to get a cached value first
            if (!string.IsNullOrEmpty(fileName))
            {
                var cached = Cacher.Get(fileName);
                if (cached != null)
                {
                    Console.WriteLine("Processing: serving {0} from cache", fileName.Replace(sconfig.WorkingDir,""));
                    var cachedText = new StringBuilder(cached.ToString());
                    var cacheMatches = GetMatches(cachedText.ToString())
                        .Select(GetCommandInfoFromMatch)
                        .Where(i => i.CommandName == "var" || i.CommandName == "vars" || i.CommandName == "getvar");
                    var fvars = CompileVars(item, vars, cachedText.ToString(), cacheMatches);
                    foreach (var match in cacheMatches)
                    {
                        cachedText = ProcessFileMatch(sconfig, item, fileName, cachedText, fvars, match);
                    }
                    return cachedText.ToString();
                }
            }

            //read the actual item's text
            string text = File.ReadAllText(fileName);
            var sb = new StringBuilder(text);

            //get all matches
            var matches = GetMatches(text).Select(GetCommandInfoFromMatch);

            var finalVars = CompileVars(item, vars, text, matches);

            foreach (var match in matches)
            {
                sb = ProcessFileMatch(sconfig, item, fileName, sb, finalVars, match);
            }

            if (!string.IsNullOrEmpty(fileName))
                Cacher.Set(fileName, sb.ToString());
            return sb.ToString();
        }

        private Dictionary<string, string> CompileVars(ItemConfig item, Dictionary<string, string> vars, string text, IEnumerable<CommandMatch> matches)
        {
            //start placing in vars
            var finalVars = new Dictionary<string, string>();
            if (vars != null)
                finalVars.MixIn(vars);

            var pageVars = GetPageVars(text, matches);
            if (pageVars != null)
                finalVars.MixIn(pageVars);

            if (item != null && item.Vars != null)
                finalVars.MixIn(item.Vars);
            return finalVars;
        }

        private Dictionary<string, string> GetPageVars(string text, IEnumerable<CommandMatch> matches)
        {
            var r = new Dictionary<string, string>();
            matches.Where(i => i.CommandName == "var" || i.CommandName == "vars")
                .SelectMany(i => i.Args.GetArgs())
                .ForEach((i) => r[i.Key] = i.Value);
            return r;
            
        }

        private static StringBuilder ProcessFileMatch(SiteConfig sconfig, ItemConfig item, string fileName, StringBuilder sb, Dictionary<string, string> finalVars, CommandMatch match)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                Console.WriteLine("Processing {0} :: {1}", fileName, match.Match.Value);
            }

            sb = ProcessMatch(sconfig, item, sb, match, finalVars);
            return sb;
        }
        

        public static IEnumerable<Match> GetMatches(string text)
        {
            foreach (Match m in Regex.Matches(text, @"(\<\![-]*\s*\{\{)[^{]*(\}\}\s*[-]*\>)"))
                yield return m;

            yield break;
        }
   

        /// <summary>
        /// Runs the match against its corresponding command
        /// </summary>
        public static StringBuilder ProcessMatch(SiteConfig sconfig, ItemConfig item, StringBuilder sb, CommandMatch match, Dictionary<string,string> vars)
        {
            if (match != null)
            {
                if (_Commands.ContainsKey(match.CommandName.ToLower()))
                {
                    sb =  _Commands[match.CommandName.ToLower()].Run( new CommandContext(){
                        SiteConfig = sconfig,
                        Item = item,
                        Match = match,
                        Text = sb,
                        Vars = vars
                    });
                       
                    return sb;
                }
                else
                {
                    Console.WriteLine("Processing: {0} does not match a registered command", match.CommandName);
                }
            }
            return sb;
        }

        /// <summary>
        /// Extracts command name and arguments out of the regex match
        /// </summary>
        public static CommandMatch GetCommandInfoFromMatch(Match m)
        {
            var val = m.Value;
            int start = val.LastIndexOf("{");
            int end = val.IndexOf("}");
            val = val.Substring(start + 1, (end - 1) - start);
            var parts = val.Split('=').Select(i => i.Trim()).ToArray();
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
                    throw new ApplicationException("error parsing the command "+val,e);
                }
            }
            return null;
        }



    }
}
