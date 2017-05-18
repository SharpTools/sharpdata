using System.Data.Common;

namespace SharpData.Databases.Oracle {
    public class OracleManagedProvider : OracleOdpProvider {
        private static OracleReflectionCache _reflectionCache = new OracleReflectionCache();
        protected override string OracleDbTypeEnumName => "Oracle.ManagedDataAccess.Client.OracleDbType";
        public override OracleReflectionCache ReflectionCache => _reflectionCache;
        public override DbProviderType Name { get; } = DbProviderType.OracleManaged;
        public OracleManagedProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {}
    }
}