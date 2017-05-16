using System;
using Xunit;
using Sharp.Data;
using Sharp.Data.Filters;
using Sharp.Data.Schema;

namespace Sharp.Tests.Databases.Data {

    public abstract class DataClientTests : IDisposable {
        protected string TableFoo = "foo";
        private IDataClient _dataClient;

        protected IDataClient DataClient => _dataClient ?? (_dataClient = DBBuilder.GetDataClient(GetDataProviderName()));

        protected abstract string GetDataProviderName();

        protected DataClientTests() {
            CleanTables();
        }

		[Fact]
		public void Should_return_false_if_table_doesnt_exist() {
			Assert.False(DataClient.TableExists("foo"));
		}

		[Fact]
		public void Should_return_true_if_table_exists() {
			CreateTableFoo();
			Assert.True(DataClient.TableExists("foo"));
		}

        protected void CreateTableFoo() {
            DataClient.AddTable(TableFoo,
                                 Column.Int32("id").AsPrimaryKey(),
                                 Column.String("name")
            );
        }

        protected void PopulateTableFoo() {
            DataClient.Insert.Into(TableFoo).Columns("id", "name")
                .Values(1, "v1")
                .Values(2, "v2")
                .Values(Int32.MaxValue, "v3");
        }

        protected void CreateTableBar() {
            DataClient.AddTable("bar",
                                 Column.Int32("id").AsPrimaryKey(),
                                 Column.Int32("id_foo1").NotNull(),
                                 Column.Int32("id_foo2").NotNull(),
                                 Column.String("name")
                );
        }


        protected void DropTable(string tableName) {
            try {
                DataClient.RemoveTable(tableName);
            }
            catch {}
        }

        public void CleanTables() {
            DropTable(TableFoo);
            DropTable("bar");
            DropTable("foobar");
            DropTable("footable");
        }

        public void Dispose() {
            CleanTables();
            DataClient.RollBack();
            DataClient.Dispose();
        }
    }
}