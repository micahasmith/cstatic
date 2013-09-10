using System;
using System.Collections.Generic;
using System.IO;
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

        private void EnsureDirectory(string dist, string itemDest)
        {
            var working = dist;
            var parts = itemDest.Split('\\');
            foreach (var p in parts)
            {
                if (!p.Contains("."))
                {
                    working = Path.Combine(working, p);
                    if (!Directory.Exists(working))
                        Directory.CreateDirectory(working);
                }
            }
        }

        public string GetExportPathToFile(string file)
        {
            var finalDest = Path.Combine(ExportDir, file);
            EnsureDirectory(ExportDir,file);
            return finalDest;
        }

        public string GetSourcePathToFile(string file)
        {
            var finalDest = Path.Combine(WorkingDir, file);
            return finalDest;
        }
        
    }
}
