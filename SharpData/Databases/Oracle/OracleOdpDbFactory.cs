using System.Data.Common;

namespace Sharp.Data.Databases.Oracle {
    public class OracleOdpDbFactory : OracleManagedDbFactory {
        public OracleOdpDbFactory(DbProviderFactory dbProviderFactory, string connectionString)
            : base(dbProviderFactory, connectionString) {
        }

        public override IDataProvider CreateDataProvider() {
            return new OracleOdpProvider(DbProviderFactory);
        }
    }
}