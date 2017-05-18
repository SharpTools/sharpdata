using System.Data.Common;

namespace SharpData.Databases.SqlServer {
    public class OleDbDbFactory : SqlServerDbFactory {
        public OleDbDbFactory(DbProviderFactory dbProviderFactory, string connectionString)
            : base(dbProviderFactory, connectionString) {
        }
    }
}