using SharpData;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace SharpData.Sample.Core {
    class Program {
        static void Main(string[] args) {
            var factory = new SharpFactory(SqlClientFactory.Instance, "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=sharp; Integrated Security=True; Encrypt=False; TrustServerCertificate=True; ApplicationIntent=ReadWrite;");
            using (var client = factory.CreateDataClient()) {
                new Example().Start(client);
            }
            Console.WriteLine("Done");
            Console.ReadLine();
        }
    }
}