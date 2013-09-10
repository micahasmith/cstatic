using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CStatic.Domain
{
    public class CommandMatch
    {
        public string CommandName { get; set; }
        public Match Match { get; set; }
        public IEnumerable<string> Args { get; set; }
    }
}
