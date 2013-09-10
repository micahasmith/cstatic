using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CStatic.Domain.Commands
{
    public class GetVarCommand:ICommand
    {

        public string Name
        {
            get { return "getvar"; }
        }

        public StringBuilder Run(CommandContext ctx)
        {
            if (ctx.Vars == null)
            {
                Console.WriteLine("can't process var for {0}", ctx.Match.Match.Value);
                return ctx.Text;
            }

            var arg = ctx.Match.Args.ElementAt(0);
            if(string.IsNullOrEmpty(arg) || !ctx.Vars.ContainsKey(arg)){
                Console.WriteLine("referenced arg for {0} doesnt exist",ctx.Match.Match.Value);
                return ctx.Text;
            }

            var val  = ctx.Vars[arg];
            return ctx.Text.Replace(ctx.Match.Match.Value, val);

        }
    }
}
