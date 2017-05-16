using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Oracle {
    public class OracleOdpSchemaTests : DataClientSchemaTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.OracleOdp;
        }
    }
}