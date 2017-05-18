using System;
using SharpData.Query;

namespace SharpData.Filters {
	public static class LogicOperatorToSymbol {

		public static string Get(LogicOperator logicOperator) {
		    return logicOperator.ToString();
		}
	}
}
