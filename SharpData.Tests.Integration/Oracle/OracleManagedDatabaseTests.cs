using System;
using System.Data;
using Xunit;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Data.Databases.Oracle;
using Sharp.Data.Schema;
using Sharp.Data.Util;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Oracle {
   
    public class OracleManagedDatabaseTests : DatabaseTests {

        public OracleManagedDatabaseTests() {
            _dataClient = DBBuilder.GetDataClient(GetDataProviderName());
            _database = _dataClient.Database;
            CleanTables();
        }

        public virtual string GetDataProviderName() {
            return DataProviderNames.OracleManaged;
        }

        public override void Can_bulk_insert_stored_procedure() {
            _dataClient.AddTable(TableFoo, Column.Int32("colInt"));
            try {
                _database.ExecuteSql("drop procedure pr_bulk");
            }
            catch { }
            _database.ExecuteSql("create or replace procedure pr_bulk(v_value in number) is begin insert into foo (colInt) values (v_value); end pr_bulk;");

            var v1s = new[] { 1, 2, 3, 4 };

            _database.ExecuteBulkStoredProcedure("pr_bulk", In.Named("v_value", v1s));
            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();

            Assert.Equal(4, res.Count);
        }

        public override void Can_bulk_insert_stored_procedure_with_nullable() {
            _dataClient.AddTable(TableFoo, Column.Decimal("colDecimal"));
            try {
                _database.ExecuteSql("drop procedure pr_bulk");
            }
            catch { }
            _database.ExecuteSql("create or replace procedure pr_bulk(v_value in float) is begin insert into foo (colDecimal) values (v_value); end pr_bulk;");

            var v1s = new decimal?[] { 1, 2, 3, 4, null };

            _database.ExecuteBulkStoredProcedure("pr_bulk", In.Named("v_value", v1s));
            var res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();

            Assert.Equal(5, res.Count);
        }
        [Fact]
        public override void Can_bulk_insert_stored_procedure_with_first_item_null() {
            _dataClient.AddTable(TableFoo, Column.Decimal("colDecimal"));
            try {
                _database.ExecuteSql("drop procedure pr_bulk");
            }
            catch { }
            _database.ExecuteSql("create or replace procedure pr_bulk(v_value in float) is begin insert into foo (colDecimal) values (v_value); end pr_bulk;");

            var v1s = new decimal?[] { null, 2, null, 4, null };

            _database.ExecuteBulkStoredProcedure("pr_bulk", In.Named("v_value", v1s));
            var res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();

            Assert.Equal(5, res.Count);
        }

        [Fact]
        public override void Can_bulk_insert_stored_procedure_with_all_items_null() {
            _dataClient.AddTable(TableFoo, Column.Decimal("colDecimal"));
            try {
                _database.ExecuteSql("drop procedure pr_bulk");
            }
            catch { }
            _database.ExecuteSql("create or replace procedure pr_bulk(v_value in float) is begin insert into foo (colDecimal) values (v_value); end pr_bulk;");

            var v1s = new decimal?[] { null, null, null, null, null };

            _database.ExecuteBulkStoredProcedure("pr_bulk", In.Named("v_value", v1s));
            ResultSet res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();

            Assert.Equal(5, res.Count);
        }

        public override void Can_bulk_insert_stored_procedure_with_nullable_and_dates() {
            _dataClient.AddTable(TableFoo, Column.Decimal("colDecimal"), Column.Date("colDate"));
            try {
                _database.ExecuteSql("drop procedure pr_bulk");
            }
            catch { }
            _database.ExecuteSql("create or replace procedure pr_bulk(v_value in float, v_date in date) is begin insert into foo (colDecimal, colDate) values (v_value, v_date); end pr_bulk;");

            var v1s = new decimal?[] { 1, 2, 3, 4, null };
            var v2s = new[] { DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now };

            _database.ExecuteBulkStoredProcedure("pr_bulk", In.Named("v_value", v1s), In.Named("v_date", v2s));
            var res = _dataClient.Select
                .AllColumns()
                .From(TableFoo)
                .AllRows();
            Assert.Equal(5, res.Count);
        }

        public override void Can_call_stored_function_with_return_as_string() {
            _database.ExecuteSql("create or replace function fn_test(par1 in varchar2, par2 in varchar2) return varchar2 is begin return upper(par1 || par2); end fn_test;");
            var result = _database.CallStoredFunction(DbType.String, "fn_test", In.Named("par1", "a"), In.Named("par2", "b"));
            Assert.Equal("AB", result);
        }

        public override void Can_call_stored_function_with_return_as_int() {
            _database.ExecuteSql("create or replace function fn_test(par1 in varchar2, par2 in varchar2) return int is begin return 1; end fn_test;");
            var result = _database.CallStoredFunction(DbType.Int32, "fn_test", In.Named("par1", "a"), In.Named("par2", "b"));
            Assert.Equal(1, result);
        }

        public override void Dispose() {
            base.Dispose();
            typeof(OracleManagedProvider).GetField("_reflectionCache", ReflectionHelper.NoRestrictions)
                                         .SetValue(null, new OracleReflectionCache());
        }
    }
}