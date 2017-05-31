using System;

namespace SharpData.Exceptions {
    
    public class DataTypeNotAvailableException : Exception {
        public DataTypeNotAvailableException(string message) : base(message) { }
    }
}
