using Sharp.Data.Databases;
using Sharp.Tests.Databases.Data;

namespace Sharp.Tests.Databases.Mysql {
    public class MySqlSchemaTests : DataClientSchemaTests {
        protected override string GetDataProviderName() {
            return DataProviderNames.MySql;
        }
    }
}