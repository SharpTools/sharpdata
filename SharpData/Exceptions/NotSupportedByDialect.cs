using System;
using Microsoft.Extensions.Logging;
using SharpData.Log;

namespace SharpData.Exceptions {
    public class NotSupportedByDialectException : Exception {

        private static ILogger Logger { get; } = SharpDataLogging.CreateLogger<NotSupportedByDialectException>();

        public string FunctionName { get; set; }
        public string DialectName { get; set; }

        public NotSupportedByDialectException(string message, string functionName, string dialectName) : base(message) {
            FunctionName = functionName;
            DialectName = dialectName;
            Logger.LogError($"Dataclient error: operation {functionName} not supported by {dialectName}");
        }
    }
}