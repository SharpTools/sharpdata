using System;
using Xunit;
using Sharp.Data;

namespace Sharp.Tests.Databases.Data {
    public abstract class DataClientFactoryTests {
	    private const string ConnectionString = "connectionString";

        public abstract IDataClient CreateDataClient();

        [Fact]
        public virtual void Can_create_dataclient() {
            var client = CreateDataClient();

            //check connection string
            Assert.Equal(ConnectionString, client.Database.ConnectionString);
            
            //check DataClient type
            Assert.True(client.GetType() == GetDataClientType());

            //check Dialect type
            Assert.True(client.Dialect.GetType() == GetDialectType());
        }

        public abstract string GetDatabaseType();
        public abstract Type GetDataProviderType();
        public abstract Type GetDataClientType();
        public abstract Type GetDialectType();
    }
}