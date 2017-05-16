using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.PostgreSql {
    public class PostgreSqlDataTests : DataClientDataTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.PostgreSql;
        }
    }
}