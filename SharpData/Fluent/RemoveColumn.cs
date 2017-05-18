using SharpData;

namespace SharpData.Fluent {
    public class RemoveColumn : RemoveItemFromTable {

        public RemoveColumn(IDataClient dataClient) : base(dataClient) {}

        protected override void ExecuteInternal() {
            DataClient.RemoveColumn(TableNames[0], ItemName);
        }
    }
}