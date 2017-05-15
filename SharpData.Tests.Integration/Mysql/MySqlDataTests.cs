using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Mysql {
	public class MySqlDataTests : DataClientDataTests {
		public MySqlDataTests() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.MySql);
		}
	}
}
