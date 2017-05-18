using System;
using System.Collections.Generic;
using SharpData.Databases;
using SharpData.Databases.MySql;
using SharpData.Databases.Oracle;
using SharpData.Databases.PostgreSql;
using SharpData.Databases.SqLite;
using SharpData.Databases.SqlServer;
using System.Data.Common;

namespace SharpData {
    public class SharpFactory : ISharpFactory {

        public string ConnectionString { get; set; }
        public DbProviderFactory DbProviderFactory { get; set; }

        private static Dictionary<DbProviderType, Type> _dbFactoryTypes = new Dictionary<DbProviderType, Type> {
            {DbProviderType.OracleManaged, typeof(OracleManagedDbFactory)},
            {DbProviderType.OracleOdp, typeof(OracleOdpDbFactory)},
            {DbProviderType.MySql, typeof(MySqlDbFactory)},
            {DbProviderType.OleDb, typeof(OleDbDbFactory)},
            {DbProviderType.SqLite, typeof(SqLiteDbFactory)},
            {DbProviderType.SqlServer, typeof(SqlServerDbFactory)},
            {DbProviderType.PostgreSql, typeof(PostgreDbFactory)}
        };
        private Dictionary<DbProviderType, DbFactory> _dbFactories = new Dictionary<DbProviderType, DbFactory>();

        public SharpFactory(DbProviderFactory dbProviderFactory, string connectionString) {
            ConnectionString = connectionString;
            DbProviderFactory = dbProviderFactory;
        }
        
        public IDataProvider CreateDataProvider() {
            return GetConfig(DbProviderFactory).CreateDataProvider();
        }

        public IDatabase CreateDatabase() {
            return GetConfig(DbProviderFactory, ConnectionString).CreateDatabase();
        }

        public IDataClient CreateDataClient() {
            return GetConfig(DbProviderFactory, ConnectionString).CreateDataClient();
        }

        public Dialect CreateDialect() {
            return GetConfig(DbProviderFactory).CreateDialect();
        }

        private DbFactory GetConfig(DbProviderFactory dbProviderFactory) {
            return GetConfig(dbProviderFactory, ConnectionString);
        }

        private DbFactory GetConfig(DbProviderFactory dbProviderFactory, string connectionString) {
            var dbProviderType = GetDbProviderType(dbProviderFactory);
            EnsureProvider(dbProviderType);
            EnsureProviderInstance(dbProviderFactory, connectionString);
            return _dbFactories[dbProviderType];
        }

        private DbProviderType GetDbProviderType(DbProviderFactory dbProviderFactory) {
            var name = dbProviderFactory.GetType().Namespace;
            return DbProviderTypeExtensions.GetDbProviderByNamespace(name);
        }

        private void EnsureProvider(DbProviderType databaseProviderName) {
            lock (_sync) {
                if (!_dbFactoryTypes.ContainsKey(databaseProviderName)) {
                    throw new ProviderNotFoundException("SharpData does not support provider " + databaseProviderName);
                }
            }
        }

        private void EnsureProviderInstance(DbProviderFactory dbProviderFactory, string connectionString) {
            var databaseProviderName = GetDbProviderType(dbProviderFactory);
            lock (_sync) {
                if (!_dbFactories.ContainsKey(databaseProviderName)) {
                    _dbFactories.Add(databaseProviderName, (DbFactory)Activator.CreateInstance(_dbFactoryTypes[databaseProviderName], dbProviderFactory, connectionString));
                    return;
                }
                _dbFactories[databaseProviderName].ConnectionString = connectionString;
            }

        }

        public static ISharpFactory Default {get; set; }
        private static object _sync = new object();
    }
}
