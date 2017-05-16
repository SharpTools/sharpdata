using Xunit;

namespace Sharp.Tests.Databases {
    public static class AssertSql {

        public static void AreEqual(string sql1, string sql2) {
            sql1 = sql1.Trim().Replace("\r\n", "").Replace(", ",",").ToUpper();
            sql2 = sql2.Trim().Replace("\r\n", "").Replace(", ", ",").ToUpper();
            Assert.Equal(sql1,sql2);
        }

		public static void AreEqual(string[] sqls1, string[] sqls2) {
			Assert.True(sqls1.Length == sqls2.Length, $"Sizes doesn't match: {sqls1.Length} != {sqls2.Length}");
			for (var i = 0; i < sqls1.Length; i++) {
				AreEqual(sqls1[i],sqls2[2]);
			}
		}
    }
}