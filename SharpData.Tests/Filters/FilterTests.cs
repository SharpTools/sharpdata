using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using SharpData.Filters;
using Xunit;
using SharpData.Query;
using FilterParameter = SharpData.Filters.FilterParameter;

namespace Sharp.Tests.Data.Filters {

	
	public class FilterTests {

		[Fact]
		public void Can_create_eq_operator_with_colName_and_value() {
			TestFilterCondition(Filter.Eq("col1", 1), CompareOperator.Equals);
		}

		[Fact]
		public void Can_create_gt_operator_with_colName_and_value() {
			TestFilterCondition(Filter.Gt("col1", 1), CompareOperator.GreaterThan);
		}

		[Fact]
		public void Can_create_lt_operator_with_colName_and_value() {
			TestFilterCondition(Filter.Lt("col1", 1), CompareOperator.LessThan);
		}

		[Fact]
		public void Can_create_ge_operator_with_colName_and_value() {
			TestFilterCondition(Filter.Ge("col1", 1), CompareOperator.GreaterOrEqualThan);
		}

		[Fact]
		public void Can_create_le_operator_with_colName_and_value() {
			TestFilterCondition(Filter.Le("col1", 1), CompareOperator.LessOrEqualThan);
		}

		private void TestFilterCondition(Filter filter, CompareOperator compareOperator) {
			FilterCondition filterCondition = (FilterCondition) filter;

			Assert.Equal(compareOperator, filterCondition.CompareOperator);

			CheckFilterParameter(filterCondition.Left, "col1", FilterParameterType.Column);
			CheckFilterParameter(filterCondition.Right, 1, FilterParameterType.Value);
		}

		private void CheckFilterParameter(object filter, object value, FilterParameterType type) {
			FilterParameter filterParameter = (FilterParameter) filter;
			Assert.Equal(value, filterParameter.Value);
			Assert.Equal(type, filterParameter.FilterParameterType);
		}

		[Fact]
		public void Can_combine_two_filters_with_and() {

			Filter filter1 = Filter.Eq("col1", 1);
			Filter filter2 = Filter.Eq("col2", 2);

			Filter filter = Filter.And(filter1, filter2);

			Assert.Equal(filter1, filter.Left);
			Assert.Equal(filter2, filter.Right);
			Assert.Equal(LogicOperator.And, ((FilterLogic)filter).LogicOperator);
		}

		[Fact]
		public void Can_combine__filterA1_and_filterA2_with_filterB1_and_filterB2_using_or() {

			Filter filterA1 = Filter.Eq("colA1", 1);
			Filter filterA2 = Filter.Eq("colA2", 2);

			Filter filterA = Filter.And(filterA1, filterA2);

			Filter filterB1 = Filter.Eq("colB1", 3);
			Filter filterB2 = Filter.Eq("colB2", 4);

			Filter filterB = Filter.And(filterB1, filterB2);

			Filter filter = Filter.Or(filterA, filterB);

			Assert.Equal(filterA, filter.Left);
			Assert.Equal(filterB, filter.Right);
			Assert.Equal(LogicOperator.Or, ((FilterLogic)filter).LogicOperator);
		}

		[Fact]
		public void Can_get_all_parameters_from_filter_tree() {

			Filter filterA1 = Filter.Eq("colA1", 1);
			Filter filterA2 = Filter.Eq("colA2", 2);

			Filter filterA = Filter.And(filterA1, filterA2);

			Filter filterB1 = Filter.Eq("colB1", 3);
			Filter filterB2 = Filter.Eq("colB2", 4);

			Filter filterB = Filter.And(filterB1, filterB2);

			Filter filter = Filter.Or(filterA, filterB);

			object[] values = filter.GetAllValueParameters();
			Assert.Equal(4, values.Count());
		}
	}
}