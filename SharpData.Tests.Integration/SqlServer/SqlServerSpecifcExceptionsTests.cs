using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.SqlServer {
    public class SqlServerSpecifcExceptionsTests : SpecificExceptionsTests {
        public SqlServerSpecifcExceptionsTests() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.SqlServer);
            _database = _dataClient.Database;
        }
    }
}