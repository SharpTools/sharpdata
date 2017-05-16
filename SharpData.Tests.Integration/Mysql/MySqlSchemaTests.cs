using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;
using Xunit;

namespace Sharp.Tests.Databases.Mysql {
    public class MySqlSchemaTests : DataClientSchemaTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.MySql;
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