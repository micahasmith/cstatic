using CStatic.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CStatic.Tests
{
    public class processor_tests
    {
        //[Fact]
        //public void can_replace_single_text()
        //{
        //    var p = new Processor();
        //    var r = p.ProcessFile(new SiteConfig(),null, "hello there <!--{{hi=hi}}--> how are you","test1");
        //    Assert.Equal("hello there hi how are you", r);
        //}

        ////[Fact]
        ////public void can_replace_multiple_text()
        ////{
        ////    var p = new Processor();
        ////    var r = p.Process(new SiteConfig(), "hello there {{hi=hi}} {{hi=jim}} how are you");
        ////    Assert.Equal("hello there hi hi how are you", r);
        ////}


        //[Fact]
        //public void can_replace_multiple_html_comment_style()
        //{
        //    var p = new Processor();
        //    var r = p.ProcessFile(new SiteConfig(), "hello there <!--{{hi=hi}}--> <!-- {{hi=jim}} --> how are you","test2");
        //    Assert.Equal("hello there hi hi how are you", r);
        //}
    }
}
