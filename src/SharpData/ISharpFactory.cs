using System.Data.Common;

namespace Sharp.Data {
    public interface ISharpFactory {
        string ConnectionString { get; set; }
        DbProviderFactory DbProviderFactory { get; set; }

        IDataProvider CreateDataProvider(DbProviderFactory dbProviderFactory);
        IDataProvider CreateDataProvider();

        IDatabase CreateDatabase(DbProviderFactory dbProviderFactory, string connectionString);
        IDatabase CreateDatabase();

        IDataClient CreateDataClient(DbProviderFactory dbProviderFactory, string connectionString);
        IDataClient CreateDataClient();

        Dialect CreateDialect(DbProviderFactory dbProviderFactory);
        Dialect CreateDialect();
    }
}