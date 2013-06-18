using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;
using CStatic.Domain;
using System.IO;

namespace CStatic
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                Console.WriteLine("missing command file");

            try
            {
                var cmd = args[0];
                //var pwd = System.IO.Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);
                var pwd = Environment.CurrentDirectory;
                SiteConfig sconfig = new SiteConfig();

                if (cmd == "init")
                {
                    var p = Path.Combine(pwd, "site.json");
                    Console.WriteLine("intting @ {0}", p);
                    File.WriteAllText(p, sconfig.ToJson());
                }
                else if (cmd == "build")
                {
                    var file = args[1];

                    if (file.EndsWith(".json"))
                        sconfig = File.ReadAllText(file).FromJson<SiteConfig>();

                    if (string.IsNullOrEmpty(sconfig.WorkingDir))
                        sconfig.WorkingDir = pwd;

                    if (string.IsNullOrEmpty(sconfig.ExportDir))
                        sconfig.ExportDir = Path.Combine(pwd, "dist");

                    new Runner().Run(sconfig);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }
        }
    }
}
