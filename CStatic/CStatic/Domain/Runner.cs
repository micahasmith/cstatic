using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CStatic.Domain.Commands;
using System.IO;
using ServiceStack.Text;

namespace CStatic.Domain
{
    public class Runner
    {
        public void Run(SiteConfig sConfig)
        {
            PrepDist(sConfig);
            

            foreach (var item in sConfig.Items)
            {
                RunItem(sConfig, item);
            }
        }

        public void Run(string siteConfigJson)
        {
            Run(siteConfigJson.FromJson<SiteConfig>());
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

        public string RunItem(SiteConfig sConfig, ItemConfig item,Dictionary<string,string> vars = null)
        {
            Console.WriteLine("running {0}", item.Source);
            var dist = sConfig.ExportDir;
            var itemDest = item.Dest ?? item.Source;
            var finalDest = sConfig.GetExportPathToFile(itemDest);

            if (!File.Exists(finalDest))
            {
                var result = Processor.Process(new ProcessRequest()
                {
                    SiteConfig = sConfig,
                    ItemConfig = item,
                    SourceFileName = Path.Combine(sConfig.WorkingDir, item.Source),
                    Vars = vars
                });
                Console.WriteLine("writing {0}", finalDest);
                File.WriteAllText(finalDest, result.Text.ToString());
                item.HadRun = true;
            }
            return finalDest;
        }

        



        

        
    }

   

    
}
