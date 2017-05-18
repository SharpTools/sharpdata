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
                    return "System.Data.SQLite";
                case DbProviderType.OleDb:
                    return "System.Data.OleDb";
                case DbProviderType.PostgreSql:
                    return "Npgsql";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static DbProviderType GetDbProviderByNamespace(string name) {
            if (name == DbProviderType.OracleManaged.GetProviderName()) return DbProviderType.OracleManaged;
            if (name == DbProviderType.OracleOdp.GetProviderName()) return DbProviderType.OracleOdp;
            if (name == DbProviderType.MySql.GetProviderName()) return DbProviderType.MySql;
            if (name == DbProviderType.SqlServer.GetProviderName()) return DbProviderType.SqlServer;
            if (name == DbProviderType.SqLite.GetProviderName()) return DbProviderType.SqLite;
            if (name == DbProviderType.OleDb.GetProviderName()) return DbProviderType.OleDb;
            if (name == DbProviderType.PostgreSql.GetProviderName()) return DbProviderType.PostgreSql;
            throw new ArgumentOutOfRangeException(nameof(name), name, null);
        }

        public static List<DbProviderType> GetAll() {
            return Enum.GetValues(typeof(DbProviderType))
                .Cast<DbProviderType>()
                .ToList();
        }
    }
}