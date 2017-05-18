using SharpData.Databases;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.Oracle {
    public class OracleOdpSchemaTests : DataClientSchemaTests {
        protected override DbProviderType GetDataProviderName() {
            return DbProviderType.OracleOdp;
        }
    }
}