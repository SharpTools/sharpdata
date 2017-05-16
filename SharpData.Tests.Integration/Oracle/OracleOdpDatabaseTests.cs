using System.Reflection;
using Xunit;
using Sharp.Data.Databases;
using Sharp.Data.Databases.Oracle;
using Sharp.Data.Util;

namespace Sharp.Tests.Databases.Oracle {
   
    public class OracleOdpDatabaseTests : OracleManagedDatabaseTests {

        public OracleOdpDatabaseTests() {
            
        }

        protected override string GetDataProviderName() {
            return DataProviderNames.OracleOdp;
        }

        public override void Dispose() {
            base.Dispose();
            typeof(OracleOdpProvider).GetTypeInfo().GetField("_reflectionCache", ReflectionHelper.NoRestrictions)
                                     .SetValue(null, new OracleReflectionCache());
        }
    }
}
