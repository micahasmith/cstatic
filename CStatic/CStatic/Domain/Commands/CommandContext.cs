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
        public SiteConfig SiteConfig { get; set; }
        public ItemConfig Item { get; set; }
        public StringBuilder Text {get;set;}
        public CommandMatch Match { get; set; }
        public Dictionary<string, string> Vars { get; set; }
    }
}
