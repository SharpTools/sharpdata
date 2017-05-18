using System;
using System.Data.Common;

namespace SharpData {
    public abstract class DbFactory {
        public string ConnectionString { get; set; }
        public DbProviderFactory DbProviderFactory { get; set; }

        protected DbFactory(DbProviderFactory dbProviderFactory, string connectionString) {
            ConnectionString = connectionString;
            DbProviderFactory = dbProviderFactory;
        }

        public abstract IDataProvider CreateDataProvider();
        public virtual IDatabase CreateDatabase() {
            return new Database(CreateDataProvider(), ConnectionString);
        }
        public abstract Dialect CreateDialect();
        public abstract IDataClient CreateDataClient();
    }
}