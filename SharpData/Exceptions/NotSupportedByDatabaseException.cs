using System;

namespace SharpData.Exceptions {
    public class NotSupportedByDatabaseException : DatabaseException {
        public NotSupportedByDatabaseException(string message, Exception innerException, string sql) : base(message, innerException, sql) {
        }
    }
}