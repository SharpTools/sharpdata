using System;
using System.Collections.Generic;
using Sharp.Data.Databases;
using Sharp.Data.Databases.MySql;
using Sharp.Data.Databases.Oracle;
using Sharp.Data.Databases.PostgreSql;
using Sharp.Data.Databases.SqLite;
using Sharp.Data.Databases.SqlServer;
using System.Data.Common;
using System.Reflection;

namespace Sharp.Data {
    public class SharpFactory : ISharpFactory {

        public string ConnectionString { get; set; }
        public DbProviderFactory DbProviderFactory { get; set; }

        private Dictionary<string, Type> _dbFactoryTypes = new Dictionary<string, Type>();
        private Dictionary<string, DbFactory> _dbFactories = new Dictionary<string, DbFactory>();

        public SharpFactory() {
            _dbFactoryTypes.Add(DataProviderNames.OracleManaged, typeof(OracleManagedDbFactory));
            _dbFactoryTypes.Add(DataProviderNames.OracleOdp, typeof(OracleOdpDbFactory));
            _dbFactoryTypes.Add(DataProviderNames.MySql, typeof(MySqlDbFactory));
            _dbFactoryTypes.Add(DataProviderNames.OleDb, typeof(OleDbDbFactory));
            _dbFactoryTypes.Add(DataProviderNames.SqLite, typeof(SqLiteDbFactory));
            _dbFactoryTypes.Add(DataProviderNames.SqlServer, typeof(SqlServerDbFactory));
            _dbFactoryTypes.Add(DataProviderNames.PostgreSql, typeof(PostgreDbFactory));
        }

        public SharpFactory(DbProviderFactory dbProviderFactory, string connectionString) {
            ConnectionString = connectionString;
            DbProviderFactory = dbProviderFactory;
        }

        public IDataProvider CreateDataProvider(DbProviderFactory dbProviderFactory) {
            return GetConfig(dbProviderFactory).CreateDataProvider();
        }

        public IDataProvider CreateDataProvider() {
            return CreateDataProvider(DbProviderFactory);
        }

        public IDatabase CreateDatabase(DbProviderFactory dbProviderFactory, string connectionString) {
            return GetConfig(dbProviderFactory, connectionString).CreateDatabase();
        }

        public IDatabase CreateDatabase() {
            return CreateDatabase(DbProviderFactory, ConnectionString);
        }

        public IDataClient CreateDataClient(DbProviderFactory dbProviderFactory, string connectionString) {
            return GetConfig(dbProviderFactory, connectionString).CreateDataClient();
        }

        public IDataClient CreateDataClient() {
            return CreateDataClient(DbProviderFactory, ConnectionString);
        }

        public Dialect CreateDialect(DbProviderFactory dbProviderFactory) {
            return GetConfig(dbProviderFactory).CreateDialect();
        }

        public Dialect CreateDialect() {
            return CreateDialect(DbProviderFactory);
        }

        private DbFactory GetConfig(DbProviderFactory dbProviderFactory) {
            return GetConfig(dbProviderFactory, ConnectionString);
        }

        private DbFactory GetConfig(DbProviderFactory dbProviderFactory, string connectionString) {
            var providerName = GetProviderName(dbProviderFactory);
            EnsureProvider(providerName);
            EnsureProviderInstance(dbProviderFactory, connectionString);
            return _dbFactories[providerName];
        }

        private string GetProviderName(DbProviderFactory dbProviderFactory) {
            return dbProviderFactory.GetType().Namespace;
        }

        private void EnsureProvider(string databaseProviderName) {
            lock (_sync) {
                if (!_dbFactoryTypes.ContainsKey(databaseProviderName)) {
                    throw new ProviderNotFoundException("SharpData does not support provider " + databaseProviderName);
                }
            }
        }

        private void EnsureProviderInstance(DbProviderFactory dbProviderFactory, string connectionString) {
            var databaseProviderName = GetProviderName(dbProviderFactory);
            lock (_sync) {
                if (!_dbFactories.ContainsKey(databaseProviderName)) {
                    _dbFactories.Add(databaseProviderName, (DbFactory)Activator.CreateInstance(_dbFactoryTypes[databaseProviderName], dbProviderFactory, connectionString));
                    return;
                }
                _dbFactories[databaseProviderName].ConnectionString = connectionString;
            }

        }

        private static ISharpFactory _default;
        public static ISharpFactory Default {
            get {
                lock (_sync) {
                    return _default ?? (_default = new SharpFactory());
                }
            }
            set { _default = value; }
        }

        private static object _sync = new object();
    }
}
