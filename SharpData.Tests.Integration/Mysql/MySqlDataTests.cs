using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Mysql {
    public class MySqlDataTests : DataClientDataTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.MySql;
        }
    }
}