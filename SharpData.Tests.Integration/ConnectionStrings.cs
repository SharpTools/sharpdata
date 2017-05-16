namespace SharpData.Tests.Integration {
    public static class ConnectionStrings {
        public static string Oracle = "Data Source=//localhost:1521/XE; User Id=sharp; Password=sharp;";
        public static string Mysql = "Server=localhost;Port=3306;Database=sharp;Uid=sharp;Pwd=sharp;";
        public static string Sqlite = "Data Source=sharp.db3;";
        public static string SqlServer = "Server=(localdb)\\mssqllocaldb;Database=sharp;Trusted_Connection=True;MultipleActiveResultSets=true";
        public static string Postgre = "User ID=sharp;Password=sharp;Host=localhost;Port=5432;Database=sharp;";
    }
}