using System.Data.SQLite;
using System.IO;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;
using Xunit;

namespace Sharp.Tests.Databases.SQLite {
    public class SqLiteSchemaTests : DataClientSchemaTests {
        public SqLiteSchemaTests() {
            var fileName = "hot.db3";
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
            SQLiteConnection.CreateFile(fileName);
        }

        protected override string GetDataProviderName() {
            return DataProviderNames.SqLite;
        }

        [Fact]
        public override void Can_add_foreign_key_to_table() {
            try {
                base.Can_add_foreign_key_to_table();
                Assert.True(false);
            }
            catch (NotSupportedByDialect ex) {
                Assert.Equal(ex.DialectName, "SqLiteDialect");
                Assert.Equal(ex.FunctionName, "GetForeignKeySql");
            }
        }

        [Fact]
        public override void Can_add_named_primary_key_to_table() {
            try {
                base.Can_add_named_primary_key_to_table();
                Assert.True(false);
            }
            catch (NotSupportedByDialect ex) {
                Assert.Equal(ex.DialectName, "SqLiteDialect");
                Assert.Equal(ex.FunctionName, "GetPrimaryKeySql");
            }
        }

        [Fact]
        public override void Can_add_primary_key_to_table() {
            try {
                base.Can_add_primary_key_to_table();
                Assert.True(false);
            }
            catch (NotSupportedByDialect ex) {
                Assert.Equal(ex.DialectName, "SqLiteDialect");
                Assert.Equal(ex.FunctionName, "GetPrimaryKeySql");
            }
        }

        [Fact]
        public override void Can_remove_column_from_table() {
            try {
                base.Can_remove_column_from_table();
                Assert.True(false);
            }
            catch (NotSupportedByDialect ex) {
                Assert.Equal(ex.DialectName, "SqLiteDialect");
                Assert.Equal(ex.FunctionName, "GetDropColumnSql");
            }
        }


        [Fact]
        public override void Can_remove_foreign_key_from_table() {
            try {
                DataClient.RemoveForeignKey("foo", "bar");
                Assert.True(false);
            }
            catch (NotSupportedByDialect ex) {
                Assert.Equal(ex.DialectName, "SqLiteDialect");
                Assert.Equal(ex.FunctionName, "GetDropForeignKeySql");
            }
        }
    }
}