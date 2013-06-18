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

        public StringBuilder Run(SiteConfig sConfig, ItemConfig item, IEnumerable<string> args, StringBuilder text, Match match)
        {
            var filetext = Runner.GetFileText(sConfig, args.ElementAt(0));

            var matches = Processor.GetMatches(filetext.ToString());
            text = text.Replace(match.Value, "");
            foreach (var m in matches)
            {
                var info = Processor.GetCommandInfoFromMatch(m);
                if (info.Item1 != "placeholder") continue;
                return filetext.Replace(m.Value,text.ToString());
            }

            Console.WriteLine("couldn't find placeholder for {0}", match.Value);
            return new StringBuilder();
        }
    }
}
