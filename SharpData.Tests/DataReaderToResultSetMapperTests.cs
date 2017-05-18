using System.Data.Common;
using Moq;
using Xunit;
using SharpData;
using SharpData.Tests;

namespace Sharp.Tests.Data {
    public class DataReaderToResultSetMapperTests {

        [Fact]
        public void Can_map_selects_with_columns() {
            var reader = new Mock<DbDataReader>();
            reader.Setup(x => x.GetName(0)).Returns("col1");
            reader.Setup(x => x.GetName(1)).Returns("col2");
            reader.Setup(x => x.FieldCount).Returns(2);
            var result = DataReaderToResultSetMapper.Map(reader.Object);
            CollectionAssert.AreEqual(new[] { "col1", "col2" }, result.GetColumnNames());
        }

        [Fact]
        public void Can_map_selects_with_two_columns_with_the_same_name() {
            var reader = new Mock<DbDataReader>();
            reader.Setup(x => x.GetName(0)).Returns("col");
            reader.Setup(x => x.GetName(1)).Returns("colx");
            reader.Setup(x => x.GetName(2)).Returns("col");
            reader.Setup(x => x.FieldCount).Returns(3);
            var result = DataReaderToResultSetMapper.Map(reader.Object);
            CollectionAssert.AreEqual(new [] {"col", "colx", "col_2"}, result.GetColumnNames());
        }
    }
}
