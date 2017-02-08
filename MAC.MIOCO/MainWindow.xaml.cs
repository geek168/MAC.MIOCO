using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace MAC.MIOCO
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private static string PATH = Path.Combine(Environment.CurrentDirectory, "MIOCO.mdf");
        private string SQLCONN = @"Data Source=(localdb)\v11.0;AttachDbFileName=" + PATH + ";Integrated Security=True;TimeOut=5;Initial Catalog=MIOCO";

        public MainWindow()
        {
            InitializeComponent();


     
            //SqlConnection conn = new SqlConnection(sqlconn);
            //conn.Open();
            //SqlCommand command = conn.CreateCommand();
            //command.CommandType = System.Data.CommandType.Text;
            //command.CommandText = "INSERT INTO Customer VALUES ('Test','15112341234','',0,100)";
            //command.ExecuteNonQuery();
            //conn.Close();

            using (SqlConnection connection = new SqlConnection(SQLCONN))
            {
                string query = "SELECT * FROM Customer";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        this.Name.Content = reader["Name"].ToString();
                    }
                }
                connection.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrEmpty(this.Input.Text))
            {
                MessageBox.Show("请先输入任意文字！");
                return;
            }

            Random rd = new Random();
            var rdn = rd.Next();
            using (SqlConnection connection = new SqlConnection(SQLCONN))
            {
                string query = "INSERT INTO Customer VALUES (N'" + this.Input.Text + "',{0},'',0,100)";
                query = string.Format(query, rdn);
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        this.Name.Content = reader["Name"].ToString();
                    }
                }
                connection.Close();
            }

            MessageBox.Show("哈哈，你真傻，叫你干啥就干啥！");

            var ss = "";
            var ph = "";
            using (SqlConnection connection = new SqlConnection(SQLCONN))
            {
                string query = "SELECT * FROM Customer WHERE Name = '" + this.Input.Text + "'";
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ss = reader["Name"].ToString();
                        ph = reader["Phone"].ToString();
                    }
                }
                connection.Close();
            }

            
            //Encoding tc = Encoding.GetEncoding("UNICOD");
            //byte[] bytes = tc.GetBytes(ss);
            //var value = Encoding.Unicode.GetString(bytes);
            MessageBox.Show("你刚才输入的是：" + this.Input.Text + "，电话是：" + ph);
        }
    }
}
