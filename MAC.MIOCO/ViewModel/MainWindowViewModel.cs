using MAC.MIOCO.Command;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MAC.MIOCO.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
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

        public DelegateCommand ClickCommand { get; private set; }

        public void Execute()
        {
            var dt = SqlServerCompactService.GetData("UserAccount");
            var useraccount = from user in dt.AsEnumerable()
                              where user.Field<string>("UserName") == UserName && user.Field<string>("Password") == Password
                              select user;
            if (useraccount.ToList().Count == 0)
            {
                
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
        }


    }
}
