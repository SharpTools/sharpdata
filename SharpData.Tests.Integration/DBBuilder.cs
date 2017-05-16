using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using Sharp.Data;
using Sharp.Data.Databases;
using SharpData.Tests.Integration;
using OracleDataAccess = Oracle.DataAccess.Client;

namespace Sharp.Tests.Databases {
    public static class DBBuilder {
        private static Dictionary<string, SharpFactory> _factories = new Dictionary<string, SharpFactory>();

        static DBBuilder() {
            _factories.Add(DataProviderNames.SqlServer,
                new SharpFactory(SqlClientFactory.Instance, ConnectionStrings.SqlServer));
            _factories.Add(DataProviderNames.MySql,
                new SharpFactory(new MySqlClientFactory(), ConnectionStrings.Mysql));
            _factories.Add(DataProviderNames.OracleManaged,
                new SharpFactory(new OracleClientFactory(), ConnectionStrings.Oracle));
            _factories.Add(DataProviderNames.OracleOdp,
                new SharpFactory(new OracleDataAccess.OracleClientFactory(), ConnectionStrings.Oracle));
            _factories.Add(DataProviderNames.PostgreSql,
                new SharpFactory(NpgsqlFactory.Instance, ConnectionStrings.Postgre));
            _factories.Add(DataProviderNames.SqLite,
                new SharpFactory(SQLiteFactory.Instance, ConnectionStrings.Sqlite));
        }

        public static IDataClient GetDataClient(string databaseProvider) {
            return _factories[databaseProvider].CreateDataClient();
        }

        public static string GetConnectionString(string databaseProvider) {
            return _factories[databaseProvider].ConnectionString;
        }
    }
}