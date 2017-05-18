using System;
using SharpData.Databases;
using SharpData.Databases.Oracle;
using SharpData.Tests.Integration.Data;

namespace SharpData.Tests.Integration.Oracle {
    public class OracleOdpDataClientFactoryTests : DataClientFactoryTests {
        public override DbProviderType GetDatabaseType() {
            return DbProviderType.OracleOdp;
        }

        public override Type GetDataProviderType() {
            return typeof(OracleOdpProvider);
        }

        public override Type GetDataClientType() {
            return typeof(OracleDataClient);
        }

        public override Type GetDialectType() {
            return typeof(OracleDialect);
        }
    }
}