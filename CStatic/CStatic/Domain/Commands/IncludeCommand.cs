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

        public StringBuilder Run(SiteConfig sConfig, ItemConfig item, IEnumerable<string> args,StringBuilder text, Match match)
        {
            var file = args.ElementAt(0);
            var incText = Runner.GetFileText(sConfig, file);

            return text.Replace(match.Value, incText.ToString());
        }

       
    }
}
