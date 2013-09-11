using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoreLinq;

namespace CStatic.Domain.Commands
{
    public class GetVarCommand:ICommand
    {

        public string Name
        {
            get { return "getvar"; }
        }

        public StringBuilder Run(CommandContext ctx)
        {
            if (ctx.Vars == null)
            {
                Console.WriteLine("can't process var for {0}", ctx.Match.Match.Value);
                return ctx.Text;
            }

            var args = ctx.Match.Args.GetArgs().AsDictionary();

            //if we need to pull in vars from another file
            if (args.ContainsKey("from"))
            {
                var newVars = new Dictionary<string,string>();
                string fileName = ctx.SiteConfig.GetSourcePathToFile(args["from"]);
                var processResult = Processor.Process(ProcessRequest.FromExistingRequest(ctx, fileName));
                CommandProcessor.GetCommandMatchesFromText(processResult.Text.ToString())
                    .Where(i => i.CommandName == "var" || i.CommandName == "val")
                    .Select(i => i.Args.GetArgs().AsDictionary())
                    .ForEach(d => d.Keys.ForEach(i => newVars[i] = d[i]));

                //add in the cached items vars -- if it was cached
                // then they would not be in there
                newVars.MixIn(processResult.Vars);

                return GetVal(ctx.Match.Args.ElementAt(0), newVars, ctx.Text, ctx.Match.Match.Value);
                    
            }

            return GetVal(ctx.Match.Args.ElementAt(0), ctx.Vars, ctx.Text, ctx.Match.Match.Value);

        }

        private static StringBuilder GetVal(string key, Dictionary<string, string> vars, StringBuilder text, string matchValue)
        {
            var arg = key;
            if (string.IsNullOrEmpty(arg) || !vars.ContainsKey(arg))
            {
                Console.WriteLine("referenced arg for {0} doesnt exist", matchValue);
                return text;
            }

            var val = vars[arg];
            return text.Replace(matchValue, val);

        }
    }
}
