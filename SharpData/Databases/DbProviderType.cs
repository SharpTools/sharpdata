using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpData.Databases {
    public enum DbProviderType {
        OracleManaged,
        OracleOdp,
        MySql,
        SqlServer,
        SqLite,
        OleDb,
        PostgreSql
    }
    public static class DbProviderTypeExtensions {
        public static string GetProviderName(this DbProviderType type) {
            switch (type) {
                case DbProviderType.OracleManaged:
                    return "Oracle.ManagedDataAccess.Client";
                case DbProviderType.OracleOdp:
                    return "Oracle.DataAccess.Client";
                case DbProviderType.MySql:
                    return "MySql.Data.MySqlClient";
                case DbProviderType.SqlServer:
                    return "System.Data.SqlClient";
                case DbProviderType.SqLite:
                    return "Microsoft.Data.Sqlite";
                case DbProviderType.OleDb:
                    return "System.Data.OleDb";
                case DbProviderType.PostgreSql:
                    return "Npgsql";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static DbProviderType GetDbProviderByNamespace(string name)
        {
            if (string.Equals(name, DbProviderType.OracleManaged.GetProviderName(), StringComparison.OrdinalIgnoreCase))
                return DbProviderType.OracleManaged;
            if (string.Equals(name, DbProviderType.OracleOdp.GetProviderName(), StringComparison.OrdinalIgnoreCase))
                return DbProviderType.OracleOdp;
            if (string.Equals(name, DbProviderType.MySql.GetProviderName(), StringComparison.OrdinalIgnoreCase))
                return DbProviderType.MySql;
            if (string.Equals(name, DbProviderType.SqlServer.GetProviderName(), StringComparison.OrdinalIgnoreCase))
                return DbProviderType.SqlServer;
            if (string.Equals(name, DbProviderType.SqLite.GetProviderName(), StringComparison.OrdinalIgnoreCase))
                return DbProviderType.SqLite;
            if (string.Equals(name, DbProviderType.OleDb.GetProviderName(), StringComparison.OrdinalIgnoreCase))
                return DbProviderType.OleDb;
            if (string.Equals(name, DbProviderType.PostgreSql.GetProviderName(), StringComparison.OrdinalIgnoreCase))
                return DbProviderType.PostgreSql;
            throw new ArgumentOutOfRangeException(nameof(name), name, null);
        }

        public static List<DbProviderType> GetAll() {
            return Enum.GetValues(typeof(DbProviderType))
                .Cast<DbProviderType>()
                .ToList();
        }
    }
}