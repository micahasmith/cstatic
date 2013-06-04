using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CStatic.Domain.Commands
{
    public class HiCommand:ICommand
    {
        public string Name { get { return "hi"; } }



        public StringBuilder Run(SiteConfig sConfig, IEnumerable<string> args, StringBuilder text, Match match)
        {
            return text.Replace(match.Value, "hi");
        }
    }
}
