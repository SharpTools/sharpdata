using System.Data.Common;

namespace Sharp.Data.Databases.Oracle {
    public class OracleManagedDbFactory : DbFactory {
        public OracleManagedDbFactory(DbProviderFactory dbProviderFactory, string connectionString)
            : base(dbProviderFactory, connectionString) {
        }

        public override IDataProvider CreateDataProvider() {
            return new OracleManagedProvider(DbProviderFactory);
        }
        
        public override Dialect CreateDialect() {
            return new OracleDialect();
        }

        public override IDataClient CreateDataClient() {
            return new OracleDataClient(CreateDatabase(), CreateDialect());
        }
    }
}