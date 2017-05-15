using Xunit;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.PostgreSql {
    public class PostgreSqlSpecificExceptionsTests : SpecificExceptionsTests {
        public PostgreSqlSpecificExceptionsTests() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.PostgreSql);
            _database = _dataClient.Database;
        } 
    }
}
