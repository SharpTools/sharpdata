using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Xunit;
using Sharp.Data;
using Sharp.Data.Schema;

namespace Sharp.Tests.Databases.Data {

	public abstract class DataClientSchemaTests : DataClientTests {

		[Fact]
		public virtual void Can_create_table() {
			_dataClient.AddTable(tableFoo,
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
			_dataClient.RemoveTable(tableFoo);
		}

		[Fact]
		public virtual void Can_create_table_with_primary_key() {
			CreateTableFoo();
		}

		[Fact]
		public virtual void Can_create_table_with_multiple_columns_as_primary_key() {
            _dataClient.AddTable(tableFoo,
								 Column.Int32("id").AsPrimaryKey(),
								 Column.Int32("id2").AsPrimaryKey(),
								 Column.String("name")
				);
		}

		[Fact]
		public virtual void Can_create_table_with_column_options() {
            _dataClient.AddTable(tableFoo,
								 Column.Int32("id").NotNull().DefaultValue(1),
								 Column.Int32("id2").AsPrimaryKey(),
								 Column.String("name").NotNull().DefaultValue("foo")
				);
		}

		[Fact]
		public virtual void Can_create_table_with_autoIncrement() {
            _dataClient.AddTable(tableFoo,
								 Column.AutoIncrement("id"),
								 Column.String("name"));
		}

		[Fact]
		public virtual void Can_create_table_with_autoIncrement_as_primary_key() {
            _dataClient.AddTable(tableFoo,
								 Column.AutoIncrement("id").AsPrimaryKey(),
								 Column.String("name"));
		}

		[Fact]
		public virtual void Can_add_primary_key_to_table() {
            _dataClient.AddTable(tableFoo,
								 Column.Int32("id").NotNull(),
								 Column.String("name")
				);
			_dataClient.AddPrimaryKey(tableFoo, "id");
		}

        [Fact]
        public virtual void Can_remove_primary_key_from_table() {
            _dataClient.AddTable(tableFoo,
                                 Column.Int32("id").NotNull(),
                                 Column.String("name")
                );
            _dataClient.AddNamedPrimaryKey(tableFoo, "pk1", "id");
            _dataClient.RemovePrimaryKey(tableFoo, "pk1");
        }

		[Fact]
		public virtual void Can_add_named_primary_key_to_table() {
            _dataClient.AddTable(tableFoo,
								 Column.Int32("id").NotNull(),
								 Column.String("name")
				);
			_dataClient.AddNamedPrimaryKey(tableFoo, "pk_" + tableFoo, "id");
		}

		[Fact]
		public virtual void Can_add_column_to_table() {
			CreateTableFoo();
			_dataClient.AddColumn(tableFoo, Column.String("bar", 100).NotNull().DefaultValue("foobar").Object);
		}

		[Fact]
		public virtual void Can_add_boolean_column_to_table_with_default_value() {
			CreateTableFoo();
			_dataClient.AddColumn(tableFoo, Column.Boolean("bar").NotNull().DefaultValue(true).Object);
		}

		[Fact]
		public virtual void Can_remove_column_from_table() {
			CreateTableFoo();
			_dataClient.RemoveColumn(tableFoo, "name");
		}

        [Fact]
        public virtual void Can_remove_column_from_table_with_default_value() {
            CreateTableFoo();
            _dataClient.AddColumn(tableFoo, Column.Int32("bar").NotNull().DefaultValue(0).Object);
            _dataClient.RemoveColumn(tableFoo, "bar");
        }

		[Fact]
		public virtual void Can_add_foreign_key_to_table() {
			CreateTableFoo();
			CreateTableBar();

			_dataClient.AddTable("foobar",
								 Column.Int32("id").AsPrimaryKey(),
								 Column.Int32("id_bar")
				);

			_dataClient.AddForeignKey("fk_foo_bar1", "bar", "id_foo1", "foo", "id", OnDelete.Cascade);
			_dataClient.AddForeignKey("fk_foo_bar2", "bar", "id_foo2", "foo", "id", OnDelete.NoAction);
			_dataClient.AddForeignKey("fk_foobar_bar", "foobar", "id_bar", "bar", "id", OnDelete.SetNull);
		}

		[Fact]
		public virtual void Can_remove_foreign_key_from_table() {
			CreateTableFoo();
			CreateTableBar();

			_dataClient.AddForeignKey("fk_foo_bar1", "bar", "id_foo1", "foo", "id", OnDelete.Cascade);

			_dataClient.RemoveForeignKey("fk_foo_bar1", "bar");
		}

		[Fact]
		public virtual void Can_add_unique_constraint() {
			CreateTableFoo();
			_dataClient.AddUniqueKey("un_foo_name", "foo", "name");
		}

		[Fact]
		public virtual void Can_remove_unique_constraint() {
			CreateTableFoo();
			_dataClient.AddUniqueKey("un_foo_name", "foo", "name");
			_dataClient.RemoveUniqueKey("un_foo_name", "foo");
		}

		[Fact]
		public void Can_add_index_to_table() {
			CreateTableFoo();
			_dataClient.AddIndex("in_foo", "foo", "name");
		}

		[Fact]
		public void Can_add_index_with_multiple_coluns_to_table() {
			CreateTableFoo();
			_dataClient.AddIndex("in_foo", "foo", "id", "name");
			_dataClient.RemoveIndex("in_foo", "foo");
		}

		[Fact]
		public void Can_remove_index_from_table() {
			CreateTableFoo();
			_dataClient.AddIndex("in_foo", "foo", "name");
			_dataClient.RemoveIndex("in_foo", "foo");
		}

        [Fact]
        public virtual void Can_add_comment_to_table() {
            CreateTableFoo();
            _dataClient.Add.Comment("foo").ToTable("foo");
            _dataClient.Remove.Comment.FromTable("foo");
        }

        [Fact]
        public virtual void Can_add_comment_to_column() {
            CreateTableFoo();
            _dataClient.Add.Comment("foo").ToColumn("name").OfTable("foo");
            _dataClient.Remove.Comment.FromColumn("name").OfTable("foo");
        }

        [Fact]
        public virtual void Can_rename_table() {
            CreateTableFoo();
            _dataClient.Rename.Table("foo").To("foo2");
            _dataClient.TableExists("foo2");
            _dataClient.Remove.Table("foo2");
        }

        [Fact]
        public virtual void Can_rename_column() {
            CreateTableFoo();
            _dataClient.Rename.Column("name").OfTable("foo").To("name2");
        }

	    [Fact]
	    public virtual void Can_modify_column() {
            CreateTableFoo();
	        _dataClient.Modify
	                   .Column("name")
	                   .OfTable("foo")
	                   .WithDefinition(Column.String("name").NotNull());

            try {
                _dataClient.Insert.Into("foo").Columns("name").Values(DBNull.Value);
                Assert.True(false, "Should not insert in a non null column");
            }
            catch {}
	    }
	}
}