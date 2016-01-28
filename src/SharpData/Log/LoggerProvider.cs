using System;
using System.IO;
using System.Reflection;

namespace Sharp.Data.Log {
    public class LogManager {
        private ISharpLoggerFactory _loggerFactory;
        private static LogManager _instance;

        public void UseLoadLog4Net() {
            SetLogAssembly(typeof(Log4NetLoggerFactory));
        }

        public void UseNLog() {
            SetLogAssembly(typeof(NLogLoggerFactory));
        }

        private void SetLogAssembly(Type type) {
            var qualifiedName = type.AssemblyQualifiedName;
            var loggerFactory = String.IsNullOrEmpty(qualifiedName) ? 
                new NoLoggingLoggerFactory() : 
                GetLoggerFactory(qualifiedName);
            SetLoggersFactory(loggerFactory);
        }


        private static ISharpLoggerFactory GetLoggerFactory(string loggerClass) {
            ISharpLoggerFactory loggerFactory;
            var loggerFactoryType = Type.GetType(loggerClass);
            try {
                loggerFactory = (ISharpLoggerFactory)Activator.CreateInstance(loggerFactoryType);
            }
            catch (MissingMethodException ex) {
                throw new Exception("Public constructor was not found for " + loggerFactoryType, ex);
            }
            catch (InvalidCastException ex) {
                throw new Exception(loggerFactoryType + "Type does not implement " + typeof(ISharpLoggerFactory), ex);
            }
            catch (Exception ex) {
                throw new Exception("Unable to instantiate: " + loggerFactoryType, ex);
            }
            return loggerFactory;
        }

        public static void SetLoggersFactory(ISharpLoggerFactory loggerFactory) {
            _instance = new LogManager(loggerFactory);
        }

        private LogManager(ISharpLoggerFactory loggerFactory) {
            _loggerFactory = loggerFactory;
        }

        public static ISharpLogger GetLogger(string keyName) {
            return _instance._loggerFactory.LoggerFor(keyName);
        }

        public static ISharpLogger GetLogger(Type type) {
            return _instance._loggerFactory.LoggerFor(type);
        }
    }
}