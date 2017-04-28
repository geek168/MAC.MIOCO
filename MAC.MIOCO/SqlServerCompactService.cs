using MAC.MIOCO.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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

        public static bool Insert(string sql)
        {
            var ret = false;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                conn.Open();
                SqlCeCommand command = new SqlCeCommand(sql, conn);
                ret = command.ExecuteNonQuery() > 0;
                conn.Close();
            }
            return ret;
        }

        public static bool InsertItemMaster(ItemMaster item)
        {
            var sql = @"INSERT INTO ItemMaster(ItemId,ItemName,ItemSize,ItemType,StockCount,SalesCount,StockPrice,Price,Id,UpdateTime,Color)
                        VALUES (@ItemId,@ItemName,@ItemSize,@ItemType,@StockCount,@SalesCount,@StockPrice,@Price,@Id,@UpdateTime,@Color)";
            var ret = false;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                conn.Open();
                SqlCeCommand command = new SqlCeCommand(sql, conn);
                var parameters = new[] 
                {
                    new SqlCeParameter("ItemId", SqlDbType.NVarChar, 100) { Value = item.ItemId },
                    new SqlCeParameter("ItemName", SqlDbType.NVarChar, 250) { Value = item.ItemName },
                    new SqlCeParameter("ItemSize", SqlDbType.Int) { Value = item.ItemSize },
                    new SqlCeParameter("ItemType", SqlDbType.Int) { Value = item.ItemType },
                    new SqlCeParameter("StockCount", SqlDbType.Int) { Value = item.StockCount },
                    new SqlCeParameter("SalesCount", SqlDbType.Int) { Value = item.SalesCount },
                    new SqlCeParameter("StockPrice", SqlDbType.Decimal) { Value = item.StockPrice },
                    new SqlCeParameter("Price", SqlDbType.Decimal) { Value = item.Price },
                    new SqlCeParameter("Id", SqlDbType.NVarChar, 50) { Value = item.Id },
                    new SqlCeParameter("UpdateTime", SqlDbType.DateTime) { Value = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") },
                    new SqlCeParameter ( "Color", SqlDbType.NVarChar, 50) { Value = item.Color ?? "" }
                };
                command.Parameters.AddRange(parameters);
                ret = command.ExecuteNonQuery() > 0;
                conn.Close();
            }
            return ret;
        }

        public static bool UpdateItemMaster(ItemMaster item)
        {
            var sql = @"UPDATE ItemMaster 
                        SET ItemId = @ItemId
                           ,ItemName = @ItemName
                           ,ItemSize = @ItemSize
                           ,ItemType = @ItemType
                           ,StockCount = @StockCount
                           ,SalesCount = @SalesCount
                           ,StockPrice = @StockPrice
                           ,Price = @Price
                           ,UpdateTime = @UpdateTime
                           ,Color = @Color
                        WHERE Id = @Id";
            var ret = false;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                conn.Open();
                SqlCeCommand command = new SqlCeCommand(sql, conn);
                var parameters = new[]
                {
                    new SqlCeParameter("ItemId", SqlDbType.NVarChar, 100) { Value = item.ItemId },
                    new SqlCeParameter("ItemName", SqlDbType.NVarChar, 250) { Value = item.ItemName },
                    new SqlCeParameter("ItemSize", SqlDbType.Int) { Value = item.ItemSize },
                    new SqlCeParameter("ItemType", SqlDbType.Int) { Value = item.ItemType },
                    new SqlCeParameter("StockCount", SqlDbType.Int) { Value = item.StockCount },
                    new SqlCeParameter("SalesCount", SqlDbType.Int) { Value = item.SalesCount },
                    new SqlCeParameter("StockPrice", SqlDbType.Decimal) { Value = item.StockPrice },
                    new SqlCeParameter("Price", SqlDbType.Decimal) { Value = item.Price },
                    new SqlCeParameter("Id", SqlDbType.NVarChar, 50) { Value = item.Id },
                    new SqlCeParameter("UpdateTime", SqlDbType.DateTime) { Value = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") },
                    new SqlCeParameter ( "Color", SqlDbType.NVarChar, 50) { Value = item.Color }
                };
                command.Parameters.AddRange(parameters);
                ret = command.ExecuteNonQuery() > 0;
                conn.Close();
            }
            return ret;
        }

        public static bool DeleteItemMaster(ItemMaster item)
        {
            var sql = @"DELETE FROM ItemMaster WHERE Id = @Id";
            var ret = false;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                conn.Open();
                SqlCeCommand command = new SqlCeCommand(sql, conn);
                var parameters = new[]
                {
                    new SqlCeParameter("Id", SqlDbType.NVarChar, 50) { Value = item.Id },
                };
                command.Parameters.AddRange(parameters);
                ret = command.ExecuteNonQuery() > 0;
                conn.Close();
            }
            return ret;
        }

        public static bool InsertCustomer(Customer customer)
        {
            var sql = @"INSERT INTO Customer(Name,Id,Phone,IM,Deposit,Remark,Discount,UpdateTime)
                        VALUES (@Name,@Id,@Phone,@IM,@Deposit,@Remark,@Discount,@UpdateTime)";
            var ret = false;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                conn.Open();
                SqlCeCommand command = new SqlCeCommand(sql, conn);
                var parameters = new[]
                {
                    new SqlCeParameter("Name", SqlDbType.NVarChar, 100) { Value = customer.Name },
                    new SqlCeParameter("Id", SqlDbType.NVarChar, 50) { Value = customer.Id },
                    new SqlCeParameter("Phone", SqlDbType.NVarChar, 100) { Value = customer.Phone ?? ""},
                    new SqlCeParameter("IM", SqlDbType.NVarChar, 150) { Value = customer.IM ?? "" },
                    new SqlCeParameter("Deposit", SqlDbType.Decimal) { Value = customer.Deposit },
                    new SqlCeParameter("Remark", SqlDbType.NVarChar, 300) { Value = customer.Remark ?? "" },
                    new SqlCeParameter("Discount", SqlDbType.Int) { Value = customer.Discount },
                    new SqlCeParameter("UpdateTime", SqlDbType.DateTime) { Value = customer.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") },
                };
                command.Parameters.AddRange(parameters);
                ret = command.ExecuteNonQuery() > 0;
                conn.Close();
            }
            return ret;
        }

    }
}
