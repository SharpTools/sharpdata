using System;
using System.Data.Common;
using System.Reflection;
using Sharp.Data.Databases;
using Sharp.Data.Exceptions;
using Sharp.Data.Util;

namespace Sharp.Data.Providers {
    public class MySqlProvider : DataProvider {
        public MySqlProvider(DbProviderFactory dbProviderFactory) : base(dbProviderFactory) {
        }

        public override string Name => DataProviderNames.MySql;

        public override DatabaseKind DatabaseKind => DatabaseKind.MySql;

        public override DatabaseException CreateSpecificException(Exception exception, string sql) {
            var numberProp = exception.GetType().GetProperty("Number", ReflectionHelper.NoRestrictions);
            if (numberProp == null) {
                return base.CreateSpecificException(exception, sql);
            }
            var number = numberProp.GetValue(exception) as int?;
            if (number == 1075) {
                return new NotSupportedByDatabaseException("Mysql databases require autoincrement columns to be the primary key", exception, sql);
            }
            return base.CreateSpecificException(exception, sql);
        }
    }
}