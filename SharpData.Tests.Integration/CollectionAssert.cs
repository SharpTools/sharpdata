using System.Collections;
using Xunit;

namespace SharpData.Tests.Integration {
    public static class CollectionAssert {
        public static void AreEqual(ICollection expected, ICollection actual) {
            Assert.True(expected.Count == actual.Count, "Collections are not the same size");
            var actualEnumerator = actual.GetEnumerator();

            foreach (var item in expected) {
                actualEnumerator.MoveNext();
                var other = actualEnumerator.Current;
                Assert.True(item.Equals(other));
            }
        }
    }
}