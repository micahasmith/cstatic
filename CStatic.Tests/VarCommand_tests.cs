using CStatic.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CStatic.Tests
{
    public class VarCommand_tests
    {
        private static TestContext _Context = null;

        static VarCommand_tests()
        {
            _Context = new TestContext("VarCommand_tests");

        }
        [Fact]
        public void can_use_in_siteconfig_vars()
        {
            var runner = new Runner();
            runner.Run(_Context.SiteConfig);
            string outPath = Path.Combine(_Context.TestDir, "dist", "out1.html");
            Assert.True(File.Exists(outPath));

            string outText = File.ReadAllText(outPath);
            Assert.Contains("hello micah", outText);
            Console.WriteLine("final html:{0}", outText);
        }


        [Fact]
        public void can_use_in_placein_level_vars()
        {
            var runner = new Runner();
            runner.Run(_Context.SiteConfig);
            string outPath = Path.Combine(_Context.TestDir, "dist", "out2.html");
            Assert.True(File.Exists(outPath));

            
            string outText = File.ReadAllText(outPath);
            Assert.Contains("<title>saying hello", outText);
            Assert.Contains("content=\"this is", outText);
            Console.WriteLine("final html:{0}",outText);
        }
    }
}
