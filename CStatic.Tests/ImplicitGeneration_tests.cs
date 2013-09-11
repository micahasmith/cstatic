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
    public class ImplicitGeneration_tests
    {
        private static TestContext _Context = null;

        static ImplicitGeneration_tests()
        {
            _Context = new TestContext("ImplicitGeneration_tests");

        }
        [Fact]
        public void can_do_include()
        {
            var runner = new Runner();
            runner.Run(_Context.SiteConfig);
            string outPath = Path.Combine(_Context.TestDir, "dist", "index.html");
            Assert.True(File.Exists(outPath));

            string outText = File.ReadAllText(outPath);




            Console.WriteLine("final html:{0}", outText);

        }
    }
}
