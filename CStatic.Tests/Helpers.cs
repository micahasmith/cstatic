using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CStatic.Tests
{
    public class Helpers
    {
        public static string GetTestDirectory()
        {
            return Environment.CurrentDirectory.Remove(Environment.CurrentDirectory.IndexOf("bin"));
        }


    }


}
