using System;

namespace SharpData.Exceptions {
    public class ProviderNotFoundException : Exception {
        public ProviderNotFoundException(string message) : base(message) { }
    }
}