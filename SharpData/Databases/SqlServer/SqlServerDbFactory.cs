using Sharp.Data.Providers;
using System.Data.Common;

namespace Sharp.Data.Databases.SqlServer {
    public class SqlServerDbFactory : DbFactory {
        public SqlServerDbFactory(DbProviderFactory dbProviderFactory, string connectionString)
            : base(dbProviderFactory, connectionString) {
        }

        public override IDataProvider CreateDataProvider() {
            return new SqlProvider(DbProviderFactory);
        }

        public override Dialect CreateDialect() {
            return new SqlDialect();
        }

        public override IDataClient CreateDataClient() {
            return new SqlServerDataClient(CreateDatabase(), CreateDialect());
        }
    }
}