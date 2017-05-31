using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using SharpData.Util;
using SharpData.Schema;
using Sharp.Util;
using SharpData.Exceptions;

namespace SharpData.Databases.SqLite {

    public class SqLiteDialect : Dialect {

		public override string ParameterPrefix => "@";

        public override DbType GetDbType(string sqlType, int dataPrecision) {
            throw new NotImplementedException();
        }

        public override string[] GetCreateTableSqls(Table table) {
            var primaryKeyColumns = new List<string>();

            //create table
            var sb = new StringBuilder();
            sb.Append("create table ").Append(table.Name).AppendLine(" ( ");

            var size = table.Columns.Count;

            for (int i = 0; i < size; i++) {
                Column col = table.Columns[i];
                sb.Append(GetColumnToSqlWhenCreating(col));
                if (col.IsPrimaryKey) {
					primaryKeyColumns.Add(col.ColumnName);
                }
                if (i != size - 1) {
                    sb.AppendLine(",");
                }
            }

			if (primaryKeyColumns.Count > 1) {
				sb.Replace("primary key", "");
				sb.AppendFormat(", primary key({0}) ", String.Join(",", primaryKeyColumns.ToArray()));
            }
            sb.AppendLine(")");

            return new[] {sb.ToString()};
        }

        public override string GetColumnToSqlWhenCreating(Column col) {
        	var colAutoIncrement = "";
			if(col.IsAutoIncrement) {
				colAutoIncrement = "autoincrement";
				col.IsPrimaryKey = true;
			}
            var colType = GetDbTypeString(col.Type, col.Size);
            var colNullable = col.IsNullable ? WordNull : WordNotNull;
            var colPrimaryKey = col.IsPrimaryKey ? "primary key" : "";

            var colDefault = (col.DefaultValue != null) ?
                                        String.Format("default ({0})", GetColumnValueToSql(col.DefaultValue)) : "";

			return $"{col.ColumnName} {colType} {colNullable} {colDefault} {colPrimaryKey} {colAutoIncrement}";
        }

        public override string[] GetDropTableSqls(string tableName) {
            var sql = String.Format("drop table {0}", tableName);
            return new[] {sql};
        }
        

        public override string GetForeignKeySql(string fkName, string table, string column, string referencingTable,
                                                string referencingColumn, OnDelete onDelete) {
			throw new NotSupportedByDialectException("ForeignKeys  not supported by SqlLite", "GetForeignKeySql", GetDialectName());
        }

        public override string GetDropForeignKeySql(string fkName, string tableName) {
            throw new NotSupportedByDialectException("ForeignKeys not supported", "GetDropForeignKeySql", GetDialectName());
        }

    	public override string GetUniqueKeySql(string ukName, string table, params string[] columnNames) {
            return String.Format("create unique index {0} on {1} ({2})",
                                 ukName,
                                 table,
                                 String.Join(",", columnNames));
        }

        public override string GetDropUniqueKeySql(string uniqueKeyName, string tableName) {
            return String.Format("drop index {0}", uniqueKeyName);
        }

        public override string GetAddColumnSql(string table, Column column) {
            return String.Format("alter table {0} add {1}", table, GetColumnToSqlWhenCreating(column));
        }

        public override string[] GetDropColumnSql(string table, string columnName) {
            throw new NotSupportedByDialectException("Try recreating the table", "GetDropColumnSql", GetDialectName());
        }

        public override string GetInsertReturningColumnSql(string table, string[] columns, object[] values,
                                                           string returningColumnName, string returningParameterName) {
            throw new NotSupportedByDialectException("Not supported", "GetInsertReturningColumnSql", GetDialectName());
        }

    	public override string WrapSelectSqlWithPagination(string sql, int skipRows, int numberOfRows) {
    		throw new NotImplementedException();
    	}

    	protected override string GetDbTypeString(DbType type, int precision) {
            switch (type) {
                case DbType.AnsiStringFixedLength:
                    if (precision <= 0) {
                        return "CHAR(255)";
                    }
                    if (precision.Between(1, 255)) {
                        return String.Format("CHAR({0})", precision);
                    }
                    if (precision.Between(256, 65535)) {
                        return "TEXT";
                    }
                    if (precision.Between(65536, 16777215)) {
                        return "MEDIUMTEXT";
                    }
                    break;
                case DbType.AnsiString:
                    if (precision <= 0) {
                        return "VARCHAR(255)";
                    }
                    if (precision.Between(1, 255)) {
                        return String.Format("VARCHAR({0})", precision);
                    }
                    if (precision.Between(256, 65535)) {
                        return "TEXT";
                    }
                    if (precision.Between(65536, 16777215)) {
                        return "MEDIUMTEXT";
                    }
                    break;
                case DbType.Binary:
                    return "BINARY";
                case DbType.Boolean:
                    return "BIT";
                case DbType.Byte:
                    return "TINYINT UNSIGNED";
                case DbType.Currency:
                    return "MONEY";
                case DbType.Date:
                    return "DATETIME";
                case DbType.DateTime:
                    return "DATETIME";
                case DbType.Decimal:
                    if (precision <= 0) {
                        return "NUMERIC(19,5)";
                    }
                    else {
                        return String.Format("NUMERIC(19,{0})", precision);
                    }
                case DbType.Double:
                    return "FLOAT";
                case DbType.Guid:
                    return "VARCHAR(40)";
                case DbType.Int16:
                    return "SMALLINT";
                case DbType.Int32:
                    return "INTEGER";
                case DbType.Int64:
                    return "BIGINT";
                case DbType.Single:
                    return "FLOAT";
                case DbType.StringFixedLength:
                    if (precision <= 0) {
                        return "CHAR(255)";
                    }
                    if (precision.Between(1, 255)) {
                        return String.Format("CHAR({0})", precision);
                    }
                    if (precision.Between(256, 65535)) {
                        return "TEXT";
                    }
                    if (precision.Between(65536, 16777215)) {
                        return "MEDIUMTEXT";
                    }
                    break;
                case DbType.String:
                    if (precision <= 0) {
                        return "VARCHAR(255)";
                    }
                    if (precision.Between(1, 255)) {
                        return String.Format("VARCHAR({0})", precision);
                    }
                    if (precision.Between(256, 65535)) {
                        return "TEXT";
                    }
                    if (precision.Between(65536, 16777215)) {
                        return "MEDIUMTEXT";
                    }
                    break;
                case DbType.Time:
                    return "TIME";
            }
            throw new DataTypeNotAvailableException(String.Format("The type {0} is no available for sqlite", type));
        }

        //protected override string GetDefaultValueString(object defaultValue) {
        //    return defaultValue.ToString();
        //}

        public override string GetColumnValueToSql(object value) {
            if (value is bool) {
                return ((bool) value) ? "true" : "false";
            }

            if ((value is Int16) || (value is Int32) || (value is Int64) || (value is double) || (value is float) ||
                (value is decimal)) {
                return Convert.ToString(value, CultureInfo.InvariantCulture);
            }

            if (value is DateTime dt) {
                return dt.ToString("s");
            }

            return String.Format("'{0}'", value);
        }

    	public override string GetTableExistsSql(string tableName) {
    		return "SELECT count(name) FROM sqlite_master WHERE upper(name)=upper('" + tableName + "')";
    	}

        public override string GetAddCommentToColumnSql(string tableName, string columnName, string comment) {
            throw new NotImplementedException();
        }

        public override string GetAddCommentToTableSql(string tableName, string comment) {
            throw new NotImplementedException();
        }

        public override string GetRemoveCommentFromColumnSql(string tableName, string columnName) {
            throw new NotImplementedException();
        }

        public override string GetRemoveCommentFromTableSql(string tableName) {
            throw new NotImplementedException();
        }

        public override string GetRenameTableSql(string tableName, string newTableName) {
            throw new NotImplementedException();
        }

        public override string GetRenameColumnSql(string tableName, string columnName, string newColumnName) {
            throw new NotImplementedException();
        }

        public override string GetModifyColumnSql(string tableName, string columnName, Column columnDefinition) {
            throw new NotImplementedException();
        }
    }
}