using System;
using SharpData.Databases;
using SharpData.Databases.SqLite;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.SQLite {
    public class SqLiteDataClientFactoryTests : DataClientFactoryTests {
        public override DbProviderType GetDatabaseType() {
            return DbProviderType.SqLite;
        }

        public override Type GetDataProviderType() {
            return typeof(SqLiteProvider);
        }

        public override Type GetDataClientType() {
            return typeof(SqLiteDataClient);
        }

        public override Type GetDialectType() {
            return typeof(SqLiteDialect);
        }
    }
}