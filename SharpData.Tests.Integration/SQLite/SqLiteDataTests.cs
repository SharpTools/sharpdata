using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Xunit;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.SQLite {

	public class SqLiteDataTests : DataClientDataTests {

		public SqLiteDataTests() {
			string fileName = "hot.db3";

			if (File.Exists(fileName)) {
				File.Delete(fileName);
			}

			SQLiteConnection.CreateFile(fileName);

            _dataClient = DBBuilder.GetDataClient(DataProviderNames.SqLite);
		}


		[Fact]
		public override void Can_insert_returning_id() {
			try {
				base.Can_insert_returning_id();
				Assert.Fail();
			}
			catch (NotSupportedByDialect ex) {
				Assert.Equal(ex.DialectName, "SqLiteDialect");
				Assert.Equal(ex.FunctionName, "GetInsertReturningColumnSql");
			}
		}
	}
}