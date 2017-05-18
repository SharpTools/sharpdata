using SharpData.Databases;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.PostgreSql {
    public class PostgreSqlSpecificExceptionsTests : SpecificExceptionsTests {
        public PostgreSqlSpecificExceptionsTests() {
            _dataClient = DBBuilder.GetDataClient(DbProviderType.PostgreSql);
            _database = _dataClient.Database;
        } 
    }
}
