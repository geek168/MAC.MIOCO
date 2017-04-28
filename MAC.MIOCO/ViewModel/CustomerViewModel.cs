using MAC.MIOCO.Command;
using MAC.MIOCO.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MAC.MIOCO.ViewModel
{
    public class CustomerViewModel : ViewModelBase, IDataErrorInfo
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windwow"></param>
        public CustomerViewModel(Window window)
        {
            InsertCommandVisibility = Visibility.Visible;
            UpdateCommandVisibility = Visibility.Collapsed;

            BindData();

            CloseCommand = new DelegateCommand(() =>
            {
                window.Close();
            });

            InsertCommand = new DelegateCommand(() =>
            {
                var customer = new Customer()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Name,
                    Phone = Phone,
                    IM = IM,
                    Deposit = string.IsNullOrEmpty(Deposit)? 0: decimal.Parse(Deposit),
                    Discount = Discount,
                    Remark = Remark,
                    UpdateTime = DateTime.Now
                };

                if (SqlServerCompactService.InsertCustomer(customer))
                {
                    InitialControlValue();
                    BindData();
                }

            }, CanExcute);


            ClearCommand = new DelegateCommand(() =>
            {
                InitialControlValue();
            });

            PreviousCommand = new DelegateCommand(() =>
            {
                PageIndex--;
                CustomerColletion = new ObservableCollection<Customer>(SOURCE.Where(t => t.Name.IndexOf(SearchName, StringComparison.OrdinalIgnoreCase) >= 0).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return PageIndex > 0 ? true : false; });

            NextCommand = new DelegateCommand(() =>
            {
                PageIndex++;
                CustomerColletion = new ObservableCollection<Customer>(SOURCE.Where(t => t.Name.IndexOf(SearchName, StringComparison.OrdinalIgnoreCase) >= 0).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return (PageIndex + 1) * PAGESIZE < SOURCE.Where(t => t.Name.IndexOf(SearchName, StringComparison.OrdinalIgnoreCase) >= 0).Count() ? true : false; });

            SearchCommand = new DelegateCommand(() =>
            {
                var s = SOURCE.Where(t => t.Name.IndexOf(SearchName, StringComparison.OrdinalIgnoreCase) >= 0);
                PageIndex = 0;
                CustomerColletion = new ObservableCollection<Customer>(s.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return string.IsNullOrEmpty(SearchName) ? false : true; });

            AllCommand = new DelegateCommand(() =>
            {
                SearchName = "";
                CustomerColletion = new ObservableCollection<Customer>(SOURCE.Where(t => t.Name.IndexOf(SearchName, StringComparison.OrdinalIgnoreCase) >= 0).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return string.IsNullOrEmpty(SearchName) ? false : true; });
        }

        private void BindData()
        {
            SOURCE = new ObservableCollection<Customer>(SqlServerCompactService.GetData("Customer").Cast<Customer>());
            SOURCE = new ObservableCollection<Customer>(SOURCE.OrderByDescending(s => s.UpdateTime));
            CustomerColletion = new ObservableCollection<Customer>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
        }

        private bool CanExcute()
        {
            var ret = false;
            decimal j = 0;

            if (!string.IsNullOrEmpty(Name) && (!string.IsNullOrEmpty(Phone) || !string.IsNullOrEmpty(IM)))
            {
                ret = true;
            }
            if(!string.IsNullOrEmpty(Deposit) && !decimal.TryParse(Deposit, out j))
            {
                ret = false;
            }

            return ret;
        }

        private void InitialControlValue()
        {
            Name = "";
            Phone = "";
            IM = "";
            Discount = 100;
            Deposit = "";
            Remark = "";
        }

        public DelegateCommand CloseCommand { get; private set; }

        public DelegateCommand InsertCommand { get; private set; }

        public DelegateCommand UpdateCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand AllCommand { get; private set; }

        public DelegateCommand PreviousCommand { get; private set; }

        public DelegateCommand NextCommand { get; private set; }


        private Visibility _InsertCommandVisibility = Visibility.Visible;
        public Visibility InsertCommandVisibility
        {
            get { return _InsertCommandVisibility; }
            set
            {
                _InsertCommandVisibility = value;
                OnPropertyChanged(nameof(InsertCommandVisibility));
            }
        }

        private Visibility _UpdateCommandVisibility = Visibility.Collapsed;
        public Visibility UpdateCommandVisibility
        {
            get { return _UpdateCommandVisibility; }
            set
            {
                _UpdateCommandVisibility = value;
                OnPropertyChanged(nameof(UpdateCommandVisibility));
            }
        }


        private int PAGESIZE = 14;
        private int _PageIndex = 0;
        public int PageIndex
        {
            get { return _PageIndex; }
            set
            {
                if (value >= 0)
                {
                    _PageIndex = value;
                }
            }
        }

        private ObservableCollection<Customer> _CustomerColletion;
        public ObservableCollection<Customer> CustomerColletion
        {
            get { return _CustomerColletion; }
            set
            {
                _CustomerColletion = value;
                OnPropertyChanged(nameof(CustomerColletion));
            }
        }

        private ObservableCollection<Customer> SOURCE = new ObservableCollection<Customer>();

        private string _SearchName = "";
        public string SearchName
        {
            get { return _SearchName; }
            set
            {
                _SearchName = value;
                OnPropertyChanged(nameof(SearchName));
            }
        }

        private string _Name;
        public string Name
        {
            get { return _Name; }
            set
            {
                _Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        private string _Phone;
        public string Phone
        {
            get { return _Phone; }
            set
            {
                _Phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        private string _IM;
        public string IM
        {
            get { return _IM; }
            set
            {
                _IM = value;
                OnPropertyChanged(nameof(IM));
            }
        }

        private int _Discount = 100;
        public int Discount
        {
            get { return _Discount; }
            set
            {
                _Discount = value;
                OnPropertyChanged(nameof(Discount));
            }
        }

        private string _Deposit;
        public string Deposit
        {
            get { return _Deposit; }
            set
            {
                _Deposit = value;
                OnPropertyChanged(nameof(Deposit));
            }
        }

        private string _Remark;
        public string Remark
        {
            get { return _Remark; }
            set
            {
                _Remark = value;
                OnPropertyChanged(nameof(Remark));
            }
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
                var i = 0;
                decimal j = 0;
                switch (columnName)
                {
                    //case nameof(ItemId):
                    //    if (IsRepeat && !string.IsNullOrEmpty(ItemId))
                    //    {
                    //        ret = " * 商品编号重复请检查！！！";
                    //    }
                    //    break;
                    //case nameof(ItemSize):
                    //    if (!string.IsNullOrEmpty(ItemSize) && !int.TryParse(ItemSize, out i))
                    //    {
                    //        ret = " * 请输入整数！！！";
                    //    }
                    //    break;
                    case nameof(Deposit):
                        if (!string.IsNullOrEmpty(Deposit) && !decimal.TryParse(Deposit, out j))
                        {
                            ret = " * 请输入整数！！！";
                        }
                        break;
                }
                return ret;
            }
        }


        #endregion
    }
}
