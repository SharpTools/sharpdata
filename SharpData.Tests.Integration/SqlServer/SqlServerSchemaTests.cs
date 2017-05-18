using SharpData.Databases;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.SqlServer {
    public class SqlServerSchemaTests : DataClientSchemaTests {
        protected override DbProviderType GetDataProviderName() {
            return DbProviderType.SqlServer;
        }
    }
}