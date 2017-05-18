using SharpData.Databases;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.Oracle {
    public class OracleSpecificExceptionsTests : SpecificExceptionsTests {
        public OracleSpecificExceptionsTests() {
            _dataClient = DBBuilder.GetDataClient(DbProviderType.OracleOdp);
            _database = _dataClient.Database;
        }
    }
}