using System.Data.Common;

namespace Sharp.Data.Databases.Oracle {
    public class OracleManagedProvider : OracleOdpProvider {
        private static OracleReflectionCache _reflectionCache = new OracleReflectionCache();
        protected override string OracleDbTypeEnumName => "Oracle.ManagedDataAccess.Client.OracleDbType";
        public override OracleReflectionCache ReflectionCache => _reflectionCache;
        public override string Name { get; } = DataProviderNames.OracleManaged;
        public OracleManagedProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {}
    }
}