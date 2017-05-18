using System.Reflection;
using SharpData.Databases;
using SharpData.Databases.Oracle;
using SharpData.Util;

namespace SharpData.Tests.Integration.Oracle {
   
    public class OracleOdpDatabaseTests : OracleManagedDatabaseTests {

        public OracleOdpDatabaseTests() {
            
        }

        protected override DbProviderType GetDataProviderName() {
            return DbProviderType.OracleOdp;
        }

        public override void Dispose() {
            base.Dispose();
            typeof(OracleOdpProvider).GetTypeInfo().GetField("_reflectionCache", ReflectionHelper.NoRestrictions)
                                     .SetValue(null, new OracleReflectionCache());
        }
    }
}
