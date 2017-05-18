using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using SharpData.Schema;

namespace Sharp.Tests.Data.Schema {
   
    public class TableTests {
       
        [Fact]
        public void ConstructorTest() {
            var table = new Table("name");
            Assert.Equal("name", table.Name);
            Assert.NotNull(table.Columns);
            Assert.Equal(0, table.Columns.Count);
        }
    }
}
