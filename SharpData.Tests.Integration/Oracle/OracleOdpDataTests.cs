using System;
using Xunit;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Data.Schema;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Oracle {
	
	public class OracleOdpDataTests : DataClientDataTests {

	    protected override string GetDataProviderName() {
	        return DataProviderNames.OracleOdp;
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
			Assert.Equal(1, Convert.ToInt32(res[0][1]));
		}
	    
	}
}