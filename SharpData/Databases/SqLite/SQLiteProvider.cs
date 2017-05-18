using System.Data.Common;

namespace SharpData.Databases.SqLite {
    public class SqLiteProvider : DataProvider {
        public SqLiteProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }

        public override DbProviderType Name => DbProviderType.SqLite;
        public override DatabaseKind DatabaseKind => DatabaseKind.Oracle;
    }
}