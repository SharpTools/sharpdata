using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Oracle {
    public class OracleSpecificExceptionsTests : SpecificExceptionsTests {
        public OracleSpecificExceptionsTests() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.OracleOdp);
            _database = _dataClient.Database;
        }
    }
}