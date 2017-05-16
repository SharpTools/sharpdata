using System;
using Sharp.Data.Databases;
using Sharp.Data.Schema;
using Sharp.Tests.Databases.Data;
using Xunit;

namespace Sharp.Tests.Databases.Mysql {
    public class MySqlDataTests : DataClientDataTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.MySql;
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