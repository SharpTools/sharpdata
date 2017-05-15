using Xunit;
using Sharp.Data.Databases;
using Sharp.Data.Databases.Oracle;
using Sharp.Data.Util;

namespace Sharp.Tests.Databases.Oracle {
   
    public class OracleOdpDatabaseTests : OracleManagedDatabaseTests {
        public override string GetDataProviderName() {
            return DataProviderNames.OracleOdp;
        }

        public override void TearDown() {
            base.TearDown();
            typeof(OracleOdpProvider).GetField("_reflectionCache", ReflectionHelper.NoRestrictions).SetValue(null, new OracleReflectionCache());
        }
    }
}
