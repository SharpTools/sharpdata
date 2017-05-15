using System;
using Xunit;
using Sharp.Data;
using Sharp.Data.Filters;
using Sharp.Data.Schema;

namespace Sharp.Tests.Databases.Data {

    public abstract class DataClientTests : IDisposable {
        protected IDataClient _dataClient;
        protected string tableFoo = "foo";       
       

		[Fact]
		public void Should_return_false_if_table_doesnt_exist() {
			Assert.False(_dataClient.TableExists("foo"));
		}

		[Fact]
		public void Should_return_true_if_table_exists() {
			CreateTableFoo();
			Assert.True(_dataClient.TableExists("foo"));
		}

        protected void CreateTableFoo() {
            _dataClient.AddTable(tableFoo,
                                 Column.Int32("id").AsPrimaryKey(),
                                 Column.String("name")
                );
        }

        protected void PopulateTableFoo() {
            _dataClient.Insert.Into(tableFoo).Columns("id", "name")
                .Values(1, "v1")
                .Values(2, "v2")
                .Values(3, "v3");
        }

        protected void CreateTableBar() {
            _dataClient.AddTable("bar",
                                 Column.Int32("id").AsPrimaryKey(),
                                 Column.Int32("id_foo1").NotNull(),
                                 Column.Int32("id_foo2").NotNull(),
                                 Column.String("name")
                );
        }


        protected void DropTable(string tableName) {
            try {
                _dataClient.RemoveTable(tableName);
            }
            catch {}
        }

        public void Dispose() {
            DropTable(tableFoo);
            DropTable("bar");
            DropTable("foobar");
            DropTable("footable");
            _dataClient.RollBack();
            _dataClient.Dispose();
        }
    }
}