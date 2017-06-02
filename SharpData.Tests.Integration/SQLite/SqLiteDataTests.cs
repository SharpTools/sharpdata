using System.Data.SQLite;
using System.IO;
using SharpData.Databases;
using SharpData.Exceptions;
using SharpData.Tests.Integration.Data;
using Xunit;

namespace SharpData.Tests.Integration.SQLite {
    public class SqLiteDataTests : DataClientDataTests {
        public SqLiteDataTests() {
            var fileName = "hot.db3";
            if (File.Exists(fileName)) {
                File.Delete(fileName);
            }
            SQLiteConnection.CreateFile(fileName);
        }

        protected override DbProviderType GetDataProviderName() {
            return DbProviderType.SqLite;
        }

        [Fact]
        public override void Can_insert_returning_id() {
            var ex = Assert.Throws<NotSupportedByDialectException>(() => { base.Can_insert_returning_id(); });
            Assert.Equal(ex.DialectName, "SqLiteDialect");
            Assert.Equal(ex.FunctionName, "GetInsertReturningColumnSql");
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_order_by_with_filter_and_pagination() {
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_select_with_pagination() {
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_select_with_pagination_and_where_filter() {
        }
    }
}