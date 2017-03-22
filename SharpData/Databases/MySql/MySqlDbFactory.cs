using Sharp.Data.Providers;
using System.Data.Common;

namespace Sharp.Data.Databases.MySql {
    public class MySqlDbFactory : DbFactory {
        public MySqlDbFactory(DbProviderFactory dbProviderFactory, string connectionString)
            : base(dbProviderFactory, connectionString) {
        }

        public override IDataProvider CreateDataProvider() {
            return new MySqlProvider(DbProviderFactory);
        }

        public override Dialect CreateDialect() {
            return new MySqlDialect();
        }

        public override IDataClient CreateDataClient() {
            return new MySqlDataClient(CreateDatabase(), CreateDialect());
        }
    }
}