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
    public class GrabVarCommand_tests
    {
        private static TestContext _Context = null;

        static GrabVarCommand_tests()
        {
            _Context = new TestContext("GrabVarCommand_tests");

        }
        [Fact]
        public void can_use_in_siteconfig_vars()
        {
            var runner = new Runner();
            runner.Run(_Context.SiteConfig);
            string outPath = Path.Combine(_Context.TestDir, "dist", "out1.html");
            Assert.True(File.Exists(outPath));


            string outText = File.ReadAllText(outPath);
            Assert.Contains("<title>saying hello", outText);
            Assert.Contains("content=\"this is", outText);
            Console.WriteLine("final html:{0}", outText);
        }


    }
}
