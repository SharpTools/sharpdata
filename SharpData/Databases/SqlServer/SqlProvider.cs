using System.Data.Common;
using Sharp.Data.Databases;
using Sharp.Data.Exceptions;
using System.Reflection;
using Sharp.Data.Util;

namespace Sharp.Data.Providers {
    public class SqlProvider : DataProvider {
        public SqlProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }

        public override string Name => DataProviderNames.SqlServer;

        public override DatabaseKind DatabaseKind => DatabaseKind.SqlServer;

        public override DatabaseException CreateSpecificException(System.Exception exception, string sql) {
            var numberProp = exception.GetType().GetProperty("Number", ReflectionHelper.NoRestrictions);
            if(numberProp == null) {
                return base.CreateSpecificException(exception, sql);
            }
            var number = numberProp.GetValue(exception) as int?;
            if(number == 208) {
                return new TableNotFoundException(exception.Message, exception, sql);
            }
            if (number == 2627) {
                return new UniqueConstraintException(exception.Message, exception, sql);
            }
            return base.CreateSpecificException(exception, sql);
        }
    }
}
