using System;
using SharpData.Exceptions;
using SharpData.Schema;
using Xunit;

namespace SharpData.Tests.Integration.Data {
    public abstract class SpecificExceptionsTests : IDisposable {

        protected IDataClient _dataClient;
        protected IDatabase _database;

        [Fact]
        public virtual void TableNotFoundException__DataClient__insert() {
            Assert.Throws<TableNotFoundException>(() => {
                _dataClient.Insert.Into("footable").Columns("name").ValuesAnd("asdf").Return<int>("id");
            });
        }

        [Fact]
        public virtual void TableNotFoundException__Database__insert() {
            Assert.Throws<TableNotFoundException>(() => {
                _database.ExecuteSql("insert into moo values (1)");
            });
        }

        [Fact]
        public virtual void TableNotFoundException__DataClient__update() {
            Assert.Throws<TableNotFoundException>(() => {
                _dataClient.Update.Table("moo").SetColumns("moo").ToValues(1).AllRows();
            });
        }

        [Fact]
        public virtual void TableNotFoundException__Database__update() {
            Assert.Throws<TableNotFoundException>(() => {
                _database.ExecuteSql("update moo set moo=1");
            });
        }

        [Fact]
        public virtual void TableNotFoundException__DataClient__select() {
            Assert.Throws<TableNotFoundException>(() => {
                _dataClient.Select.AllColumns().From("moo").AllRows();
            });
        }

        [Fact]
        public virtual void TableNotFoundException__Database__select() {
            Assert.Throws<TableNotFoundException>(() => {
                _database.ExecuteSql("select * from moo");
            });
        }


        private void CreateTableFoo() {
            _dataClient.Add.Table("foo").WithColumns(Column.Int32("id").AsPrimaryKey());
        }

        [Fact]
        public virtual void UniqueConstraintException__DataClient__insert() {
            CreateTableFoo();
            _dataClient.Insert.Into("foo").Columns("id").Values(1);
            Assert.Throws<UniqueConstraintException>(() => {
                _dataClient.Insert.Into("foo").Columns("id").Values(1);
            });
        }

        [Fact]
        public virtual void UniqueConstraintException__Database__insert() {
             CreateTableFoo();
            Assert.Throws<UniqueConstraintException>(() => {
                _database.ExecuteSql("insert into foo values (1)");
            _database.ExecuteSql("insert into foo values (1)");
            });
        }

        [Fact]
        public virtual void UniqueConstraintException__DataClient__update() {
             CreateTableFoo();
            
            _dataClient.Insert.Into("foo").Columns("id").Values(1);
            _dataClient.Insert.Into("foo").Columns("id").Values(2);
            Assert.Throws<UniqueConstraintException>(() => {
                _dataClient.Update.Table("foo").SetColumns("id").ToValues(1).AllRows();
            });
        }

        [Fact]
        public virtual void UniqueConstraintException__Database__update() {
             CreateTableFoo();
            _database.ExecuteSql("insert into foo values (1)");
            _database.ExecuteSql("insert into foo values (2)");
            Assert.Throws<UniqueConstraintException>(() => {
                _database.ExecuteSql("update foo set id=1");
            });
        }

        public void Dispose() {
            if (_dataClient.TableExists("foo")) {
                _dataClient.RemoveTable("foo");
            }
            _dataClient.RollBack();
            _dataClient.Dispose();
        }
    }
}
