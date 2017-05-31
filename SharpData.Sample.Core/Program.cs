using System;
using System.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SharpData.Log;

namespace SharpData.Sample.Core {
    class Program {
        static void Main(string[] args) {
            SharpDataLogging.LoggerFactory = new LoggerFactory().AddConsole(LogLevel.Debug);
            
            var factory = new SharpFactory(SqlClientFactory.Instance, "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=sharp; Integrated Security=True; Encrypt=False; TrustServerCertificate=True; ApplicationIntent=ReadWrite;");
            using (var client = factory.CreateDataClient()) {
                new Example().Start(client);
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}