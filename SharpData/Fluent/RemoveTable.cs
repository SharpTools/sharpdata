using SharpData;

namespace SharpData.Fluent {
    public class RemoveTable : DataClientAction {
        public RemoveTable(IDataClient dataClient) : base(dataClient) {}

        protected override void ExecuteInternal() {
            DataClient.RemoveTable(TableNames[0]);
        }
    }
}