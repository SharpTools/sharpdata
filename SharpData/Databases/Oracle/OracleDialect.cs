using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using SharpData.Schema;

namespace SharpData.Databases.Oracle {
    public class OracleDialect : Dialect {
        public static string SequencePrefix = "SEQ_";
        public static string TriggerPrefix = "TR_INC_";
        public static string PrimaryKeyPrefix = "PK_";
        public override string ParameterPrefix => ":";
        public override string ScriptSeparator => "/";

        public override string[] GetCreateTableSqls(Table table) {
            var sqls = new List<string>();
            var primaryKeyColumns = new List<string>();
            Column autoIncrement = null;

            //create table
            var sb = new StringBuilder();
            sb.Append("create table ").Append(table.Name).AppendLine(" (");

            var size = table.Columns.Count;
            for (var i = 0; i < size; i++) {
                sb.Append(GetColumnToSqlWhenCreating(table.Columns[i]));
                if (i != size - 1) {
                    sb.AppendLine(",");
                }
                if (table.Columns[i].IsAutoIncrement) {
                    autoIncrement = table.Columns[i];
                }
                if (table.Columns[i].IsPrimaryKey) {
                    primaryKeyColumns.Add(table.Columns[i].ColumnName);
                }
            }
            sb.AppendLine(")");
            sqls.Add(sb.ToString());

            //create sequence and trigger for the autoincrement
            if (autoIncrement != null) {
                var sequenceName = SequencePrefix + table.Name;
                var triggerName = TriggerPrefix + table.Name;
                //create sequence in case of autoincrement
                sb = new StringBuilder();
                sb.AppendFormat("create sequence {0} minvalue 1 maxvalue 999999999999999999999999999 ", sequenceName);
                sb.Append("start with 1 increment by 1 cache 20");
                sqls.Add(sb.ToString());

                //create trigger to run the sequence
                sb = new StringBuilder();
                sb.AppendFormat("create or replace trigger \"{0}\" before insert on {1} for each row ", triggerName,
                    table.Name);
                sb.AppendFormat("when (new.{0} is null) ", autoIncrement.ColumnName);
                sb.AppendFormat("begin select {0}.nextval into :new.{1} from dual; end {2};", sequenceName,
                    autoIncrement.ColumnName, triggerName);
                sqls.Add(sb.ToString());
            }
            //primary key
            if (primaryKeyColumns.Count > 0) {
                sqls.Add(GetPrimaryKeySql(table.Name, String.Format("{0}{1}", PrimaryKeyPrefix, table.Name),
                    primaryKeyColumns.ToArray()));
            }
            //comments
            sqls.AddRange(GetColumnCommentsSql(table));
            return sqls.ToArray();
        }

        public override string[] GetDropTableSqls(string tableName) {
            return new[] {
                $"drop table {tableName} cascade constraints",
                $"begin execute immediate 'drop sequence {SequencePrefix}{tableName}'; exception when others then null; end;"
            };
        }

        public override string GetForeignKeySql(string fkName, string table, string column, string referencingTable,
            string referencingColumn, OnDelete onDelete) {
            string onDeleteSql;
            switch (onDelete) {
                case OnDelete.Cascade:
                    onDeleteSql = "on delete cascade";
                    break;
                case OnDelete.SetNull:
                    onDeleteSql = "on delete set null";
                    break;
                default:
                    onDeleteSql = "";
                    break;
            }

            return String.Format("alter table {0} add constraint {1} foreign key ({2}) references {3} ({4}) {5}",
                table,
                fkName,
                column,
                referencingTable,
                referencingColumn,
                onDeleteSql);
        }

        public override string GetUniqueKeySql(string ukName, string table, params string[] columnNames) {
            return String.Format("create unique index {0} on {1} ({2})", ukName, table, String.Join(",", columnNames));
        }

        public override string GetDropUniqueKeySql(string uniqueKeyName, string tableName) {
            return "drop index " + uniqueKeyName;
        }

        public override string GetDropIndexSql(string indexName, string table) {
            return String.Format("drop index {0}", indexName);
        }

        public override string GetInsertReturningColumnSql(string table, string[] columns, object[] values,
            string returningColumnName, string returningParameterName) {
            return String.Format("{0} returning {1} into {2}{3}",
                GetInsertSql(table, columns, values),
                returningColumnName,
                ParameterPrefix,
                returningParameterName);
        }

        public override string WrapSelectSqlWithPagination(string sql, int skipRows, int numberOfRows) {
            var innerSql = String.Format("select /* FIRST_ROWS(n) */ a.*, ROWNUM rnum from ({0}) a where ROWNUM <= {1}",
                sql,
                skipRows + numberOfRows);
            return String.Format("select * from ({0}) where rnum > {1}", innerSql, skipRows);
        }

        public override string GetColumnToSqlWhenCreating(Column col) {
            var colType = GetDbTypeString(col.Type, col.Size);
            var colNullable = col.IsNullable ? WordNull : WordNotNull;

            var colDefault = (col.DefaultValue != null)
                ? String.Format(" default {0}", GetColumnValueToSql(col.DefaultValue))
                : "";

            //name type default nullable
            return $"{col.ColumnName} {colType}{colDefault} {colNullable}";
        }

        public override string GetColumnValueToSql(object value) {
            if (value is bool) {
                return ((bool) value) ? "1" : "0";
            }
            if ((value is Int16) || (value is Int32) || (value is Int64) || (value is double) || (value is float) ||
                (value is decimal)) {
                return Convert.ToString(value, CultureInfo.InvariantCulture);
            }
            if (value is DateTime dt) {
                return String.Format("to_date('{0}','dd/mm/yyyy hh24:mi:ss')",
                    dt.ToString("d/M/yyyy H:m:s", CultureInfo.InvariantCulture));
            }
            return String.Format("'{0}'", value);
        }

        protected override string GetDbTypeString(DbType type, int precision) {
            switch (type) {
                case DbType.AnsiString:
                    if (precision == 0) return "CHAR(255)";
                    if (precision <= 4000) return "VARCHAR2(" + precision + ")";
                    return "CLOB";
                case DbType.Binary:
                    return "BLOB";
                case DbType.Boolean:
                    return "NUMBER(1)";
                case DbType.Byte:
                    return "NUMBER(3)";
                case DbType.Currency:
                    return "NUMBER(19,1)";
                case DbType.Date:
                    return "DATE";
                case DbType.DateTime:
                    return "DATE";
                case DbType.Decimal:
                    return "NUMBER(19,5)";
                case DbType.Double:
                    return "FLOAT";
                case DbType.Guid:
                    return "CHAR(38)";
                case DbType.Int16:
                    return "NUMBER(5)";
                case DbType.Int32:
                    return "NUMBER(10)";
                case DbType.Int64:
                    return "NUMBER(19)";
                case DbType.Single:
                    return "FLOAT(24)";
                case DbType.String:
                    if (precision == 0) return "VARCHAR2(255)";
                    if (precision <= 4000) return "VARCHAR2(" + precision + ")";
                    return "CLOB";
                case DbType.Time:
                    return "DATE";
            }
            throw new DataTypeNotAvailableException(String.Format("The type {0} is no available for oracle", type));
        }

        public override DbType GetDbType(string sqlType, int dataPrecision) {
            switch (sqlType) {
                case "varchar2":
                case "varchar":
                case "char":
                case "nchar":
                case "nvarchar2":
                case "rowid":
                case "nclob":
                case "clob":
                    return DbType.String;
                case "number":
                    return DbType.Decimal;
                case "float":
                    return DbType.Double;
                case "raw":
                case "long raw":
                case "blob":
                    return DbType.Binary;
                case "date":
                case "timestamp":
                    return DbType.DateTime;
                default:
                    return DbType.String;
            }
        }

        public override string GetTableExistsSql(string tableName) {
            return $"select count(table_name) from user_tables where upper(table_name) = upper('{tableName}')";
        }

        public override string GetAddCommentToColumnSql(string tableName, string columnName, string comment) {
            return $"COMMENT ON COLUMN {tableName}.{columnName} IS '{comment}'";
        }

        public override string GetAddCommentToTableSql(string tableName, string comment) {
            return $"COMMENT ON TABLE {tableName} IS '{comment}'";
        }

        public override string GetRemoveCommentFromColumnSql(string tableName, string columnName) {
            return $"COMMENT ON COLUMN {tableName}.{columnName} IS ''";
        }

        public override string GetRemoveCommentFromTableSql(string tableName) {
            return $"COMMENT ON TABLE {tableName} IS ''";
        }

        public override string GetRenameTableSql(string tableName, string newTableName) {
            return $"ALTER TABLE {tableName} RENAME TO {newTableName}";
        }

        public override string GetRenameColumnSql(string tableName, string columnName, string newColumnName) {
            return $"ALTER TABLE {tableName} RENAME COLUMN {columnName} TO {newColumnName}";
        }

        public override string GetModifyColumnSql(string tableName, string columnName, Column columnDefinition) {
            return $"alter table {tableName} modify {GetColumnToSqlWhenCreating(columnDefinition)}";
        }
        //              select p).FirstOrDefault();
        //              where p["CONSTRAINT_TYPE"].Equals("P")
        //    var pk = (from p in tableKeys

        //    //get PK from database table

        //    ResultSet tableKeys = _database.Query(TABLE_KEYS, table);
        //    ));
        //        }
        //            Type = GetDbType(p["data_type"].ToString(), Convert.ToInt32(p["data_precision"]))
        //        new Column(p["column_name"].ToString()) {
        //    t.ForEach(p => list.Add(

        //    int rows = t.Count;

        //    List<Column> list = new List<Column>();
        //    ResultSet t = _database.Query(TABLE_COLUMN_SQL, table);

        //public override List<Column> GetAllColumns(string table) {
        //                                  "and lower(a.table_name) = lower(:tableName)";
        //                                  "AND a.constraint_type IN ('R', 'P') " +
        //                                 "WHERE a.constraint_name = b.constraint_name " +
        //                                  "FROM user_constraints a, user_cons_columns b " +

        //private const string TABLE_KEYS = "SELECT a.constraint_name, a.constraint_type, b.column_name " +
        //                                        " WHERE lower(a.table_name) = lower(:tableName)";
        //                                        "  FROM user_tab_columns a " +
        //                                        "       a.nullable, a.data_type, a.char_length, a.data_precision, a.data_scale " +

        //private const string TABLE_COLUMN_SQL = "SELECT user, a.table_name, a.column_name, a.column_id, a.data_default, " +

        //public OracleDialect(Database database) : base(database) { }


        //public OracleDialect() : base() { }

        //    //table has to have a PK so we can identify it
        //    if (pk == null) {

        //        //get column matching pk
        //        var colPk = (from p in list
        //                     where p.ColumnName.Equals(pk["COLUMN_NAME"])
        //                     select p).FirstOrDefault();

        //        colPk.IsPrimaryKey = true;
        //    }

        //    return list;
        //}
    }
}