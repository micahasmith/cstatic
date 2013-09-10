using CStatic.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Text;

namespace CStatic.Tests
{
    public class TestContext
    {
        public string TestDir { get; private set; }
        public SiteConfig SiteConfig { get; private set; }

        public TestContext(string testRootDir)
        {
            TestDir = Path.Combine(Helpers.GetTestDirectory(), "testdata", testRootDir );
            SiteConfig = File.ReadAllText(Path.Combine(TestDir, "site.json")).FromJson<SiteConfig>();
            SiteConfig.ExportDir = Path.Combine(TestDir, "dist");
            SiteConfig.WorkingDir = Path.Combine(TestDir);

        }
    }
}
