using System.Data.SQLite;
using System.IO;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;
using Xunit;

namespace Sharp.Tests.Databases.SQLite {
    public class SqLiteDataTests : DataClientDataTests {
        public SqLiteDataTests() {
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
        public override void Can_insert_returning_id() {
            var ex = Assert.Throws<NotSupportedByDialect>(() => { base.Can_insert_returning_id(); });
            Assert.Equal(ex.DialectName, "SqLiteDialect");
            Assert.Equal(ex.FunctionName, "GetInsertReturningColumnSql");
        }
    }
}