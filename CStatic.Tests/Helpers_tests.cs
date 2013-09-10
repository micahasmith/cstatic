using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CStatic.Tests
{
    public class Helpers_tests
    {
        [Fact]
        public void can_get_test_dir()
        {
            var dir = Helpers.GetTestDirectory();
            Assert.Contains("CStatic.Tests", dir);
            Assert.DoesNotContain("bin", dir);
        }
    }
}
