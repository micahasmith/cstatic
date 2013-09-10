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
            var filetext = Runner.GetFileText(ctx.SiteConfig, ctx.Match.Args.ElementAt(0),ctx.Vars);

            var matches = Processor.GetMatches(filetext.ToString());
            ctx.Text = ctx.Text.Replace(ctx.Match.Match.Value, "");

            Func<StringBuilder> final = null;
            foreach (var m in matches)
            {
                var info = Processor.GetCommandInfoFromMatch(m);
                if (info.CommandName == "placeholder")
                {
                    final = ()=> 
                    {
                        
                        return filetext.Replace(m.Value, ctx.Text.ToString());
                    };
                }
            }

            return final();

        }
    }
}
