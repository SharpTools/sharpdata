using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using Sharp.Data;
using Sharp.Data.Filters;
using Sharp.Data.Schema;

namespace Sharp.Tests.Databases.Data {

    public abstract class DataClientDataTests : DataClientTests {

        [Fact]
        public virtual void Can_insert_returning_id() {
            _dataClient.AddTable("footable",
                                 Column.AutoIncrement("id"),
                                 Column.String("name")
                );

            Assert.Equal(1, _dataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id"));
            Assert.Equal(2, _dataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id"));
            Assert.Equal(3, _dataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id"));
        }

        [Fact]
        public virtual void Can_insert_dates_and_booleans() {
            _dataClient.AddTable("footable",
                                 Column.AutoIncrement("id"),
                                 Column.Date("colDate"),
                                 Column.Boolean("colBool"));


            DateTime now = DateTime.Now;

            _dataClient.Insert.Into("footable").Columns("colDate", "colBool").Values(now, true);

            ResultSet res = _dataClient.Select.Columns("colDate", "colBool").From("footable").AllRows();

            Assert.Equal(now.ToString(), res[0][0].ToString());

            Assert.Equal(true, res[0][1]);
        }

        [Fact]
        public virtual void Can_insert_ints_and_strings() {
            _dataClient.AddTable("footable",
                                 Column.Int32("colInt"),
                                 Column.String("colString"));

            _dataClient.Insert.Into("footable").Columns("colInt", "colString").Values(1, "asdf");

            ResultSet res = _dataClient.Select.Columns("colInt", "colString").From("footable").AllRows();

            Assert.Equal(1, res[0][0]);
            Assert.Equal("asdf", res[0][1]);
        }

        [Fact]
        public virtual void Can_insert_with_only_null() {
            _dataClient.AddTable("footable",
                                 Column.AutoIncrement("id"),
                                 Column.String("name")
            );

            _dataClient.Insert.Into("footable").Columns("name").Values(null);
            ResultSet res = _dataClient.Select.Columns("name").From("footable").AllRows();
            Assert.Null(res[0][0]);
        }

        [Fact]
        public virtual void Can_insert_with_values_plus_null() {
            _dataClient.AddTable("footable",
                                 Column.AutoIncrement("id"),
                                 Column.String("name"),
                                 Column.String("surname")
            );

            _dataClient.Insert.Into("footable").Columns("name", "surname").Values("foo", null);

            ResultSet res = _dataClient.Select.Columns("name", "surname").From("footable").AllRows();

            Assert.Equal("foo", res[0][0]);
            Assert.Null(res[0][1]);
        }

        [Fact]
        public virtual void Can_select_all_rows() {
            CreateTableFoo();
            PopulateTableFoo();

            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(tableFoo)
                .AllRows();

            Assert.Equal(3, res.Count);
            Assert.Equal(1, res[0][0]);
        }

        [Fact]
        public virtual void Can_select_with_filter() {
            CreateTableFoo();
            PopulateTableFoo();

            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(tableFoo)
                .Where(
                Filter.Eq("id", 1)
                ).AllRows();

            Assert.Equal(1, res.Count);
            Assert.Equal(1, res[0][0]);
        }

        [Fact]
        public void Can_select_with_pagination() {
            CreateTableFoo();
            PopulateTableFoo();

            ResultSet res = _dataClient.Select
                                       .AllColumns()
                                       .From(tableFoo)
                                       .SkipTake(1, 1);

            Assert.Equal(1, res.Count);
            Assert.Equal(2, res[0][0]);
        }

        [Fact]
        public void Can_select_with_pagination_and_where_filter() {
            CreateTableFoo();

            for (int i = 0; i < 10; i++) {
                _dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(i, "sameValue");
            }
            for (int i = 10; i < 20; i++) {
                _dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(i, "otherValue");
            }

            ResultSet res = _dataClient.Select
                                       .AllColumns()
                                       .From(tableFoo)
                                       .Where(Filter.Eq("name", "sameValue"))
                                       .SkipTake(5, 5);

            Assert.Equal(5, res.Count);
            Assert.Equal("sameValue", res[0][1]);
        }

        [Fact]
        public virtual void Can_select_with_complex_filter() {
            CreateTableFoo();
            PopulateTableFoo();

            Filter complexFilter = CreateComplexFilter();

            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(tableFoo)
                .Where(complexFilter)
                .AllRows();

            Assert.Equal(2, res.Count);
            Assert.Equal(1, res[0][0]);
            Assert.Equal(2, res[1][0]);
        }

        private static Filter CreateComplexFilter() {
            Filter idEqualsOne = Filter.Eq("id", 1);
            Filter nameEqualsV1 = Filter.Eq("name", "v1");
            Filter idEqualsTwo = Filter.Eq("id", 2);

            Filter filterAnd = Filter.And(idEqualsOne, nameEqualsV1);
            Filter filterOr = Filter.Or(filterAnd, idEqualsTwo);
            return filterOr;
        }

        [Fact]
        public virtual void Can_update_rows() {
            CreateTableFoo();
            PopulateTableFoo();

            int num = _dataClient.Update
                .Table(tableFoo)
                .SetColumns("name")
                .ToValues("vvv")
                .AllRows();

            ResultSet res = _dataClient.Select.Columns("name").From(tableFoo).AllRows();

            Assert.Equal(3, num);
            Assert.Equal(3, res.Count);
            Assert.Equal("vvv", res[0][0]);
            Assert.Equal("vvv", res[1][0]);
            Assert.Equal("vvv", res[2][0]);
        }

        [Fact]
        public virtual void Can_update_rows_with_filter() {
            CreateTableFoo();
            PopulateTableFoo();

            int num = _dataClient.Update
                .Table(tableFoo)
                .SetColumns("name")
                .ToValues("vvv")
                .Where(
                    Filter.Or(Filter.Eq("id", 1), Filter.Eq("id", 2))
                );

            ResultSet res = _dataClient.Select.Columns("name").From(tableFoo).AllRows();
            IEnumerable<object> allValues = res.SelectMany(col => col).ToList();

            Assert.Equal(2, num);
            Assert.Equal(3, res.Count);
            Assert.Equal(2, allValues.Count(row => row.ToString().Equals("vvv")));
            Assert.Equal(1, allValues.Count(row => row.ToString().Equals("v3")));
        }

        [Fact]
        public virtual void Can_update_rows_with_filter_with_null() {
            CreateTableFoo();
            PopulateTableFoo();

            _dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(10, null);

            var vvvOrNull = Filter.Or(Filter.Eq("name", null), Filter.Eq("name", "vvv"));

            int num = _dataClient.Update
                .Table(tableFoo)
                .SetColumns("name")
                .ToValues("vvv")
                .Where(
                    Filter.And(Filter.Eq("id", 10), vvvOrNull)
                );

            ResultSet res = _dataClient.Select.Columns("name").From(tableFoo).AllRows();

            Assert.Equal(1, num);
            Assert.Equal(4, res.Count);
            Assert.Equal("v1", res[0][0]);
            Assert.Equal("v2", res[1][0]);
            Assert.Equal("v3", res[2][0]);
            Assert.Equal("vvv", res[3][0]);
        }

        [Fact]
        public virtual void Can_update_rows_with_filter_with_null_as_second_param() {
            CreateTableFoo();
            PopulateTableFoo();

            _dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(10, null);

            var vvvOrNull = Filter.Or(Filter.Eq("name", "vvv"), Filter.Eq("name", null));

            int num = _dataClient.Update
                .Table(tableFoo)
                .SetColumns("name")
                .ToValues("vvv")
                .Where(
                    Filter.And(Filter.Eq("id", 10), vvvOrNull)
                );

            ResultSet res = _dataClient.Select.Columns("name").From(tableFoo).AllRows();

            Assert.Equal(1, num);
            Assert.Equal(4, res.Count);
            Assert.Equal("v1", res[0][0]);
            Assert.Equal("v2", res[1][0]);
            Assert.Equal("v3", res[2][0]);
            Assert.Equal("vvv", res[3][0]);
        }

        [Fact]
        public void Can_update_all_rows_to_null() {
            CreateTableFoo();
            _dataClient.Update.Table(tableFoo).SetColumns("name").ToValues(null).AllRows();

        }

        [Fact]
        public void Can_update_all_rows_to_DBNull() {
            CreateTableFoo();
            _dataClient.Update.Table(tableFoo).SetColumns("name").ToValues(DBNull.Value).AllRows();

        }

        [Fact]
        public virtual void Can_delete_all_rows() {
            CreateTableFoo();
            PopulateTableFoo();

            int num = _dataClient.Delete.From(tableFoo).AllRows();

            ResultSet res = _dataClient.Select.AllColumns().From(tableFoo).AllRows();

            Assert.Equal(3, num);
            Assert.Equal(0, res.Count);
        }

        [Fact]
        public virtual void Can_delete_rows_with_filter() {
            CreateTableFoo();
            PopulateTableFoo();

            Filter idEquals1 = Filter.Eq("id", 1);

            int num = _dataClient.Delete.From(tableFoo).Where(idEquals1);

            ResultSet res = _dataClient.Select.AllColumns().From(tableFoo).AllRows();

            Assert.Equal(1, num);
            Assert.Equal(2, res.Count);
        }

        [Fact]
        public void Can_count() {
            CreateTableFoo();
            PopulateTableFoo();

            int num = _dataClient.Count.Table(tableFoo).AllRows();

            Assert.Equal(3, num);
        }

        [Fact]
        public void Can_count_with_filter() {
            CreateTableFoo();
            PopulateTableFoo();

            int num = _dataClient.Count.Table(tableFoo).Where(Filter.Eq("id", 1));

            Assert.Equal(1, num);
        }

        [Fact]
        public void Can_order_by_asc() {
            CreateTableFoo();
            PopulateTableFoo();
            _dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(4, "aaa");

            ResultSet res = _dataClient.Select.AllColumns().From(tableFoo).OrderBy(OrderBy.Ascending("name")).AllRows();
            Assert.Equal("aaa", res[0]["name"]);
        }

        [Fact]
        public void Can_order_by_desc() {
            CreateTableFoo();
            PopulateTableFoo();
            _dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(4, "aaa");

            ResultSet res = _dataClient.Select.AllColumns().From(tableFoo).OrderBy(OrderBy.Descending("name")).AllRows();
            Assert.Equal("v3", res[0]["name"]);
        }

        [Fact]
        public void Can_order_by_with_filter_and_pagination() {
            CreateTableFoo();
            PopulateTableFoo();
            _dataClient.Insert.Into(tableFoo).Columns("id", "name").Values(4, "aaa");

            ResultSet res = _dataClient.Select
                                        .AllColumns()
                                        .From(tableFoo)
                                        .Where(Filter.Gt("id", 1))
                                        .OrderBy(OrderBy.Ascending("name"))
                                        .SkipTake(1, 2);

            Assert.Equal("v2", res[0]["name"]);
        }

    }
}
