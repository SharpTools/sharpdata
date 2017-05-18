using System;
using SharpData.Databases;
using Xunit;

namespace SharpData.Tests.Integration.Data {
    public abstract class DataClientFactoryTests {
        public IDataClient CreateDataClient() {
            return DBBuilder.GetDataClient(GetDatabaseType());
        }

        [Fact]
        public virtual void Can_create_dataclient() {
            var client = CreateDataClient();

            //check connection string
            Assert.Equal(DBBuilder.GetConnectionString(GetDatabaseType()), client.Database.ConnectionString);
            
            //check DataClient type
            Assert.True(client.GetType() == GetDataClientType());

            //check Dialect type
            Assert.True(client.Dialect.GetType() == GetDialectType());
        }

        public abstract DbProviderType GetDatabaseType();
        public abstract Type GetDataProviderType();
        public abstract Type GetDataClientType();
        public abstract Type GetDialectType();
    }
}