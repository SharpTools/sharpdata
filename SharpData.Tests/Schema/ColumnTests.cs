using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Data;
using SharpData.Schema;

namespace Sharp.Tests.Data.Schema {
   
    public class ColumnTests {
       
        [Fact]
        public void Constructor_with_no_type_test() {
            var c = new Column("COL1");
            Assert.Equal(DbType.String, c.Type);
            TestDefaults(c);
        }

        [Fact]
        public void Constructor_with_type_test() {
            var c = new Column("COL1", DbType.UInt32);
            Assert.Equal(DbType.UInt32, c.Type);
            TestDefaults(c);
        }

        [Fact]
        public void When_column_is_autoIncrement_it_is_also_not_null() {
            var c = new Column("COL1", DbType.UInt32);
            c.IsAutoIncrement = true;
            Assert.False(c.IsNullable);
        }

        [Fact]
        public void When_column_is_primary_key_it_is_also_not_null() {
            var c = new Column("COL1", DbType.UInt32);
            c.IsPrimaryKey = true;
            Assert.False(c.IsNullable);
        }

        protected void TestDefaults(Column c) {
            Assert.Null(c.DefaultValue);
            Assert.Equal("COL1", c.ColumnName);
            Assert.False(c.IsAutoIncrement);
            Assert.True(c.IsNullable);
        }
    }
}
