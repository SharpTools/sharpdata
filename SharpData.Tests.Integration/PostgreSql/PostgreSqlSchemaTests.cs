using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.PostgreSql {
    public class PostgreSqlSchemaTests : DataClientSchemaTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.PostgreSql;
        }
    }
}