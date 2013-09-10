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
            string filePath = ctx.SiteConfig.GetSourcePathToFile(file);
            var incText = Processor.Process(ProcessRequest.FromExistingRequest(ctx, filePath));

            return ctx.Text.Replace(ctx.Match.Match.Value, incText.Text.ToString());
        }

       
    }
}