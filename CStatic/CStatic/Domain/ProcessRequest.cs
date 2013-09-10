using CStatic.Domain.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStatic.Domain
{
    public class ProcessRequest
    {
        public SiteConfig SiteConfig { get; set; }
        public ItemConfig ItemConfig { get; set; }
        public string SourceFileName { get; set; }
        public Dictionary<string, string> Vars { get; set; }
        public HashSet<string> ExcludeCommands { get; set; }

        public ProcessRequest()
        {
            Vars = new Dictionary<string, string>();
            ExcludeCommands = new HashSet<string>();
        }

        public static ProcessRequest FromExistingRequest(ProcessRequest req, string sourceFileName, Dictionary<string,string> vars = null)
        {
            var v = new Dictionary<string, string>();
            v.MixIn(req.Vars);
            v.MixIn(vars);
            return new ProcessRequest()
            {
                SiteConfig = req.SiteConfig,
                SourceFileName = sourceFileName,
                Vars = v
            };
        }

        public static ProcessRequest FromExistingRequest(CommandContext ctx, string sourceFileName, Dictionary<string, string> vars = null)
        {
            var req = ctx.FromRequest;
            var v = new Dictionary<string, string>();
            v.MixIn(req.Vars);
            v.MixIn(ctx.Vars);
            v.MixIn(vars);
            return new ProcessRequest()
            {
                SiteConfig = req.SiteConfig,
                SourceFileName = sourceFileName,
                Vars = v
            };
        }
    }
}
