using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CStatic.Domain
{
    public class SiteConfig
    {
        public List<ItemConfig> Items { get; set; }
        public string WorkingDir { get; set; }
        public string ExportDir { get; set; }

        public string[] DistIgnorePaths { get; set; }

        public SiteConfig()
        {
            Items = new List<ItemConfig>();
            DistIgnorePaths = new string[] { };
        }

        
    }
}
