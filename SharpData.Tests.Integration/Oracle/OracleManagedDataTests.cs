using System;
using Sharp.Data.Databases;
using Sharp.Data.Schema;
using Sharp.Tests.Databases.Data;
using Xunit;

namespace Sharp.Tests.Databases.Oracle {
    public class OracleManagedDataTests : DataClientDataTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.OracleManaged;
        }

        public override void Can_insert_dates_and_booleans() {
            DataClient.AddTable("footable",
                Column.AutoIncrement("id"),
                Column.Date("colDate"),
                Column.Boolean("colBool"));


            var now = DateTime.Now;
            DataClient.Insert.Into("footable").Columns("colDate", "colBool").Values(now, true);
            var res = DataClient.Select.Columns("colDate", "colBool").From("footable").AllRows();
            Assert.Equal(now.ToString(), res[0][0].ToString());
            Assert.Equal((short)1, res[0][1]);
        }
    }
}