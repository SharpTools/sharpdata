using Microsoft.Extensions.Logging;

namespace SharpData.Log {
    public static class SharpDataLogging {
        public static ILoggerFactory LoggerFactory { get; set; } = new LoggerFactory();
        public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
        public static ILogger CreateLogger(string category) => LoggerFactory.CreateLogger(category);
    }
}