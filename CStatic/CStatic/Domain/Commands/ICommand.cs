using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CStatic.Domain.Commands
{
    public interface ICommand
    {
         string Name { get; }
         StringBuilder Run(CommandContext ctx);

    }
}
