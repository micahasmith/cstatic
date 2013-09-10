using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CStatic.Domain
{
    public class ItemConfig
    {
        public string Id { get; set; }
        public string Source { get; set; }
        public string Dest { get; set; }

        public Dictionary<string, string> Vars { get; set; }

        public bool HadRun { get; set; }

        public ItemConfig()
        {
            Vars = new Dictionary<string, string>();
        }
    }
}
