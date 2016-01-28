using System.Data.Common;

namespace Sharp.Data.Databases.SqlServer {
    public class OleDbDbFactory : SqlServerDbFactory {
        public OleDbDbFactory(DbProviderFactory dbProviderFactory, string connectionString)
            : base(dbProviderFactory, connectionString) {
        }
    }
}