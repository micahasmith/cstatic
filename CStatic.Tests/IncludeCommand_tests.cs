using CStatic.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CStatic.Tests
{
    public class IncludeCommand_tests
    {
        private static TestContext _Context = null;

        static IncludeCommand_tests()
        {
            _Context = new TestContext("IncludeCommand_tests");

        }
        [Fact]
        public void can_do_include()
        {
            var runner = new Runner();
            runner.Run(_Context.SiteConfig);
            string outPath = Path.Combine(_Context.TestDir, "dist", "out1.html");
            Assert.True(File.Exists(outPath));

            string outText = File.ReadAllText(outPath);
            Assert.Contains("hello include!", outText);
            Console.WriteLine("final html:{0}", outText);
        }
    }
}
