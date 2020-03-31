using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace MUDhub.Core.Tests
{
    public class TestClass
    {
        [Fact]
        public void TestMethode()
        {
            MyTestClass @class = new MyTestClass();
            var str = @class.MyTest(true);
            Assert.Contains("hallo", str);
        }

        [Fact]
        public void TestMethode2()
        {
            MyTestClass @class = new MyTestClass();
            var str = @class.MyTest(false);
            Assert.Contains("welt", str);
        }
    }
}
