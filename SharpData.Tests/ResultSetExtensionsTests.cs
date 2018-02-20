using System;
using System.Linq;
using Xunit;
using SharpData;

namespace Sharp.Tests.Data {
    public class ResultSetExtensionsTests {
        [Fact]
        public void Should_map_object() {
            var res = CreateResultSet();
            var list = res.Map<SomeClass>();

            Assert.Equal(1, list[0].Int);
            Assert.Equal("String", list[0].String);
            Assert.Equal(DateTime.Today, list[0].DateTime);
            Assert.True(list[0].Double - 1.1 < Double.Epsilon);

            Assert.Equal(2, list[1].Int);
            Assert.Equal("String2", list[1].String);
            Assert.Equal(DateTime.Today, list[1].DateTime);
            Assert.True(list[1].Double - 1.2 < Double.Epsilon);
        }

        [Fact]
        public void Should_map_object_with_extra_properties() {
            var res = CreateResultSet();
            var list = res.Map<ExtraProperties>();

            Assert.Equal(1, list[0].Int);
            Assert.Equal("String", list[0].String);
            Assert.Equal(DateTime.Today, list[0].DateTime);
            Assert.True(list[0].Double - 1.1 < Double.Epsilon);
        }


        [Fact]
        public void Should_map_object_with_missing_properties() {
            var res = CreateResultSet();
            var list = res.Map<MissingProperties>();
            Assert.Equal(1, list[0].Int);
        }

        [Fact]
        public void Should_map_object_with_different_type() {
            var res = CreateResultSet();
            var list = res.Map<WrongType>();
            Assert.Equal("1", list[0].Int);
        }

        [Fact]
        public void Should_map_private_property() {
            var res = CreateResultSet();
            var list = res.Map<PrivateSet>();
            Assert.Equal(1, list[0].Int);
        }

        [Fact]
        public void Should_map_nullable_prop() {
            var res = CreateResultSet();
            var list = res.Map<NullableProp>();
            Assert.Equal(1, list[0].Int);
        }

        [Fact]
        public void Should_map_null_value_to_nullable_prop() {
            var res = CreateResultSet();
            res.AddRow(null, "String", DateTime.Today, 1.1);
            var list = res.Map<NullableProp>();
            Assert.Null(list.Last().Int);
        }

        [Fact]
        public void Should_map_null_value() {
            var res = CreateResultSet();
            res.AddRow(1, null, DateTime.Today, 1.1);
            var list = res.Map<SomeClass>();
            Assert.Null(list.Last().String);
        }

        [Fact]
        public void Should_int_to_enum() {
            var res = CreateResultSet();
            res.AddRow(1, null, DateTime.Today, 1.1);
            var list = res.Map<WithEnum>();
            Assert.Equal(SuperEnum.Bar, list[0].Int);
        }

        private static ResultSet CreateResultSet() {
            var res = new ResultSet("Int", "String", "DateTime", "Double");
            res.AddRow(1, "String", DateTime.Today, 1.1);
            res.AddRow(2, "String2", DateTime.Today, 1.2);
            return res;
        }

        private class SomeClass {
            public int Int { get; set; }
            public string String { get; set; }
            public DateTime DateTime { get; set; }
            public double Double { get; set; }
        }

        private class ExtraProperties {
            public int Int { get; set; }
            public string String { get; set; }
            public DateTime DateTime { get; set; }
            public double Double { get; set; }
            public string String2 { get; set; }
        }

        private class NullableProp {
            public int? Int { get; set; }
        }

        private class MissingProperties {
            public int Int { get; set; }
        }

        private class PrivateSet {
            public int Int { get; private set; }
        }

        private class WrongType {
            public string Int { get; private set; }
        }

        private class WithEnum {
            public SuperEnum Int { get; set; }
        }

        private enum SuperEnum {
            Foo = 0,
            Bar = 1,
            FooBar = 2
        }
    }
}