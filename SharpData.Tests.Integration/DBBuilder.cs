using System.Collections.Generic;
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
            _factories.Add(DbProviderType.SqlServer,
                new SharpFactory(SqlClientFactory.Instance, ConnectionStrings.SqlServer));
            _factories.Add(DbProviderType.MySql,
                new SharpFactory(new MySqlClientFactory(), ConnectionStrings.Mysql));
            _factories.Add(DbProviderType.OracleManaged,
                new SharpFactory(new OracleClientFactory(), ConnectionStrings.Oracle));
            _factories.Add(DbProviderType.OracleOdp,
                new SharpFactory(new OracleDataAccess.OracleClientFactory(), ConnectionStrings.Oracle));
            _factories.Add(DbProviderType.PostgreSql,
                new SharpFactory(NpgsqlFactory.Instance, ConnectionStrings.Postgre));
            _factories.Add(DbProviderType.SqLite,
                new SharpFactory(SQLiteFactory.Instance, ConnectionStrings.Sqlite));
        }

        public static IDataClient GetDataClient(DbProviderType databaseProvider) {
            return _factories[databaseProvider].CreateDataClient();
        }

        public static string GetConnectionString(DbProviderType databaseProvider) {
            return _factories[databaseProvider].ConnectionString;
        }
    }
}