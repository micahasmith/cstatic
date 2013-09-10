using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CStatic.Domain.Commands
{
    public class CommandContext
    {
        public ProcessRequest FromRequest { get; private set; }
        public SiteConfig SiteConfig { get {return FromRequest.SiteConfig; }}
        public ItemConfig Item { get{return FromRequest.ItemConfig;} }
        public StringBuilder Text {get;set;}
        public CommandMatch Match { get; private set; }
        public Dictionary<string, string> Vars { get; private set; }

        public CommandContext(ProcessRequest request, StringBuilder text, CommandMatch match, Dictionary<string, string> vars)
        {
            FromRequest = request;
            Text = text;
            Match = match;
            Vars = vars;
        }
    }
}
