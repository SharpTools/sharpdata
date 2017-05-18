using System;

namespace SharpData.Log {
    public interface ISharpLoggerFactory {
        ISharpLogger LoggerFor(string keyName);
        ISharpLogger LoggerFor(Type type);
    }
}