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
            {"placein",new PlaceInCommand()},
            {"getvar",new GetVarCommand()},
        };

        private static ExecutionTracker _Tracker = new ExecutionTracker();

        public static ProcessResult Process(ProcessRequest req)
        {
            var result = new ProcessResult();
            req.SourceFileName = req.SourceFileName.Replace("\\", "/");

            //try to get a cached value first
            if (!string.IsNullOrEmpty(req.SourceFileName))
            {
                var cached = _Tracker.GetResult(req.SourceFileName);
                if (cached != null)
                {
                    Console.WriteLine("Processing: serving {0} from cache", req.SourceFileName.Replace(req.SiteConfig.WorkingDir, ""));
                    var cachedText = new StringBuilder(cached.Text.ToString());

                    //even if a final page is cached, we still may need to mix vars into its output
                    var cacheMatches = CommandProcessor.GetCommandMatchesFromText(cached.Text.ToString())
                        .Where(i => i.CommandName == "var" || i.CommandName == "vars" || i.CommandName == "getvar")
                        .Where(i => !req.ExcludeCommands.Contains(i.CommandName));
                    var fvars = CompileVars(req.ItemConfig, req.Vars, cachedText.ToString(), cacheMatches);
                    foreach (var match in cacheMatches)
                        cachedText = ProcessMatch(req, cachedText, fvars, match);
                    result.Text = cachedText;
                    return result;
                }
            }

            //read the actual item's text
            string text = File.ReadAllText(req.SourceFileName);
            var sb = new StringBuilder(text);

            //get all matches
            var matches = CommandProcessor.GetCommandMatchesFromText(text);

            var finalVars = CompileVars(req.ItemConfig, req.Vars, text, matches);

            foreach (var match in matches)
            {
                sb = ProcessMatch(req, sb, finalVars, match);
            }
            result.Text = sb;

            if (!string.IsNullOrEmpty(req.SourceFileName))
                _Tracker.AddResult(req, result);
            

            return result;
        }

        private static StringBuilder ProcessMatch(ProcessRequest req, StringBuilder sb, Dictionary<string, string> finalVars, CommandMatch match)
        {
            Console.WriteLine("Processing {0} :: {1}", req.SourceFileName, match.Match.Value);
            if (_Commands.ContainsKey(match.CommandName.ToLower()))
            {
                sb = _Commands[match.CommandName.ToLower()].Run(new CommandContext(req, sb, match, finalVars));
            }
            else
            {
                Console.WriteLine("Processing: {0} does not match a registered command", match.CommandName);
            }
            return sb;
        }

        private static Dictionary<string, string> CompileVars(ItemConfig item, Dictionary<string, string> vars, string text, IEnumerable<CommandMatch> matches)
        {
            var globalVars = new Dictionary<string, string>();
            var now = DateTime.Now;
            globalVars["date.year"] = now.Year.ToString();
            globalVars["date.month"] = now.Month.ToString();
            globalVars["date.day"] = now.Day.ToString();

            //start placing in vars
            var finalVars = new Dictionary<string, string>();
            finalVars.MixIn(globalVars);

            if (vars != null)
                finalVars.MixIn(vars);

            var pageVars = GetPageVars(text, matches);
            if (pageVars != null)
                finalVars.MixIn(pageVars);

            if (item != null && item.Vars != null)
                finalVars.MixIn(item.Vars);
            return finalVars;
        }

        private static Dictionary<string, string> GetPageVars(string text, IEnumerable<CommandMatch> matches)
        {
            var r = new Dictionary<string, string>();
            matches.Where(i => i.CommandName == "var" || i.CommandName == "vars")
                .SelectMany(i => i.Args.GetArgs())
                .ForEach((i) => r[i.Key] = i.Value);
            return r;
            
        }
    }
}
