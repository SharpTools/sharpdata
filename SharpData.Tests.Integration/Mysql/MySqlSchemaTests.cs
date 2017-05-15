using Xunit;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Mysql {
   
    public class MySqlSchemaTests : DataClientSchemaTests {
        public MySqlSchemaTests() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.MySql);
        }
    }
}