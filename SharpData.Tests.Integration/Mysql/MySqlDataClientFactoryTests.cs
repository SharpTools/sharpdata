using System;
using MySql.Data.MySqlClient;
using Sharp.Data;
using Xunit;
using Sharp.Data.Databases;
using Sharp.Data.Databases.MySql;
using Sharp.Data.Providers;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Mysql {
   
    public class MySqlDataClientFactoryTests : DataClientFactoryTests {
        public override IDataClient CreateDataClient() {
            return SharpFactory.Default.CreateDataClient(MySqlClientFactory.Instance, GetDatabaseType());
        }

        public override string GetDatabaseType() {
            return DataProviderNames.MySql;
        }

        public override Type GetDataProviderType() {
            return typeof (MySqlProvider);
        }

        public override Type GetDataClientType() {
            return typeof(MySqlDataClient);            
        }

        public override Type GetDialectType() {
            return typeof(MySqlDialect);            
        }
    }
}