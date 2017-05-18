using SharpData.Databases;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.SqlServer {
    public class SqlServerSpecifcExceptionsTests : SpecificExceptionsTests {
        public SqlServerSpecifcExceptionsTests() {
            _dataClient = DBBuilder.GetDataClient(DbProviderType.SqlServer);
            _database = _dataClient.Database;
        }
    }
}