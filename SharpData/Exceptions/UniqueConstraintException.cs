using System;

namespace SharpData.Exceptions {
    public class UniqueConstraintException : DatabaseException {
        public UniqueConstraintException(string message, Exception innerException, string sql)
            : base(message, innerException, sql) {
        }
    }
}