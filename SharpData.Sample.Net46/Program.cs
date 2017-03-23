using Oracle.ManagedDataAccess.Client;
using Sharp.Data;
using Sharp.Data.Filters;
using Sharp.Data.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Sharp.Data.Schema.Column;

namespace SharpData.Sample.Net46 {
    partial class Program {
        static void Main(string[] args) {
            var factory = new OracleClientFactory();
            using (var client = SharpFactory.Default.CreateDataClient(factory, "Data Source=//localhost/XE;User Id=sharp;Password=sharp;")) {
                new Sample().Start(client);
            }
        }
    }
}