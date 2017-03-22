using System;
using Sharp.Data.Log;

namespace Sharp.Data {
    
    public class DataTypeNotAvailableException : Exception {
        public DataTypeNotAvailableException(string message) : base(message) { }
    }

    public class NotSupportedByDialect : Exception {

        private static readonly ISharpLogger Log = LogManager.GetLogger(typeof(NotSupportedByDialect));

        public string FunctionName { get; set; }
        public string DialectName { get; set; }

        public NotSupportedByDialect(string message, string functionName, string dialectName) : base(message) {
            FunctionName = functionName;
            DialectName = dialectName;
            Log.Error(String.Format("Dataclient error: operation {0} not supported by {1}", functionName, dialectName));
        }
    }

    public class ProviderNotFoundException : Exception {
        public ProviderNotFoundException(string message) : base(message) { }
    }
}
