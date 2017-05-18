using SharpData.Databases;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.PostgreSql {
    public class PostgreSqlDataTests : DataClientDataTests {
        protected override DbProviderType GetDataProviderName() {
            return DbProviderType.PostgreSql;
        }
    }
}