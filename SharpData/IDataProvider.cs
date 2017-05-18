using System;
using System.Data;
using SharpData.Databases;
using System.Data.Common;

namespace SharpData {
    public interface IDataProvider {
        DbProviderType Name { get; }
        DatabaseKind DatabaseKind { get; }
        IDbConnection GetConnection();
        void ConfigCommand(IDbCommand command, object[] parameters, bool isBulk);
        DbParameter GetParameter();
        DbParameter GetParameter(In parameter, bool isBulk);
        DbParameter GetParameterCursor();
        DatabaseException CreateSpecificException(Exception exception, string sql);
        string GetPreCommand();
        string GetOnErrorCommand();
    }
}