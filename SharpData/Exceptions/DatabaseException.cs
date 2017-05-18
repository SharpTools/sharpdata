using System;

namespace SharpData {
    public class DatabaseException : Exception {

        public string SQL { get; set; }

        public DatabaseException(string message, Exception innerException, string sql)
            : base(message, innerException) {
            SQL = sql;
        }

        public override string ToString() {
            return $"Error running SQL: {SQL}\r\n{base.ToString()}";
        }
    }
}