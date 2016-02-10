using Oracle.ManagedDataAccess.Client;
using Sharp.Data;
using Sharp.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharpData.Sample {
    public class Program {
        public static void Main(string[] args) {
            //update tb_medidor_ene set DT_HR_INSERCAO = :par0,DT_HR_INSTANTE
            // = :par1,vl_eneat_rec_cp = :par2 WHERE (((cd_medidor = :par3) And
            // (DT_HR_INSTANTE = :par4)) And((vl_eneat_rec_cp = :par5) Or
            //(vl_eneat_rec_cp is NULL)))

            var factory = new OracleClientFactory();
            var client = SharpFactory.Default.CreateDataClient(factory, "Data Source=//localhost:1521/XE;User Id=W2E_PIM;Password=W2E_PIM;");

            var data = DateTime.Today.AddMinutes(5);
            var colunas = new[] {
                "DT_HR_INSERCAO",
                "DT_HR_INSTANTE",
                "vl_eneat_rec_cp"
            };
            var valores = new object[] {
                DateTime.Now,
                data,
                10
            };

            var filter = GetFilter(1, data);
            filter = GetFilterValor(filter, 10);

            var count = client.Update
                  .Table("tb_medidor_ene")
                  .SetColumns(colunas)
                  .ToValues(valores)
                  .Where(filter);

            Console.WriteLine("Done");
        }

        private static Filter GetFilter(int id, DateTime data) {
            var filterId = Filter.Eq("cd_medidor", id);
            var filterData = Filter.Eq("DT_HR_INSTANTE", In.Par(data, System.Data.DbType.Date));
            return Filter.And(filterId, filterData);
        }

        private static Filter GetFilterValor(Filter filter, decimal valor) {
            var mesmoValor = Filter.Eq("vl_eneat_rec_cp", valor);
            var valorNull = Filter.Eq("vl_eneat_rec_cp", null);
            return Filter.And(filter, Filter.Or(mesmoValor, valorNull));
        }

        public static void X() {
            var factory = new OracleClientFactory();
            //factory.GetType().Namespace;
        }

    }
}
