using Xunit;
using Sharp.Data;
using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.SqlServer {
   
    public class SqlServerDataTests : DataClientDataTests {
        public SqlServerDataTests() {
            _dataClient = DBBuilder.GetDataClient(DataProviderNames.SqlServer);
        }
    }
}