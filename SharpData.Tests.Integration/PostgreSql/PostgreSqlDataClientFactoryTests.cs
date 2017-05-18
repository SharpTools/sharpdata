using System;
using SharpData.Databases;
using SharpData.Databases.PostgreSql;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.PostgreSql {
    public class PostgreSqlDataClientFactoryTests : DataClientFactoryTests {
        
        public override DbProviderType GetDatabaseType() {
            return DbProviderType.PostgreSql;
        }

        public override Type GetDataProviderType() {
            return typeof(PostgreSqlProvider);
        }

        public override Type GetDataClientType() {
            return typeof(PostgreSqlDataClient);
        }

        public override Type GetDialectType() {
            return typeof(PostgreSqlDialect);
        }
    }
}
