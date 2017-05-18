namespace SharpData.Fluent {
    public interface IFluentRename {
        IRenameTableTo Table(string tableName);
        IRenameColumnOfTable Column(string columnName);
    }
}