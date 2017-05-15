using System;
using System.Collections.Generic;
using System.Data;

namespace Sharp.Tests.Data {
    public class DataParameterCollectionFake : List<object>, IDataParameterCollection {

        public void RemoveAt(string parameterName) {

        }

        public object this[string parameterName] {
            get => this[parameterName];
            set => this[parameterName] = value;
        }

        public bool Contains(string parameterName) {
            throw new NotImplementedException();
        }

        public int IndexOf(string parameterName) {
            throw new NotImplementedException();
        }
    }
}