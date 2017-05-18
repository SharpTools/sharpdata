using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;
using SharpData.Filters;
using SharpData.Query;

namespace Sharp.Tests.Data.Filters {
	
	public class CompareOperatorToSymbolTests {

		[Fact]
		public void Should_return_equal_symbol() {
			Assert.Equal("=",CompareOperatorToSymbol.Get(CompareOperator.Equals));
		}

		[Fact]
		public void Should_return_greater_symbol() {
			Assert.Equal(">", CompareOperatorToSymbol.Get(CompareOperator.GreaterThan));
		}

		[Fact]
		public void Should_return_less_than_symbol() {
			Assert.Equal("<", CompareOperatorToSymbol.Get(CompareOperator.LessThan));
		}

		[Fact]
		public void Should_return_greater_or_equal_symbol() {
			Assert.Equal(">=", CompareOperatorToSymbol.Get(CompareOperator.GreaterOrEqualThan));
		}

		[Fact]
		public void Should_return_less_or_equal_symbol() {
			Assert.Equal("<=", CompareOperatorToSymbol.Get(CompareOperator.LessOrEqualThan));
		}

        [Fact]
        public void Should_return_is_symbol() {
            Assert.Equal("is", CompareOperatorToSymbol.Get(CompareOperator.Is));
        }
	}
}
