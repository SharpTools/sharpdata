using System.Data.Common;

namespace Sharp.Data {
    public interface ISharpFactory {
        string ConnectionString { get; }
        DbProviderFactory DbProviderFactory { get; }
        IDataProvider CreateDataProvider();
        IDatabase CreateDatabase();
        IDataClient CreateDataClient();
        Dialect CreateDialect();
    }
}