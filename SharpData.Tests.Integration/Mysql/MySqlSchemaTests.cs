using Sharp.Data.Databases;
using Sharp.Data.Exceptions;
using Sharp.Data.Schema;
using Sharp.Tests.Databases.Data;
using Xunit;

namespace Sharp.Tests.Databases.Mysql {
    public class MySqlSchemaTests : DataClientSchemaTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.MySql;
        }

        [Fact]
        public override void Can_create_table_with_autoIncrement() {
            Assert.Throws<NotSupportedByDatabaseException>(() => {
                DataClient.AddTable(TableFoo,
                    Column.AutoIncrement("id"),
                    Column.String("name"));
            });
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_add_comment_to_column() {
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_add_comment_to_table() {
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_add_foreign_key_to_table() {
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_add_unique_constraint() {
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_modify_column() {
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_remove_foreign_key_from_table() {
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_remove_unique_constraint() {
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_rename_table() {
            
        }

        [Fact(Skip = "Not implemented. Pull requests welcome :)")]
        public override void Can_rename_column() {
          
        }
    }
}