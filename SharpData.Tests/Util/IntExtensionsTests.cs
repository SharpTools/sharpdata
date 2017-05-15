using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sharp.Data.Util;
using Xunit;

namespace Sharp.Tests.Data.Util {
   
    public class IntExtensionsTests {

        [Fact]
        public void BetweenTests() {
            int ten = 10;

            Assert.True(ten.Between(9, 11));
            Assert.True(ten.Between(10, 10));
            Assert.True(ten.Between(-1, 20));
            Assert.False(ten.Between(11, 9));
            Assert.False(ten.Between(-1, 0));
            Assert.False(ten.Between(11, 12));
        }
    }
}
