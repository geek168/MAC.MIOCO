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
            var sql = @"INSERT INTO ItemMaster(ItemId,ItemName,ItemSize,ItemType,StockCount,StockPrice,Price,Id,UpdateTime,Color)
                        VALUES (@ItemId,@ItemName,@ItemSize,@ItemType,@StockCount,@StockPrice,@Price,@Id,@UpdateTime,@Color)";
            var ret = false;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                SqlCeTransaction tx = null;
                try
                {
                    conn.Open();
                    tx = conn.BeginTransaction();
                    SqlCeCommand command = conn.CreateCommand();
                    var parameters = new[]
                    {
                        new SqlCeParameter("ItemId", SqlDbType.NVarChar, 100) { Value = item.ItemId },
                        new SqlCeParameter("ItemName", SqlDbType.NVarChar, 250) { Value = item.ItemName },
                        new SqlCeParameter("ItemSize", SqlDbType.Int) { Value = item.ItemSize },
                        new SqlCeParameter("ItemType", SqlDbType.Int) { Value = item.ItemType },
                        new SqlCeParameter("StockCount", SqlDbType.Int) { Value = item.StockCount },
                        new SqlCeParameter("StockPrice", SqlDbType.Decimal) { Value = item.StockPrice },
                        new SqlCeParameter("Price", SqlDbType.Decimal) { Value = item.Price },
                        new SqlCeParameter("Id", SqlDbType.NVarChar, 50) { Value = item.Id },
                        new SqlCeParameter("UpdateTime", SqlDbType.DateTime) { Value = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") },
                        new SqlCeParameter ( "Color", SqlDbType.NVarChar, 50) { Value = item.Color ?? "" }
                    };
                    command.Parameters.AddRange(parameters);
                    command.CommandText = sql;
                    ret = command.ExecuteNonQuery() > 0;
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                }
                finally
                {
                    conn.Close();
                }
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
                           ,StockPrice = @StockPrice
                           ,Price = @Price
                           ,UpdateTime = @UpdateTime
                           ,Color = @Color
                        WHERE Id = @Id";
            var ret = false;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                SqlCeTransaction tx = null;
                try
                {
                    conn.Open();
                    tx = conn.BeginTransaction();
                    SqlCeCommand command = conn.CreateCommand();
                    var parameters = new[]
                    {
                        new SqlCeParameter("ItemId", SqlDbType.NVarChar, 100) { Value = item.ItemId },
                        new SqlCeParameter("ItemName", SqlDbType.NVarChar, 250) { Value = item.ItemName },
                        new SqlCeParameter("ItemSize", SqlDbType.Int) { Value = item.ItemSize },
                        new SqlCeParameter("ItemType", SqlDbType.Int) { Value = item.ItemType },
                        new SqlCeParameter("StockCount", SqlDbType.Int) { Value = item.StockCount },
                        new SqlCeParameter("StockPrice", SqlDbType.Decimal) { Value = item.StockPrice },
                        new SqlCeParameter("Price", SqlDbType.Decimal) { Value = item.Price },
                        new SqlCeParameter("Id", SqlDbType.NVarChar, 50) { Value = item.Id },
                        new SqlCeParameter("UpdateTime", SqlDbType.DateTime) { Value = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") },
                        new SqlCeParameter ( "Color", SqlDbType.NVarChar, 50) { Value = item.Color }
                    };
                    command.Parameters.AddRange(parameters);
                    command.CommandText = sql;
                    ret = command.ExecuteNonQuery() > 0;
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
            return ret;
        }

        public static bool DeleteItemMaster(ItemMaster item)
        {
            var sql = @"DELETE FROM ItemMaster WHERE Id = @Id";
            var ret = false;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                SqlCeTransaction tx = null;
                try
                {
                    conn.Open();
                    tx = conn.BeginTransaction();
                    SqlCeCommand command = conn.CreateCommand();
                    var parameters = new[]
                    {
                        new SqlCeParameter("Id", SqlDbType.NVarChar, 50) { Value = item.Id },
                    };
                    command.Parameters.AddRange(parameters);
                    command.CommandText = sql;
                    ret = command.ExecuteNonQuery() > 0;
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                }
                finally
                {
                    conn.Close();
                }
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
                SqlCeTransaction tx = null;
                try
                {
                    conn.Open();
                    tx = conn.BeginTransaction();
                    SqlCeCommand command = conn.CreateCommand();
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
                    command.CommandText = sql;
                    ret = command.ExecuteNonQuery() > 0;
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
            return ret;
        }

        public static bool UpdateCustomer(Customer item)
        {
            var sql = @"UPDATE Customer
                       SET Name = @Name
                          ,Id = @Id
                          ,Phone = @Phone
                          ,IM = @IM
                          ,Deposit = @Deposit
                          ,Remark = @Remark
                          ,Discount = @Discount
                          ,UpdateTime = @UpdateTime
                     WHERE Id = @Id";
            var ret = false;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                SqlCeTransaction tx = null;
                try
                {
                    conn.Open();
                    tx = conn.BeginTransaction();
                    SqlCeCommand command = conn.CreateCommand();
                    var parameters = new[]
                    {
                        new SqlCeParameter("Name", SqlDbType.NVarChar, 100) { Value = item.Name },
                        new SqlCeParameter("Id", SqlDbType.NVarChar, 50) { Value = item.Id },
                        new SqlCeParameter("Phone", SqlDbType.NVarChar, 100) { Value = item.Phone },
                        new SqlCeParameter("IM", SqlDbType.NVarChar, 150) { Value = item.IM },
                        new SqlCeParameter("Deposit", SqlDbType.Decimal) { Value = item.Deposit },
                        new SqlCeParameter("Remark", SqlDbType.NVarChar, 300) { Value = item.Remark },
                        new SqlCeParameter("Discount", SqlDbType.Int) { Value = item.Discount },
                        new SqlCeParameter("UpdateTime", SqlDbType.DateTime) { Value = item.UpdateTime.ToString("yyyy-MM-dd HH:mm:ss") }
                    };
                    command.Parameters.AddRange(parameters);
                    command.CommandText = sql;
                    ret = command.ExecuteNonQuery() > 0;
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
            return ret;
        }


        public static bool InsertItemSales(List<ItemSales> list)
        {
            var sql = @"INSERT INTO ItemSales(ItemSalesId,ItemMasterId,ItemName,CustomerId,SalesType,SalesCount,SoldPirce,UpdateTime)
                        VALUES (@ItemSalesId,@ItemMasterId,@ItemName,@CustomerId,@SalesType,@SalesCount,@SoldPirce,@UpdateTime)";

            var ret = false;
            using (SqlCeConnection conn = new SqlCeConnection(SQLCONN))
            {
                SqlCeTransaction tx = null;
                try
                {
                    conn.Open();
                    tx = conn.BeginTransaction();

                    var ItemSalesId = Guid.NewGuid().ToString();

                    list.ForEach(s =>
                    {
                        SqlCeCommand command = conn.CreateCommand();
                        var parameters = new[]
                        {
                            new SqlCeParameter("ItemSalesId", SqlDbType.NVarChar, 50) { Value = ItemSalesId },
                            new SqlCeParameter("ItemMasterId", SqlDbType.NVarChar, 50) { Value = s.Id },
                            new SqlCeParameter("ItemName", SqlDbType.NVarChar, 250) { Value = s.ItemName },
                            new SqlCeParameter("CustomerId", SqlDbType.NVarChar, 50) { Value = s.CustomerId ?? "" },
                            new SqlCeParameter("SalesType", SqlDbType.Int) { Value = s.SalesType},
                            new SqlCeParameter("SalesCount", SqlDbType.Int) { Value = s.SalesCount },
                            new SqlCeParameter("SoldPirce", SqlDbType.Decimal) { Value = s.SoldPirce },
                            new SqlCeParameter("UpdateTime", SqlDbType.DateTime) { Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                        };
                        command.Parameters.AddRange(parameters);
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    });

                    var cusid = list.FirstOrDefault().CustomerId;
                    if (!string.IsNullOrEmpty(cusid))
                    {
                        sql = "UPDATE Customer SET Deposit = @Deposit, UpdateTime = @UpdateTime WHERE Id = @Id";
                        var deposit = list.FirstOrDefault().DepositForUpdate - list.Sum(s => s.SoldPirce);
                        if (deposit < 0)
                        {
                            deposit = 0;
                        };
                        SqlCeCommand command = conn.CreateCommand();
                        var parameters = new[]
                        {
                            new SqlCeParameter("Deposit", SqlDbType.Decimal) { Value = deposit },
                            new SqlCeParameter("Id", SqlDbType.NVarChar, 50) { Value = cusid },
                            new SqlCeParameter("UpdateTime", SqlDbType.DateTime) { Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                        };
                        command.Parameters.AddRange(parameters);
                        command.CommandText = sql;
                        command.ExecuteNonQuery();

                        sql = @"INSERT INTO DepositDetail (CustomerId,Detail,ItemSalesId)
                                VALUES (@CustomerId,@Detail,@ItemSalesId)";
                        command = conn.CreateCommand();
                        parameters = new[]
                        {
                            new SqlCeParameter("CustomerId", SqlDbType.NVarChar, 50) { Value = cusid },
                            new SqlCeParameter("Detail", SqlDbType.NVarChar, 200) { Value = "消费---消费前还有 " + list.FirstOrDefault().DepositForUpdate + " 元，消费了 " + list.Sum(s => s.SoldPirce) + " 元，还剩 " + deposit + " 元" },
                            new SqlCeParameter("ItemSalesId", SqlDbType.NVarChar, 50) { Value = ItemSalesId },
                        };
                        command.Parameters.AddRange(parameters);
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }

                    sql = @"UPDATE ItemMaster SET StockCount = (StockCount - @SalesCount), UpdateTime = @UpdateTime WHERE Id = @Id";
                    list.ForEach(s =>
                    {
                        SqlCeCommand command = conn.CreateCommand();
                        var parameters = new[]
                        {
                            new SqlCeParameter("SalesCount", SqlDbType.Int) { Value = s.SalesCount },
                            new SqlCeParameter("Id", SqlDbType.NVarChar, 50) { Value = s.Id },
                            new SqlCeParameter("UpdateTime", SqlDbType.DateTime) { Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") },
                        };
                        command.Parameters.AddRange(parameters);
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    });

                    tx.Commit();
                    ret = true;
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                }
                finally
                {
                    conn.Close();
                }
            }
            return ret;
        }


    }
}
