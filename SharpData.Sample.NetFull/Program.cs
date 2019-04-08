using System;
using Oracle.ManagedDataAccess.Client;
using SharpData;

namespace SharpData.Sample.NetFull {
    class Program {
        static void Main(string[] args) {
            SharpFactory.Default = new SharpFactory(new OracleClientFactory(), "Data Source=//localhost/XE;User Id=sharp;Password=sharp;");

            using (var client =
                SharpFactory.Default.CreateDataClient()) {
                new Sample().Start(client);
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}