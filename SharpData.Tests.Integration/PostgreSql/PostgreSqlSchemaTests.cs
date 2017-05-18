using SharpData.Databases;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.PostgreSql {   
    public class PostgreSqlSchemaTests : DataClientSchemaTests {
        protected override DbProviderType GetDataProviderName() {
            return DbProviderType.PostgreSql;
        }
    }
}