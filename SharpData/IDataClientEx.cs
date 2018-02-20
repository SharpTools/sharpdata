using System;
using System.Collections.Generic;

namespace SharpData {
    public static class IDataClientEx {
        public static int ExecSqlFormattable(this IDataClient client, FormattableString formattable) {
            var (Sql, Pars) = Prepare(client, formattable);
            return client.Database.ExecuteSql(Sql, Pars);
        }

        public static int ExecSqlFormattableCommitAndDispose(this IDataClient client, FormattableString formattable) {
            var (Sql, Pars) = Prepare(client, formattable);
            return client.Database.ExecuteSqlCommitAndDispose(Sql, Pars);
        }

        public static ResultSet QueryFormattable(this IDataClient client, FormattableString formattable) {
            var (Sql, Pars) = Prepare(client, formattable);
            return client.Database.Query(Sql, Pars);
        }

        public static ResultSet QueryFormattableAndDispose(this IDataClient client, FormattableString formattable) {
            var (Sql, Pars) = Prepare(client, formattable);
            return client.Database.QueryAndDispose(Sql, Pars);
        }

        public static List<T> QueryFormattable<T>(this IDataClient client, FormattableString formattable) where T : new() {
            var (Sql, Pars) = Prepare(client, formattable);
            return client.Database.Query(Sql, Pars).Map<T>();
        }

        public static List<T> QueryFormattableAndDispose<T>(this IDataClient client, FormattableString formattable) where T : new() {
            var (Sql, Pars) = Prepare(client, formattable);
            return client.Database.QueryAndDispose(Sql, Pars).Map<T>();
        }

        private static (string Sql, In[] Pars) Prepare(IDataClient client, FormattableString formattable) {
            var sql = formattable.Format;
            var prefix = client.Dialect.ParameterPrefix;
            var pars = new List<In>();
            for (var i = 0; i < formattable.ArgumentCount; i++) {
                var parName = prefix + "par" + i;
                sql = sql.Replace("{" + i + "}", parName);
                pars.Add(In.Named(parName, formattable.GetArgument(i)));
            }
            return (sql, pars.ToArray());
        }
    }
}