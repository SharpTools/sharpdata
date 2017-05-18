using System;
using System.Data.Common;
using SharpData.Exceptions;

namespace SharpData.Databases.PostgreSql {
    public class PostgreSqlProvider : DataProvider {
        private const string SavepointId = "PostgreSqlId";

        public PostgreSqlProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) { }
        public override DbProviderType Name => DbProviderType.PostgreSql;
        public override DatabaseKind DatabaseKind => DatabaseKind.PostgreSql;

        public override DatabaseException CreateSpecificException(Exception exception, string sql) {
            if (exception.Message.Contains("42P01")) {
                return new TableNotFoundException(exception.Message, exception, sql);
            }
            if (exception.Message.Contains("23505")) {
                return new UniqueConstraintException(exception.Message, exception, sql);
            }
            return base.CreateSpecificException(exception, sql);
        }

        public override string GetPreCommand() {
            return String.Format("SAVEPOINT {0}", SavepointId);
        }

        public override string GetOnErrorCommand() {
            return String.Format("ROLLBACK TO {0}", SavepointId);
        }
    }
}
