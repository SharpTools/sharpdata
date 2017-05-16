using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.SqlServer {
    public class SqlServerSchemaTests : DataClientSchemaTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.SqlServer;
        }
    }
}