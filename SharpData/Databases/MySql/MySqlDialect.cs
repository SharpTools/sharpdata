using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Sharp.Data.Schema;
using Sharp.Util;

namespace Sharp.Data.Databases.MySql {
    public class MySqlDialect : Dialect {
        
        public override string ParameterPrefix => "@";

        public override DbType GetDbType(string sqlType, int dataPrecision) {
            throw new NotImplementedException();
        }

        public override string[] GetCreateTableSqls(Table table) {
			var sqls = new List<string>();
			var primaryKeyColumns = new List<string>();

			var sb = new StringBuilder();
			sb.Append("create table ").Append(table.Name).AppendLine(" ( ");

			var size = table.Columns.Count;
			for (var i = 0; i < size; i++) {
				sb.Append(GetColumnToSqlWhenCreating(table.Columns[i]));
				if (i != size - 1) {
					sb.AppendLine(",");
				}
				if (table.Columns[i].IsPrimaryKey) {
					primaryKeyColumns.Add(table.Columns[i].ColumnName);
				}
			}

			//primary keys
			if (primaryKeyColumns.Count > 0) {
				sb.AppendLine(String.Format(", primary key ({0})", String.Join(",", primaryKeyColumns.ToArray())));
			}
			sb.AppendLine(")");
			sqls.Add(sb.ToString());
			return sqls.ToArray();
        }

        public override string[] GetDropTableSqls(string tableName) {
        	return new[] {String.Format("drop table {0}",tableName)};
        }
       
        public override string GetDropPrimaryKeySql(object tableName, string primaryKeyName) {
            return String.Format("alter table {0} drop primary key", tableName);
        }

        public override string GetForeignKeySql(string fkName, string table, string column, string referencingTable, string referencingColumn, OnDelete onDelete) {
            throw new NotImplementedException();
        }

    	public override string GetUniqueKeySql(string ukName, string table, params string[] columnNames) {
            throw new NotImplementedException();
        }

        public override string GetDropUniqueKeySql(string uniqueKeyName, string tableName) {
            throw new NotImplementedException();
        }

        public override string GetInsertReturningColumnSql(string table, string[] columns, object[] values, string returningColumnName, string returningParameterName) {
            throw new NotImplementedException();
        }

    	public override string WrapSelectSqlWithPagination(string sql, int skipRows, int numberOfRows) {
    	    return sql + " limit " + skipRows + ", " + numberOfRows;
    	}

    	protected override string GetDbTypeString(DbType type, int precision) {
            switch (type) {
                case DbType.String:
                    if (precision == 0) {
                        return "VARCHAR(255)";
                    }
                    else if (precision <= 255) {
                        return "VARCHAR(" + precision + ")";
                    }
                    else if (precision <= 65536) {
                        return "TEXT";
                    }
                    else if (precision <= 65536) {
                        return "MEDIUMTEXT";
                    }
                    else {
                        return "LONGTEXT";
                    }
				case DbType.Binary:
				    if (precision > 0 && precision <= 255) {
				        return "TINYBLOB";
				    }
				    else if (precision <= 65536) {
				        return "BLOB";
				    }
				    else if (precision <= 65536) {
				        return "MEDIUMBLOB";
				    }
				    else {
				        return "LONGBLOB";
				    }
                case DbType.Boolean: return "BOOLEAN";
				case DbType.Decimal: return "DECIMAL";
				case DbType.Guid: return "VARCHAR(40)";
				case DbType.Int16: return "SMALLINT";
				case DbType.Int32: return "INT";
                case DbType.Int64: return "BIGINT";
                case DbType.Single: return "FLOAT";
                case DbType.Double: return "DOUBLE";
                case DbType.Date: return "DATE";
                case DbType.DateTime: return "DATETIME";
                case DbType.Time: return "TIME";
            }
            throw new DataTypeNotAvailableException(String.Format("The type {0} is no available for mysql", type.ToString()));
        }

        public override string GetColumnToSqlWhenCreating(Column col) {
            var nullOption = col.IsNullable ? WordNull : WordNotNull;
            var autoIncrement = col.IsAutoIncrement ? " AUTO_INCREMENT " : "";
            var defaultValue = col.DefaultValue != null ? " default "+GetColumnValueToSql(col.DefaultValue): "";
            return $"{col.ColumnName} {GetDbTypeString(col.Type, col.Size)} {nullOption}{defaultValue}{autoIncrement}";
        }

        public override string GetColumnValueToSql(object value) {
            if (value is bool) {
                return ((bool)value) ? "1" : "0";
            }

            if ((value is Int16) || (value is Int32) || (value is Int64) || (value is double) || (value is float) || (value is decimal)) {
                return Convert.ToString(value, CultureInfo.InvariantCulture);
            }

            if (value is DateTime dt) {
                return String.Format("'{0}'", dt.ToString("yyyy-MM-dd HH:mm:ss"));
            }

            return String.Format("'{0}'", value);
        }

    	public override string GetTableExistsSql(string tableName) {
	        return
	            $"select count(table_name) from information_schema.tables where table_schema = SCHEMA() and table_name = '{tableName}'";
	    }

        public override string GetAddCommentToColumnSql(string tableName, string columnName, string comment) {
            throw new NotImplementedException();
//            SELECT CONCAT(
//      CAST(COLUMN_NAME AS CHAR),
//      ' ',
//      CAST(COLUMN_TYPE AS CHAR),
//      IF(ISNULL(CHARACTER_SET_NAME),
//         '',
//         CONCAT(' CHARACTER SET ', CHARACTER_SET_NAME)),
//      IF(ISNULL(COLLATION_NAME), '', CONCAT(' COLLATE ', COLLATION_NAME)),
//      ' ',
//      IF(IS_NULLABLE = 'NO', 'NOT NULL ', ''),
//      IF(IS_NULLABLE = 'NO' AND ISNULL(COLUMN_DEFAULT),
//         '',
//         CONCAT('DEFAULT ', QUOTE(COLUMN_DEFAULT), ' ')),
//      UPPER(extra))
//      AS column_definition
// FROM INFORMATION_SCHEMA.COLUMNS
//WHERE  TABLE_NAME = 'issues'
//   AND COLUMN_NAME = 'id';
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
