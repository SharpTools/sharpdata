using System.Data.Common;

namespace SharpData.Databases.SqLite {
    public class SqLiteDbFactory : DbFactory {
        public SqLiteDbFactory(DbProviderFactory dbProviderFactory, string connectionString)
            : base(dbProviderFactory, connectionString) {
        }

        public override IDataProvider CreateDataProvider() {
            return new SqLiteProvider(DbProviderFactory);
        }

        public override Dialect CreateDialect() {
            return new SqLiteDialect();
        }

        public override IDataClient CreateDataClient() {
            return new SqLiteDataClient(CreateDatabase(), CreateDialect());
        }
    }
}