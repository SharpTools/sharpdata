using SharpData.Databases;
using SharpData.Schema;
using SharpData.Tests.Integration.Data;
using Xunit;

namespace SharpData.Tests.Integration.Mysql {
    public class MySqlDataTests : DataClientDataTests {
        protected override DbProviderType GetDataProviderName() {
            return DbProviderType.MySql;
        }

        [Fact(Skip = "Not implemented yet. Pull requests welcome!")]
        public override void Can_insert_returning_id() {
            DataClient.AddTable("footable",
                Column.AutoIncrement("id"),
                Column.String("name")
            );
            Assert.Equal(1, DataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id"));
            Assert.Equal(2, DataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id"));
            Assert.Equal(3, DataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id"));
        }
    }
}