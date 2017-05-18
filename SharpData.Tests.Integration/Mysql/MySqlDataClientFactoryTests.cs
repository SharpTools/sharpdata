using System;
using SharpData.Databases;
using SharpData.Databases.MySql;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.Mysql {
    public class MySqlDataClientFactoryTests : DataClientFactoryTests {
        public override DbProviderType GetDatabaseType() {
            return DbProviderType.MySql;
        }

        public override Type GetDataProviderType() {
            return typeof(MySqlProvider);
        }

        public override Type GetDataClientType() {
            return typeof(MySqlDataClient);
        }

        public override Type GetDialectType() {
            return typeof(MySqlDialect);
        }
    }
}