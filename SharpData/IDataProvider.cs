using System;
using System.Data;
using Sharp.Data.Databases;
using System.Data.Common;

namespace Sharp.Data {
    public interface IDataProvider {
        string Name { get; }
        DatabaseKind DatabaseKind { get; }
        IDbConnection GetConnection();
        void ConfigCommand(IDbCommand command, object[] parameters, bool isBulk);
        DbParameter GetParameter();
        DbParameter GetParameter(In parameter, bool isBulk);
        DbParameter GetParameterCursor();
        DatabaseException CreateSpecificException(Exception exception, string sql);
        string CommandToBeExecutedBeforeEachOther();
        string CommandToBeExecutedAfterAnExceptionIsRaised();
    }
}