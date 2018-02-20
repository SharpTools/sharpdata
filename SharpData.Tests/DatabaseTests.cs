using System;
using System.Data;
using System.Data.Common;
using Moq;
using SharpData;
using Xunit;

namespace Sharp.Tests.Data {

    public class DatabaseTests {
        private Mock<IDbCommand> _cmd;
        private Mock<IDbConnection> _connection;
        private Database _db;
        private Mock<DbParameter> _parameter1, _parameter2;
        private Mock<IDataProvider> _provider;
        private string _sql = "foo";
        private Mock<IDbTransaction> _transaction;
        private Mock<Dialect> _dialect;

        public DatabaseTests() {
            _dialect = new Mock<Dialect>();
            _provider = new Mock<IDataProvider>();
            _connection = new Mock<IDbConnection>();
            _transaction = new Mock<IDbTransaction>();
            _cmd = new Mock<IDbCommand>();
            _parameter1 = new Mock<DbParameter>();
            _parameter2 = new Mock<DbParameter>();
            _parameter1.SetupAllProperties();
            _parameter2.SetupAllProperties();
            
            var allParameters = new DataParameterCollectionFake();

            _connection.Setup(p => p.BeginTransaction()).Returns(_transaction.Object);
            _connection.Setup(p => p.CreateCommand()).Returns(_cmd.Object);
            _provider.Setup(p => p.GetConnection()).Returns(_connection.Object);
            _provider.Setup(p => p.GetParameter())
                .Returns(() => _parameter1.Object)
                .Callback(() => _provider.Setup(p => p.GetParameter())
                                         .Returns(_parameter2.Object));

            _provider.Setup(p => p.GetParameter(It.IsAny<In>(), false)).Returns(() => _parameter1.Object)
                                  .Callback(() => _provider.Setup(p => p.GetParameter())
                                  .Returns(_parameter2.Object));

            _cmd.SetupGet(p => p.Parameters).Returns(allParameters);

            _db = new Database(_provider.Object, "");
        }

        [Fact]
        public void Can_execute_sql_with_parameters() {
            _cmd.Setup(p => p.ExecuteNonQuery()).Returns(1);

            Assert.Equal(1, _db.ExecuteSql(_sql, "par1", "par2"));
            Assert.Equal(2, _cmd.Object.Parameters.Count);
        }

        [Fact]
        public void Can_execute_sql_without_parameters() {
            _cmd.Setup(p => p.ExecuteNonQuery()).Returns(1);

            Assert.Equal(1, _db.ExecuteSql(_sql));
        }

        [Fact]
        public void Commit_commits_the_transaction_and_closes() {
            _cmd.Setup(p => p.ExecuteNonQuery()).Returns(1);
            _db.ExecuteSql(_sql);
            _db.Commit();

            _transaction.Verify(p => p.Commit());
            _connection.Verify(p => p.Close());
        }

        [Fact]
        public void Constructor_should_set_provider_and_connectionstring() {
            var dataProvider = new Mock<IDataProvider>().Object;
            var db = new Database(dataProvider, "foo");

            Assert.Equal("foo", db.ConnectionString);
            Assert.Equal(dataProvider, db.Provider);
        }

        [Fact]
        public void Do_not_roolback_when_exception() {
            _cmd.Setup(p => p.ExecuteNonQuery()).Throws(new Exception("foo"));

            try {
                _db.ExecuteSql(_sql);
            }
            catch { }
            _transaction.Verify(p => p.Rollback(), Times.Never());
        }

        [Fact]
        public void Throws_DatabaseException_when_some_exception_is_thrown() {
            var ex = new Exception("moo");
            _cmd.Setup(p => p.ExecuteNonQuery()).Throws(ex);
            _provider.Setup(x => x.CreateSpecificException(ex, _sql)).Returns(new DatabaseException("moo", ex, _sql));
            Assert.Throws<DatabaseException>(() => _db.ExecuteSql(_sql));
        }
    }
}