using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using CStatic.Domain;
using ServiceStack.Text;

namespace CStatic.Tests
{
    public class PlaceInCommand_tests
    {
        private static TestContext _Context = null;

        static PlaceInCommand_tests()
        {
            _Context = new TestContext("PlaceInCommand_tests");

        }
        [Fact]
        public void can_do_place_in()
        {
            var runner = new Runner();
            runner.Run(_Context.SiteConfig);
            string outPath = Path.Combine(_Context.TestDir, "dist", "out1.html");
            Assert.True(File.Exists(outPath));

            string outText = File.ReadAllText(outPath);
            Assert.Contains("hello world!", outText);
        }


    }
}
