using MAC.MIOCO.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MAC.MIOCO
{
    public class SqlServerCompactService
    {
        public static readonly string SQLCONN;

        static SqlServerCompactService()
        {
            var relativepath = ConfigurationManager.ConnectionStrings["CONN"].ConnectionString;
            string path = Path.Combine(Environment.CurrentDirectory, "Data");
            SQLCONN = relativepath.Replace("|DataDirectory|", path);
        }

        public static DataTable GetData(string tableName)
        {
            //MessageBox.Show(SQLCONN);

            DataTable dt = new DataTable();
            var sql = "SELECT * FROM " + tableName;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                conn.Open();
                SqlCeCommand command = new SqlCeCommand(sql, conn);
                using (SqlCeDataReader dataReader = command.ExecuteReader())
                {
                    dt.Load(dataReader);
                }
                conn.Close();
            }
            return dt;
        }

    }
}
