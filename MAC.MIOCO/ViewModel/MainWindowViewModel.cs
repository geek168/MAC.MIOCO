using MAC.MIOCO.Command;
using MAC.MIOCO.View;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MAC.MIOCO.ViewModel
{
    public class MainWindowViewModel : ViewModelBase, IDataErrorInfo
    {
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set
            {
                _UserName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string _Password;
        public string Password
        {
            get { return _Password; }
            set
            {
                _Password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private Visibility _LoginVisibility = Visibility.Visible;
        public Visibility LoginVisibility
        {
            get { return _LoginVisibility; }
            set
            {
                _LoginVisibility = value;
                OnPropertyChanged(nameof(LoginVisibility));
            }
        }

        private Visibility _LoginErrorVisibility = Visibility.Collapsed;
        public Visibility LoginErrorVisibility
        {
            get { return _LoginErrorVisibility; }
            set
            {
                _LoginErrorVisibility = value;
                OnPropertyChanged(nameof(LoginErrorVisibility));
            }
        }

        public DelegateCommand ClickCommand { get; private set; }

        public void Execute()
        {
            var dt = SqlServerCompactService.GetData("UserAccount");
            var useraccount = from user in dt.AsEnumerable()
                              where user.Field<string>("UserName") == UserName && user.Field<string>("Password") == Password
                              select user;
            if (useraccount.ToList().Count == 0)
            {
                UserName = "Error";
            }
            else
            {
                SalesWindow window = new SalesWindow();
                SalesWindowViewModel model = new SalesWindowViewModel(window);
                model.AfterLogoutHandler += (s, e) => { UserName = ""; Password = ""; };
                window.DataContext = model;
                window.Owner = Application.Current.MainWindow;
                window.ShowDialog();
            }
        }

        public bool CanExecute()
        {
            if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public MainWindowViewModel()
        {
            ClickCommand = new DelegateCommand(Execute, CanExecute);

            var relativepath = ConfigurationManager.ConnectionStrings["FilePath"].ConnectionString;
            var backuprelativepath = ConfigurationManager.ConnectionStrings["BackupFilePath"].ConnectionString;
            var path = Path.Combine(Environment.CurrentDirectory, "Data");
            var backuppath = Path.Combine(Environment.CurrentDirectory, "BackupData");
            var file = relativepath.Replace("|DataDirectory|", path);
            var backupfile = relativepath.Replace("|DataDirectory|", backuppath);
            if (!File.Exists(file))
            {
                LoginVisibility = Visibility.Collapsed;
                LoginErrorVisibility = Visibility.Visible;
            }
            //else
            //{
            //    File.Copy(file, backupfile, true);
            //    LoginVisibility = Visibility.Visible;
            //    LoginErrorVisibility = Visibility.Collapsed;
            //}
        }


        #region IDataErrorInfo

        public string Error
        {
            get
            {
                return null;
            }
        }

        public string this[string columnName]
        {
            get
            {
                var ret = string.Empty;

                switch (columnName)
                {
                    case nameof(UserName):
                        if (UserName == "Error")
                        {
                            ret = " * 账号密码错误！！！";
                            UserName = "";
                            Password = "";
                        }
                        break;
                }

                return ret;
            }
        }

        #endregion

    }
}
