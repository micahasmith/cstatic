using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CStatic.Domain.Commands
{
    public class HiCommand:ICommand
    {
        public string Name { get { return "hi"; } }



        public StringBuilder Run(CommandContext ctx)
        {
            return ctx.Text.Replace(ctx.Match.Match.Value, "hi");
        }
    }
}
