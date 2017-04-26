using MAC.MIOCO.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
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


        //private static string SQLCONN = ConfigurationManager.ConnectionStrings["CONN"].ConnectionString;


        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();

            //    using (SqlCeConnection connection = new SqlCeConnection(SQLCONN))
            //    {
            //        string query = "SELECT * FROM Customer";
            //        SqlCeCommand command = new SqlCeCommand(query, connection);
            //        connection.Open();
            //        using (SqlCeDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                //this.Name.Content = reader["Name"].ToString();
            //            }
            //        }
            //        connection.Close();
            //    }
            //}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtUserName.Focus();
        }
    }
}
