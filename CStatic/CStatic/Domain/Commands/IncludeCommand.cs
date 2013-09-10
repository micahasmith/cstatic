using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CStatic.Domain.Commands
{
    public class IncludeCommand:ICommand
    {
        public string Name
        {
            get { return "include"; }
        }

        public StringBuilder Run(CommandContext ctx)
        {
            var file = ctx.Match.Args.ElementAt(0);
            var incText = Runner.GetFileText(ctx.SiteConfig, file);

            return ctx.Text.Replace(ctx.Match.Match.Value, incText.ToString());
        }

       
    }
}