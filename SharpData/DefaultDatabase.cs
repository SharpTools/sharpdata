using System;
using System.Data;
using System.Text;
using Sharp.Data.Log;
using System.Data.Common;

namespace Sharp.Data {
    public class DefaultDatabase {
        private static readonly ISharpLogger Log = LogManager.GetLogger("Sharp.Data.Database");

        public IDataProvider Provider { get; protected set; }
        public string ConnectionString { get; protected set; }
        public int Timeout { get; set; }
        
        protected IDbConnection Connection;
        protected IDbTransaction Transaction;

        public DefaultDatabase(IDataProvider provider, string connectionString) {
            Provider = provider;
            ConnectionString = connectionString;
            LogDatabaseProviderName(provider.ToString());
        }

        protected static void LogDatabaseProviderName(string providerName) {
            Log.Debug("Provider: " + providerName);
        }

        protected void RetrieveOutParameters(object[] parameters, IDbCommand cmd) {
            if (parameters == null) {
                return;
            }
            foreach (object parameter in parameters) {
                var pout = parameter as Out;
                if (pout != null) {
                    pout.Value = ((DbParameter) cmd.Parameters[pout.Name]).Value;
                    continue;
                }
                if (parameter is InOut pinout) {
                    pinout.Value = ((DbParameter)cmd.Parameters[pinout.Name]).Value;
                }
            }
        }

        protected IDataReader TryCreateReader(string call, object[] parameters, CommandType commandType) {
            var cmd = CreateCommand(call, parameters);
            cmd.CommandType = commandType;
            return cmd.ExecuteReader();
        }

        protected object TryQueryReader(string call, object[] parameters) {
            var cmd = CreateCommand(call, parameters);
            var obj = cmd.ExecuteScalar();
            return obj;
        }

        protected void TryExecuteStoredProcedure(string call, object[] parameters, bool isBulk = false) {
            var cmd = CreateCommand(call, parameters, isBulk);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();
            RetrieveOutParameters(parameters, cmd);
        }

        protected object TryCallStoredFunction(DbType returnType, string call, object[] parameters) {
            var returnPar = GetReturnParameter(returnType);

            var cmd = CreateCommand(call, parameters);
            cmd.Parameters.Insert(0, returnPar);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.ExecuteNonQuery();

            object returnObject = returnPar.Value;
            return returnObject;
        }
        
        protected void SetTimeoutForCommand(IDbCommand cmd) {
            //Doesn't work for oracle!
            if (Timeout >= 0) {
                cmd.CommandTimeout = Timeout;
            }
        }

        protected IDbCommand CreateIDbCommand(string call, object[] parameters) {
            var cmd = Connection.CreateCommand();
            cmd.CommandText = call;
            cmd.Transaction = Transaction;
            return cmd;
        }

        protected static void LogCommandCall(string call, IDbCommand cmd) {
            if (Log.IsDebugEnabled) {
                var sb = new StringBuilder();
                sb.Append("Call: ").AppendLine(call);
                foreach (DbParameter p in cmd.Parameters) {
                    sb.Append(p.Direction).Append("-> ").Append(p.ParameterName);
                    if (p.Value != null) {
                        sb.Append(": ").Append(p.Value);
                    }
                    sb.AppendLine();
                }
                Log.Debug(sb.ToString());
            }
        }

        protected void PopulateCommandParameters(IDbCommand cmd, object[] parameters, bool isBulk) {
            if (parameters == null) {
                return;
            }
            foreach (var parameter in parameters) {
                DbParameter par;
                if (parameter is Out) {
                    par = GetOutParameter((Out) parameter);
                }
                else if (parameter is InOut) {
                    par = GetInOutParameter((InOut) parameter);
                }
                else if (parameter is In) {
                    par = GetInParameter((In) parameter, cmd, isBulk);
                }
                else {
                    par = GetInParameter(new In { Value = parameter }, cmd, isBulk);
                }
                //this is for when you have the cursor parameter, ignored by sql server
                if (par != null) {
                    cmd.Parameters.Add(par);
                }
            }
        }

        protected DbParameter GetInParameter(In p, IDbCommand cmd, bool isBulk) {
            var par = Provider.GetParameter(p, isBulk);
            par.Direction = ParameterDirection.Input;
            par.Value = p.Value ?? DBNull.Value;
            par.ParameterName = p.Name;
            return par;
        }

        protected DbParameter GetOutParameter(Out outParameter) {
            DbParameter par;
            par = outParameter.IsCursor ? 
                Provider.GetParameterCursor() : 
                Provider.GetParameter();
            //this "if != null" is for the cursor parameter, ignored by sql server
            if (par != null) {
                par.Direction = ParameterDirection.Output;
                par.ParameterName = outParameter.Name;
                par.Size = outParameter.Size;
                par.Value = outParameter.Value;
                par.DbType = outParameter.Type;
            }
            return par;
        }

        protected DbParameter GetInOutParameter(InOut p) {
            var par = Provider.GetParameter();

            par.Direction = ParameterDirection.InputOutput;
            par.ParameterName = p.Name;
            par.Size = p.Size;
            par.Value = p.Value ?? DBNull.Value;
            par.DbType = p.Type;
            return par;
        }

        protected DbParameter GetReturnParameter(DbType type) {
            var par = Provider.GetParameter(null, false);
            par.Direction = ParameterDirection.ReturnValue;
            par.Size = 4000;
            par.DbType = type;
            return par;
        }

        protected void CloseTransaction() {
            if (Transaction == null) {
                return;
            }
            try {
                Transaction.Dispose();
            }
            catch { }
            Transaction = null;
        }

        protected void CloseConnection() {
            if (Connection == null) {
                return;
            }
            try {
                Connection.Close();
                Connection.Dispose();
                Log.Debug("Connection closed");
            }
            catch { }
            Connection = null;
        }

        protected void CommitTransaction() {
            if (Transaction == null) {
                return;
            }
            Transaction.Commit();
            Log.Debug("Commit");
        }

        protected void RollBackTransaction() {
            if (Transaction == null) {
                return;
            }
            Transaction.Rollback();
            Log.Debug("Rollback");
        }

        protected int TryExecuteSql(string call, object[] parameters, bool isBulk = false) {
            var cmd = CreateCommand(call, parameters, isBulk);
            var modifiedRows = cmd.ExecuteNonQuery();
            RetrieveOutParameters(parameters, cmd);
            return modifiedRows;
        }

        protected IDbCommand CreateCommand(string call, object[] parameters, bool isBulk = false) {
            OpenConnection();
            var cmd = CreateIDbCommand(call, parameters);
            Provider.ConfigCommand(cmd, parameters, isBulk);
            SetTimeoutForCommand(cmd);
            PopulateCommandParameters(cmd, parameters, isBulk);
            LogCommandCall(call, cmd);
            return cmd;
        }

        protected void OpenConnection() {
            if (Connection != null) {
                return;
            }
            Connection = Provider.GetConnection();
            Connection.ConnectionString = ConnectionString;
            Connection.Open();
            Transaction = Connection.BeginTransaction();

            Log.Debug("Connection open");
        }

        protected ResultSet ExecuteCatchingErrors(Func<IDataReader> getReader, string call) {
            IDataReader reader = null;
            try {
                ExecutePreCommand();
                reader = getReader();
                return DataReaderToResultSetMapper.Map(reader);
            }
            catch (Exception ex) {
                ExecuteOnErrorCommand();
                throw Provider.CreateSpecificException(ex, call);
            }
            finally {
                reader?.Dispose();
            }
        }

        protected T ExecuteCatchingErrors<T>(Func<T> action, string call) {
            try {
                ExecutePreCommand();
                return action();
            }
            catch (Exception ex) {
                ExecuteOnErrorCommand();
                throw Provider.CreateSpecificException(ex, call);
            }
        }

        protected void ExecuteCatchingErrors(Action action, string call) {
            try {
                ExecutePreCommand();
                action();
            }
            catch (Exception ex) {
                ExecuteOnErrorCommand();
                throw Provider.CreateSpecificException(ex, call);
            }
        }

        protected void ExecutePreCommand() {
            if (!String.IsNullOrEmpty(Provider.GetPreCommand())) {
                TryExecuteSql(Provider.GetPreCommand(), new object[] { });
            }
        }

        protected void ExecuteOnErrorCommand() {
            if (!String.IsNullOrEmpty(Provider.GetOnErrorCommand())) {
                TryExecuteSql(Provider.GetOnErrorCommand(), new object[] { });
            }
        }
    }
}