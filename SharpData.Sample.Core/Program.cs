using Sharp.Data;
using System;
using System.Data.Common;
using System.Data.SqlClient;

namespace SharpData.Sample.Core {
    class Program {
        static void Main(string[] args) {
            var factory = SqlClientFactory.Instance;
            using (var client = SharpFactory.Default.CreateDataClient(factory, "Data Source=(localdb)\\MSSQLLocalDB; Initial Catalog=sharp; Integrated Security=True; Encrypt=False; TrustServerCertificate=True; ApplicationIntent=ReadWrite;")) {
                new Example().Start(client);
            }
        }
    }
}