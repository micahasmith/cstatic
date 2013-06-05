using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CStatic.Domain.Commands;
using System.IO;

namespace CStatic.Domain
{
    public class Runner
    {
        public void Run(SiteConfig sConfig)
        {
            PrepDist(sConfig);
            

            foreach (var item in sConfig.Items.AsParallel())
            {
                RunItem(sConfig, item);
            }
        }

        private static void PrepDist(SiteConfig sConfig)
        {
           
            var dist = sConfig.ExportDir;

            if (!Directory.Exists(dist))
                Directory.CreateDirectory(dist);
            else
            {
                Console.WriteLine("cleaning up {0}", dist);
                var di = new DirectoryInfo(dist);
                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
            }
            var mixin = Path.Combine(sConfig.WorkingDir, "dist-addins");
            if (Directory.Exists(mixin))
            {
                //Now Create all of the directories
                foreach (string dirPath in Directory.GetDirectories(mixin, "*",
                    SearchOption.AllDirectories))
                    Directory.CreateDirectory(dirPath.Replace("dist-addins", "dist"));

                //Copy all the files
                foreach (string newPath in Directory.GetFiles(mixin, "*.*",
                    SearchOption.AllDirectories))
                    File.Copy(newPath, newPath.Replace("dist-addins", "dist"));
            }

        }

        public string RunItem(SiteConfig sConfig, ItemConfig item)
        {
            Console.WriteLine("running {0}", item.Source);
            var dist = sConfig.ExportDir;
            var itemDest = item.Dest ?? item.Source;
            var finalDest = Path.Combine(dist, itemDest);

            EnsureDirectory(dist, itemDest);

            if (!File.Exists(finalDest))
            {
                var output = new Processor().ProcessFile(sConfig, item,Path.Combine(sConfig.WorkingDir, item.Source));
                File.WriteAllText(finalDest, output);
                item.HadRun = true;
            }
            return finalDest;
        }

        public void EnsureDirectory(string dist, string itemDest)
        {
            var working = dist;
            var parts = itemDest.Split('\\');
            foreach (var p in parts)
            {
                if (!p.Contains("."))
                {
                    working = Path.Combine(dist, p);
                    if (!Directory.Exists(working))
                        Directory.CreateDirectory(working);
                }
            }
        }

        public static StringBuilder GetFileText(SiteConfig sConfig, string file)
        {
            var distFile = sConfig.Items.FirstOrDefault(i => i.Source == file);
            string incText = null;
            if (distFile != null)
            {
                new Runner().RunItem(sConfig, distFile);
                incText = File.ReadAllText(Path.Combine(sConfig.ExportDir, file));
            }
            else
            {
                var ff = Path.Combine(sConfig.WorkingDir, file);
                incText = new Processor().ProcessFile(sConfig,null, ff);
            }
            return new StringBuilder(incText);
        }

        

        
    }

   

    
}
