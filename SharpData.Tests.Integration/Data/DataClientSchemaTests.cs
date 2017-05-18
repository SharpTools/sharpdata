using System;
using SharpData.Schema;
using Xunit;

namespace SharpData.Tests.Integration.Data {

	public abstract class DataClientSchemaTests : DataClientTests {

		[Fact]
		public virtual void Can_create_table() {
			DataClient.AddTable(TableFoo,
								 Column.Binary("col_binary"),
								 Column.Boolean("col_boolean"),
								 Column.Date("col_data"),
								 Column.Decimal("col_decimal"),
								 Column.Double("col_double"),
								 Column.Guid("col_guid"),
								 Column.Int16("col_int16"),
								 Column.Int32("col_int32"),
								 Column.Int64("col_int64"),
								 Column.Single("col_single"),
								 Column.String("col_string"),
                                 Column.Clob("col_clob")
				);
		}

		[Fact]
		public virtual void Can_drop_table() {
			Can_create_table();
			DataClient.RemoveTable(TableFoo);
		}

		[Fact]
		public virtual void Can_create_table_with_primary_key() {
			CreateTableFoo();
		}

		[Fact]
		public virtual void Can_create_table_with_multiple_columns_as_primary_key() {
            DataClient.AddTable(TableFoo,
								 Column.Int32("id").AsPrimaryKey(),
								 Column.Int32("id2").AsPrimaryKey(),
								 Column.String("name")
				);
		}

		[Fact]
		public virtual void Can_create_table_with_column_options() {
            DataClient.AddTable(TableFoo,
								 Column.Int32("id").NotNull().DefaultValue(1),
								 Column.Int32("id2").AsPrimaryKey(),
								 Column.String("name").NotNull().DefaultValue("foo")
				);
		}

		[Fact]
		public virtual void Can_create_table_with_autoIncrement() {
            DataClient.AddTable(TableFoo,
								 Column.AutoIncrement("id"),
								 Column.String("name"));
		}

		[Fact]
		public virtual void Can_create_table_with_autoIncrement_as_primary_key() {
            DataClient.AddTable(TableFoo,
								 Column.AutoIncrement("id").AsPrimaryKey(),
								 Column.String("name"));
		}

		[Fact]
		public virtual void Can_add_primary_key_to_table() {
            DataClient.AddTable(TableFoo,
								 Column.Int32("id").NotNull(),
								 Column.String("name")
				);
			DataClient.AddPrimaryKey(TableFoo, "id");
		}

        [Fact]
        public virtual void Can_remove_primary_key_from_table() {
            DataClient.AddTable(TableFoo,
                                 Column.Int32("id").NotNull(),
                                 Column.String("name")
                );
            DataClient.AddNamedPrimaryKey(TableFoo, "pk1", "id");
            DataClient.RemovePrimaryKey(TableFoo, "pk1");
        }

		[Fact]
		public virtual void Can_add_named_primary_key_to_table() {
            DataClient.AddTable(TableFoo,
								 Column.Int32("id").NotNull(),
								 Column.String("name")
				);
			DataClient.AddNamedPrimaryKey(TableFoo, "pk_" + TableFoo, "id");
		}

		[Fact]
		public virtual void Can_add_column_to_table() {
			CreateTableFoo();
			DataClient.AddColumn(TableFoo, Column.String("bar", 100).NotNull().DefaultValue("foobar").Object);
		}

		[Fact]
		public virtual void Can_add_boolean_column_to_table_with_default_value() {
			CreateTableFoo();
			DataClient.AddColumn(TableFoo, Column.Boolean("bar").NotNull().DefaultValue(true).Object);
		}

		[Fact]
		public virtual void Can_remove_column_from_table() {
			CreateTableFoo();
			DataClient.RemoveColumn(TableFoo, "name");
		}

        [Fact]
        public virtual void Can_remove_column_from_table_with_default_value() {
            CreateTableFoo();
            DataClient.AddColumn(TableFoo, Column.Int32("bar").NotNull().DefaultValue(0).Object);
            DataClient.RemoveColumn(TableFoo, "bar");
        }

		[Fact]
		public virtual void Can_add_foreign_key_to_table() {
			CreateTableFoo();
			CreateTableBar();

			DataClient.AddTable("foobar",
								 Column.Int32("id").AsPrimaryKey(),
								 Column.Int32("id_bar")
				);

			DataClient.AddForeignKey("fk_foo_bar1", "bar", "id_foo1", "foo", "id", OnDelete.Cascade);
			DataClient.AddForeignKey("fk_foo_bar2", "bar", "id_foo2", "foo", "id", OnDelete.NoAction);
			DataClient.AddForeignKey("fk_foobar_bar", "foobar", "id_bar", "bar", "id", OnDelete.SetNull);
		}

		[Fact]
		public virtual void Can_remove_foreign_key_from_table() {
			CreateTableFoo();
			CreateTableBar();

			DataClient.AddForeignKey("fk_foo_bar1", "bar", "id_foo1", "foo", "id", OnDelete.Cascade);

			DataClient.RemoveForeignKey("fk_foo_bar1", "bar");
		}

		[Fact]
		public virtual void Can_add_unique_constraint() {
			CreateTableFoo();
			DataClient.AddUniqueKey("un_foo_name", "foo", "name");
		}

		[Fact]
		public virtual void Can_remove_unique_constraint() {
			CreateTableFoo();
			DataClient.AddUniqueKey("un_foo_name", "foo", "name");
			DataClient.RemoveUniqueKey("un_foo_name", "foo");
		}

		[Fact]
		public void Can_add_index_to_table() {
			CreateTableFoo();
			DataClient.AddIndex("in_foo", "foo", "name");
		}

		[Fact]
		public void Can_add_index_with_multiple_coluns_to_table() {
			CreateTableFoo();
			DataClient.AddIndex("in_foo", "foo", "id", "name");
			DataClient.RemoveIndex("in_foo", "foo");
		}

		[Fact]
		public void Can_remove_index_from_table() {
			CreateTableFoo();
			DataClient.AddIndex("in_foo", "foo", "name");
			DataClient.RemoveIndex("in_foo", "foo");
		}

        [Fact]
        public virtual void Can_add_comment_to_table() {
            CreateTableFoo();
            DataClient.Add.Comment("foo").ToTable("foo");
            DataClient.Remove.Comment.FromTable("foo");
        }

        [Fact]
        public virtual void Can_add_comment_to_column() {
            CreateTableFoo();
            DataClient.Add.Comment("foo").ToColumn("name").OfTable("foo");
            DataClient.Remove.Comment.FromColumn("name").OfTable("foo");
        }

        [Fact]
        public virtual void Can_rename_table() {
            CreateTableFoo();
            DataClient.Rename.Table("foo").To("foo2");
            DataClient.TableExists("foo2");
            DataClient.Remove.Table("foo2");
        }

        [Fact]
        public virtual void Can_rename_column() {
            CreateTableFoo();
            DataClient.Rename.Column("name").OfTable("foo").To("name2");
        }

	    [Fact]
	    public virtual void Can_modify_column() {
            CreateTableFoo();
	        DataClient.Modify
	                   .Column("name")
	                   .OfTable("foo")
	                   .WithDefinition(Column.String("name").NotNull());

            try {
                DataClient.Insert.Into("foo").Columns("name").Values(DBNull.Value);
                Assert.True(false, "Should not insert in a non null column");
            }
            catch {}
	    }
	}
}