using System.Data;

namespace SharpData.Schema {
    public class FluentColumn {

        public Column Object { get; protected set; }

        public FluentColumn(string name, DbType type) : this(name, type, -1) { }

        public FluentColumn(string name, DbType type = DbType.String, int size = -1) {
            Object = new Column(name, type);
            if (size > 0) {
                Object.Size = size;
            }
        }

        public FluentColumn Size(int size) {
            Object.Size = size;
            return this;
        }

        public FluentColumn NotNull() {
            Object.IsNullable = false;
            return this;
        }

        public FluentColumn AsPrimaryKey() {
            Object.IsPrimaryKey = true;
            return this;
        }

        public FluentColumn DefaultValue(object value) {
            Object.DefaultValue = value;
            return this;
        }

        public FluentColumn Comment(string comment) {
            Object.Comment = comment;
            return this;
        }
    }
}