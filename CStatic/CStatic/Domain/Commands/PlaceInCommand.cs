using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CStatic.Domain.Commands
{
    public class PlaceInCommand:ICommand
    {
        public string Name
        {
            get { return "placein"; }
        }

        public StringBuilder Run(CommandContext ctx)
        {
            var filePath = ctx.SiteConfig.GetSourcePathToFile(ctx.Match.Args.ElementAt(0));
            var filetext = Processor.Process(ProcessRequest.FromExistingRequest(ctx, 
                filePath));

            var matches = CommandProcessor.GetRegexMatches(filetext.Text.ToString());
            ctx.Text = ctx.Text.Replace(ctx.Match.Match.Value, "");

            Func<StringBuilder> final = null;
            foreach (var m in matches)
            {
                var info = CommandProcessor.ParseCommandMatches(m);
                if (info.CommandName == "placeholder")
                {
                    final = ()=> 
                    {
                        
                        return filetext.Text.Replace(m.Value, ctx.Text.ToString());
                    };
                }
            }

            return final();

        }
    }
}
