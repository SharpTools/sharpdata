using System;
using SharpData.Databases;
using SharpData.Databases.SqlServer;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.SqlServer {
    public class SqlServerDataClientFactoryTests : DataClientFactoryTests {
        public override DbProviderType GetDatabaseType() {
            return DbProviderType.SqlServer;
        }

        public override Type GetDataProviderType() {
            return typeof(SqlProvider);
        }

        public override Type GetDataClientType() {
            return typeof(SqlServerDataClient);
        }

        public override Type GetDialectType() {
            return typeof(SqlDialect);
        }
    }
}