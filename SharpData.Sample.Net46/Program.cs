using System;
using Oracle.ManagedDataAccess.Client;
using Sharp.Data;

namespace SharpData.Sample.Net46 {
    class Program {
        static void Main(string[] args) {
            var factory = new OracleClientFactory();
            using (var client =
                SharpFactory.Default.CreateDataClient(factory,
                    "Data Source=//localhost/XE;User Id=sharp;Password=sharp;")) {
                new Sample().Start(client);
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}