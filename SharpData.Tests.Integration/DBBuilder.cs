using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using Npgsql;
using SharpData.Databases;
using OracleClientFactory = Oracle.ManagedDataAccess.Client.OracleClientFactory;
using OracleDataAccess = Oracle.DataAccess.Client;

namespace SharpData.Tests.Integration {
    public static class DBBuilder {
        private static Dictionary<DbProviderType, SharpFactory> _factories = new Dictionary<DbProviderType, SharpFactory>();

        static DBBuilder() {
            AddFactoryOrNull(DbProviderType.SqlServer, ConnectionStrings.SqlServer, () => SqlClientFactory.Instance);
            AddFactoryOrNull(DbProviderType.MySql, ConnectionStrings.Mysql, () => new MySqlClientFactory());
            AddFactoryOrNull(DbProviderType.OracleManaged, ConnectionStrings.Oracle, () => new OracleClientFactory());
            AddFactoryOrNull(DbProviderType.OracleOdp, ConnectionStrings.Oracle, () => new OracleDataAccess.OracleClientFactory());
            AddFactoryOrNull(DbProviderType.PostgreSql, ConnectionStrings.Postgre, () => NpgsqlFactory.Instance);
            AddFactoryOrNull(DbProviderType.SqLite, ConnectionStrings.Sqlite, () => SQLiteFactory.Instance);
        }

        public static void AddFactoryOrNull(DbProviderType dbProviderType, 
                                            string connectionString, 
                                            Func<DbProviderFactory> createAction) {
            DbProviderFactory factory = null;
            try {
                factory = createAction.Invoke();
            }
            catch {
                //sorry, continue the other tests
            }
            _factories.Add(dbProviderType, new SharpFactory(factory, ConnectionStrings.SqlServer));
        }

        public static IDataClient GetDataClient(DbProviderType databaseProvider) {
            return _factories[databaseProvider].CreateDataClient();
        }

        public static string GetConnectionString(DbProviderType databaseProvider) {
            return _factories[databaseProvider].ConnectionString;
        }
    }
}