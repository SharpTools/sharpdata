using System.Collections.Generic;

namespace SharpData {
    public class TableRow : List<object> {

        protected ResultSet _table;

        public TableRow(ResultSet table, IEnumerable<object> values) {
            _table = table;
            AddRange(values);
        }

        public object this[string col] {
            get {
                return this[_table.GetColumnIndex(col)];
            }
        }

		public string[] GetColumnNames() {
			return _table.GetColumnNames();
		}
    }
}