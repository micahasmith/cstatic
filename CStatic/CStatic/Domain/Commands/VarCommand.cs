using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CStatic.Domain.Commands
{
    public class VarCommand:ICommand
    {

        public string Name
        {
            get { return "var"; }
        }

        public StringBuilder Run(SiteConfig sConfig, ItemConfig item, IEnumerable<string> args, StringBuilder text, System.Text.RegularExpressions.Match match)
        {
            if (item == null)
            {
                Console.WriteLine("can't process var for {0}", match.Value);
                return text;
            }

            var arg = args.ElementAt(0);
            if(string.IsNullOrEmpty(arg) || !item.Vars.ContainsKey(arg)){
                Console.WriteLine("referenced arg for {0} doesnt exist",match.Value);
                return text;
            }

            var val  = item.Vars[arg];
            return text.Replace(match.Value, val);

        }
    }
}
