using System;
using System.Data;
using System.Data.Common;

namespace Sharp.Data.Databases {
    public abstract class DataProvider : IDataProvider {
        protected DbProviderFactory DbProviderFactory { get; private set; }
        public abstract string Name { get; }
        public abstract DatabaseKind DatabaseKind { get; }

        protected DataProvider(DbProviderFactory dbProviderFactory) {
            DbProviderFactory = dbProviderFactory;
        }

        public virtual DbConnection GetConnection() {
            return DbProviderFactory.CreateConnection();
        }

        public virtual void ConfigCommand(DbCommand command, object[] parameters, bool isBulk) { }

        public DbParameter GetParameter() {
            return DbProviderFactory.CreateParameter();
        }

        public virtual DbParameter GetParameter(In parameter, bool isBulk) {
            return GetParameter();
        }

        public virtual DbParameter GetParameterCursor() {
            return DbProviderFactory.CreateParameter();
        }

        public virtual DatabaseException CreateSpecificException(Exception exception, string sql) {
            return new DatabaseException(exception.Message, exception, sql);
        }

        public virtual string CommandToBeExecutedBeforeEachOther() {
            return null;
        }

        public virtual string CommandToBeExecutedAfterAnExceptionIsRaised() {
            return null;
        }
    }
}