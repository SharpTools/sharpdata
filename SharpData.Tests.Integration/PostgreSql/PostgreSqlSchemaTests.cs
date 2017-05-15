using Xunit;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.PostgreSql {
    public class PostgreSqlSchemaTests : DataClientSchemaTests {
        public PostgreSqlSchemaTests() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.PostgreSql);
        }
    }
}
