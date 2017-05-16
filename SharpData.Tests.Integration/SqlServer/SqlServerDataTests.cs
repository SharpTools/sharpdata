using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.SqlServer {
    public class SqlServerDataTests : DataClientDataTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.SqlServer;
        }
    }
}