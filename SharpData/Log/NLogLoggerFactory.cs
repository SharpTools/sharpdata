using System;
using System.Reflection;

namespace SharpData.Log {
    public class NLogLoggerFactory : ISharpLoggerFactory {
        private static readonly Type LogManagerType = Type.GetType("NLog.LogManager, NLog");
        private static readonly Func<string, object> GetLoggerByNameDelegate;

        static NLogLoggerFactory() {
            var method = LogManagerType.GetTypeInfo().GetDeclaredMethod("GetLogger");
            GetLoggerByNameDelegate = name => method.Invoke(null, new [] { name });
        }

        public ISharpLogger LoggerFor(string keyName) {
            return new NLogLogger(GetLoggerByNameDelegate(keyName));
        }

        public ISharpLogger LoggerFor(Type type) {
            return new NLogLogger(GetLoggerByNameDelegate(type.FullName));
        }
    }
}