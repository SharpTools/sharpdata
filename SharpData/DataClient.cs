using System;
using System.Linq;
using SharpData.Databases;
using SharpData.Filters;
using SharpData.Fluent;
using SharpData.Schema;

namespace SharpData {
    public class DataClient : IDataClient {
        public DataClient(IDatabase database, Dialect dialect) {
            Database = database;
            Dialect = dialect;
            ThrowException = true;
        }

        public IDatabase Database { get; set; }
        public Dialect Dialect { get; set; }
        public bool ThrowException { get; set; }

        public FluentAdd Add => new FluentAdd(this);
        public FluentRemove Remove => new FluentRemove(this);
        public FluentRename Rename => new FluentRename(this);
        public IFluentModify Modify => new FluentModify(this);
        public IFluentInsert Insert => new FluentInsert(this);
        public IFluentUpdate Update => new FluentUpdate(this);
        public IFluentSelect Select => new FluentSelect(this);
        public IFluentDelete Delete => new FluentDelete(this);
        public IFluentCount Count => new FluentCount(this);

        public virtual void AddTable(string tableName, params FluentColumn[] columns) {
            var table = new Table(tableName);
            foreach (var fcol in columns) table.Columns.Add(fcol.Object);
            var sqls = Dialect.GetCreateTableSqls(table);
            ExecuteSqls(sqls);
        }

        public virtual void RemoveTable(string tableName) {
            var sqls = Dialect.GetDropTableSqls(tableName);
            ExecuteSqls(sqls);
        }

        public virtual void AddPrimaryKey(string tableName, params string[] columnNames) {
            var sql = Dialect.GetPrimaryKeySql(tableName, "pk_" + tableName, columnNames);
            Database.ExecuteSql(sql);
        }

        public virtual void AddNamedPrimaryKey(string tableName, string pkName, params string[] columnNames) {
            var sql = Dialect.GetPrimaryKeySql(tableName, pkName, columnNames);
            Database.ExecuteSql(sql);
        }

        public virtual void AddForeignKey(string fkName, string table, string column, string referencingTable,
            string referencingColumn, OnDelete onDelete) {
            var sql = Dialect.GetForeignKeySql(fkName, table, column, referencingTable, referencingColumn, onDelete);
            Database.ExecuteSql(sql);
        }

        public void RemovePrimaryKey(string tableName, string primaryKeyName) {
            var sql = Dialect.GetDropPrimaryKeySql(tableName, primaryKeyName);
            Database.ExecuteSql(sql);
        }

        public virtual void RemoveForeignKey(string foreigKeyName, string tableName) {
            var sql = Dialect.GetDropForeignKeySql(foreigKeyName, tableName);
            Database.ExecuteSql(sql);
        }

        public virtual void AddUniqueKey(string uniqueKeyName, string tableName, params string[] columnNames) {
            var sql = Dialect.GetUniqueKeySql(uniqueKeyName, tableName, columnNames);
            Database.ExecuteSql(sql);
        }

        public virtual void RemoveUniqueKey(string uniqueKeyName, string tableName) {
            var sql = Dialect.GetDropUniqueKeySql(uniqueKeyName, tableName);
            Database.ExecuteSql(sql);
        }

        public virtual void AddIndex(string indexName, string tableName, params string[] columnNames) {
            var sql = Dialect.GetCreateIndexSql(indexName, tableName, columnNames);
            Database.ExecuteSql(sql);
        }

        public virtual void RemoveIndex(string indexName, string table) {
            var sql = Dialect.GetDropIndexSql(indexName, table);
            Database.ExecuteSql(sql);
        }

        public virtual void AddColumn(string tableName, Column column) {
            var sql = Dialect.GetAddColumnSql(tableName, column);
            Database.ExecuteSql(sql);
        }

        public virtual void RemoveColumn(string tableName, string columnName) {
            var sqls = Dialect.GetDropColumnSql(tableName, columnName);
            for (var i = 0; i < sqls.Length; i++) {
                Database.ExecuteSql(sqls[i]);
            }
        }

        public void AddTableComment(string tableName, string comment) {
            var sql = Dialect.GetAddCommentToTableSql(tableName, comment);
            Database.ExecuteSql(sql);
        }

        public void AddColumnComment(string tableName, string columnName, string comment) {
            var sql = Dialect.GetAddCommentToColumnSql(tableName, columnName, comment);
            Database.ExecuteSql(sql);
        }

        public virtual void RemoveTableComment(string tableName) {
            var sql = Dialect.GetRemoveCommentFromTableSql(tableName);
            Database.ExecuteSql(sql);
        }

        public virtual void RemoveColumnComment(string tableName, string columnName) {
            var sql = Dialect.GetRemoveCommentFromColumnSql(tableName, columnName);
            Database.ExecuteSql(sql);
        }

        public void RenameTable(string tableName, string newTableName) {
            var sql = Dialect.GetRenameTableSql(tableName, newTableName);
            Database.ExecuteSql(sql);
        }

        public void RenameColumn(string tableName, string columnName, string newColumnName) {
            var sql = Dialect.GetRenameColumnSql(tableName, columnName, newColumnName);
            Database.ExecuteSql(sql);
        }

        public void ModifyColumn(string tableName, string columnName, Column columnDefinition) {
            Database.ExecuteSql(Dialect.GetModifyColumnSql(tableName, columnName, columnDefinition));
        }

        public virtual ResultSet SelectSql(string[] tables, string[] columns, Filter filter, OrderBy[] orderBys,
            int skip, int take) {
            var selectBuilder = new SelectBuilder(Dialect, tables, columns);
            selectBuilder.Filter = filter;
            selectBuilder.OrderBys = orderBys;
            selectBuilder.Skip = skip;
            selectBuilder.Take = take;

            var sql = selectBuilder.Build();
            if (selectBuilder.HasFilter) {
                return Database.Query(sql, selectBuilder.Parameters);
            }
            return Database.Query(sql);
        }

        public virtual int InsertSql(string table, string[] columns, object[] values) {
            if (values == null) values = new object[columns.Length];
            var sql = Dialect.GetInsertSql(table, columns, values);
            return Database.ExecuteSql(sql, Dialect.ConvertToNamedParameters(values));
        }

        public virtual object InsertReturningSql(string table, string columnToReturn, string[] columns,
            object[] values) {
            var returningPar = new Out {Name = "returning_" + columnToReturn, Size = 4000};
            var retSql = Dialect.GetInsertReturningColumnSql(table, columns, values, columnToReturn, returningPar.Name);
            object[] pars = Dialect.ConvertToNamedParameters(values);
            var listPars = pars.ToList();
            listPars.Add(returningPar);
            Database.ExecuteSql(retSql, listPars.ToArray());
            return returningPar.Value;
        }

        public virtual int UpdateSql(string table, string[] columns, object[] values, Filter filter) {
            if (values == null) values = new object[columns.Length];
            var sql = Dialect.GetUpdateSql(table, columns, values);

            var parameters = Dialect.ConvertToNamedParameters(values);
            if (filter != null) {
                var whereSql = Dialect.GetWhereSql(filter, parameters.Count());
                var pars = filter.GetAllValueParameters();
                var filterParameters = Dialect.ConvertToNamedParameters(parameters.Count(), pars);
                filterParameters = filterParameters.Where(x => x.Value != null && x.Value != DBNull.Value).ToArray();
                parameters = parameters.Concat(filterParameters).ToArray();
                sql = sql + " " + whereSql;
            }

            return Database.ExecuteSql(sql, parameters);
        }

        public virtual int DeleteSql(string table, Filter filter) {
            var sql = Dialect.GetDeleteSql(table);

            if (filter != null) {
                var whereSql = Dialect.GetWhereSql(filter, 0);
                var pars = filter.GetAllValueParameters();
                var parameters = Dialect.ConvertToNamedParameters(0, pars);
                return Database.ExecuteSql(sql + " " + whereSql, parameters);
            }

            return Database.ExecuteSql(sql);
        }

        public virtual int CountSql(string table, Filter filter) {
            var sql = Dialect.GetCountSql(table);
            object obj;

            if (filter != null) {
                var whereSql = Dialect.GetWhereSql(filter, 0);
                var pars = filter.GetAllValueParameters();
                var parameters = Dialect.ConvertToNamedParameters(0, pars);
                obj = Database.QueryScalar(sql + " " + whereSql, parameters);
                return Convert.ToInt32(obj);
            }

            obj = Database.QueryScalar(sql);
            return Convert.ToInt32(obj);
        }

        public bool TableExists(string table) {
            var sql = Dialect.GetTableExistsSql(table);
            return Convert.ToInt32(Database.QueryScalar(sql)) > 0;
        }

        public void Commit() {
            Database?.Commit();
        }

        public void RollBack() {
            Database?.RollBack();
        }

        public void Close() {
            Database?.Close();
        }

        public void Dispose() {
            Close();
        }

        private void ExecuteSqls(string[] sqls) {
            foreach (var sql in sqls) {
                Database.ExecuteSql(sql);
            }
        }
    }
}