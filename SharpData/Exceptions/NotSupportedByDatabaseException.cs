using System;

namespace Sharp.Data.Exceptions {
    public class NotSupportedByDatabaseException : DatabaseException {
        public NotSupportedByDatabaseException(string message, Exception innerException, string sql) : base(message, innerException, sql) {
        }
    }
}