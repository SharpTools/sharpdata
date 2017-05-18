using System;

namespace SharpData.Fluent {
    public class ReversibleFluentActions {
        public event Action<DataClientAction> OnAction;

        public void FireOnAction(DataClientAction action) {
            if (OnAction != null) {
                OnAction(action);
            }
        }
    }
}