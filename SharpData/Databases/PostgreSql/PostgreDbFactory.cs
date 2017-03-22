using System.Data.Common;

namespace Sharp.Data.Databases.PostgreSql {
    public class PostgreDbFactory : DbFactory {
        public PostgreDbFactory(DbProviderFactory dbProviderFactory, string connectionString) : 
            base(dbProviderFactory, connectionString) { }
        public override IDataProvider CreateDataProvider() {
            return new PostgreSqlProvider(DbProviderFactory);
        }

        public override Dialect CreateDialect() {
            return new PostgreSqlDialect();
        }

        public override IDataClient CreateDataClient() {
            return new PostgreSqlDataClient(CreateDatabase(), CreateDialect());
        }
    }
}
