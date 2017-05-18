using System;
using SharpData.Databases;
using SharpData.Schema;
using SharpData.Tests.Integration.Data;
using Xunit;

namespace SharpData.Tests.Integration.Oracle {
	
	public class OracleOdpDataTests : DataClientDataTests {

	    protected override DbProviderType GetDataProviderName() {
	        return DbProviderType.OracleOdp;
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