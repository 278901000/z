using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using z.Foundation.Data;
using System.Configuration;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            DataTable dataTable = OracleHelper.ExecuteDataTable("PAPAGO", "SELECT * FROM T_IM_ITEM", System.Data.CommandType.Text, null);

            Console.WriteLine(dataTable.Rows.Count);
            Console.Read();
        }
    }
}
