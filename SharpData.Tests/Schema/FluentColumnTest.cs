using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using System.Data;
using SharpData.Schema;

namespace Sharp.Tests.Data.Schema {
   
    public class FluentColumnTest {

        [Fact]
        public void FluentCreationTests() {
            string col = "COL1";

            FluentColumn f = Column.Binary(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Binary, f.Object.Type);

            f = Column.String(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.String, f.Object.Type);

            f = Column.Int16(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Int16, f.Object.Type);

            f = Column.Int32(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Int32, f.Object.Type);

            f = Column.Int64(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Int64, f.Object.Type);

            f = Column.Boolean(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Boolean, f.Object.Type);

            f = Column.Binary(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Binary, f.Object.Type);

            f = Column.Date(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Date, f.Object.Type);

            f = Column.Decimal(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Decimal, f.Object.Type);

            f = Column.Single(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Single, f.Object.Type);

            f = Column.Double(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Double, f.Object.Type);

            f = Column.Guid(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.Guid, f.Object.Type);

            f = Column.Clob(col);
            Assert.Equal(col, f.Object.ColumnName);
            Assert.Equal(DbType.String, f.Object.Type);
            Assert.Equal(Int32.MaxValue, f.Object.Size);
        }

        [Fact]
        public void Fluent_can_set_default_value() {
            Column c = Column.String("COL").DefaultValue("foo").Object;
            Assert.Equal("foo", c.DefaultValue);
        }

        [Fact]
        public void Fluent_can_set_autoIncrement() {
            Column c = Column.AutoIncrement("COL").Object;
            Assert.True(c.IsAutoIncrement);
        }

        [Fact]
        public void Fluent_can_set_is_not_null() {
            Column c = Column.String("COL").NotNull().Object;
            Assert.False(c.IsNullable);
        }

        [Fact]
        public void Fluent_can_set_comment() {
            string comment = "This is a comment";
            Column c = Column.String("COL").Comment(comment).Object;
            Assert.Equal(comment, c.Comment);
        }

        [Fact]
        public void ComplexExample() {
            Column c = Column.String("COL").DefaultValue("foo").NotNull().AsPrimaryKey().Object;
            Assert.Equal("COL", c.ColumnName);
            Assert.Equal("foo", c.DefaultValue);
            Assert.True(c.IsPrimaryKey);
            Assert.False(c.IsNullable);
        }
    }
}
