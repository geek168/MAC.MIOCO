﻿using MAC.MIOCO.Command;
using MAC.MIOCO.Model;
using MAC.MIOCO.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace MAC.MIOCO.ViewModel
{
    public class CustomerViewModel : ViewModelBase, IDataErrorInfo
    {

        private string Id;

        private bool IsPhoneRepeat = false;

        private bool IsIMRepeat = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="windwow"></param>
        public CustomerViewModel(Window window)
        {
            InsertCommandVisibility = Visibility.Visible;
            UpdateCommandVisibility = Visibility.Collapsed;
            DepositIsReadOnly = false;

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

            UpdateCommand = new DelegateCommand(() =>
            {
                var item = new Customer()
                {
                    Id = Id,
                    Name = Name,
                    Phone = Phone,
                    IM = IM,
                    Discount = Discount,
                    Deposit = string.IsNullOrEmpty(Deposit) ? 0 : decimal.Parse(Deposit),
                    Remark = Remark,
                    UpdateTime = DateTime.Now
                };

                if (SqlServerCompactService.UpdateCustomer(item))
                {
                    InitialControlValue();
                    BindData();
                }
            }, CanExcute);

            SelectCommand = new DelegateCommand<Customer>(s =>
            {
                if (s != null)
                {
                    InsertCommandVisibility = Visibility.Collapsed;
                    UpdateCommandVisibility = Visibility.Visible;
                    DepositIsReadOnly = true;

                    Id = s.Id;
                    Name = s.Name;
                    Phone = s.Phone;
                    IM = s.IM;
                    Discount = s.Discount;
                    Deposit = s.Deposit.ToString();
                    Remark = s.Remark;

                    IsPhoneRepeat = false;
                    IsIMRepeat = false;
                }
            }); 

            AddDepositCommand = new DelegateCommand(() =>
            {
                PopupIsOpen = !PopupIsOpen;
            });

            UpdateDepositCommand = new DelegateCommand(() =>
            {
                decimal j = 0;
                decimal.TryParse(Deposit, out j);
                if (SqlServerCompactService.TopUpCustomerDeposit(Id, j))
                {
                    BindData();
                    PopupIsOpen = false;

                    InitialControlValue();
                }
            }, () => {
                var ret = false;
                decimal j = 0;
                if (!string.IsNullOrEmpty(Deposit) && decimal.TryParse(Deposit, out j))
                {
                    if (j > 0)
                    {
                        ret = true;
                    }
                }
                return ret;
            });


            DeleteCommand = new DelegateCommand<Customer>(s =>
            {
                if (s != null)
                {
                    if (MessageBox.Show(window, "是否确认删除该客户？", "确认删除点“Yes”，否则点“No”", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                    {
                        SqlServerCompactService.DeleteCustomer(s);
                        BindData();
                        InitialControlValue();
                    }
                }
            });

            ViewCommand = new DelegateCommand<Customer>(s =>
            {
                ViewCustomerDetailWindow w = new ViewCustomerDetailWindow();
                ViewCustomerDetailViewModel model = new ViewCustomerDetailViewModel(w, s);
                w.DataContext = model;
                w.Owner = App.Current.MainWindow;
                w.ShowDialog();
            });

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

            if (!string.IsNullOrEmpty(Name) && ((!string.IsNullOrEmpty(Phone) && !IsPhoneRepeat) || (!string.IsNullOrEmpty(IM) && !IsIMRepeat)))
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
            Id = "";
            Name = "";
            Phone = "";
            IM = "";
            Discount = 100;
            Deposit = "";
            Remark = "";

            InsertCommandVisibility = Visibility.Visible;
            UpdateCommandVisibility = Visibility.Collapsed;
            DepositIsReadOnly = false;
        }

        public DelegateCommand CloseCommand { get; private set; }

        public DelegateCommand InsertCommand { get; private set; }

        public DelegateCommand UpdateCommand { get; private set; }

        public DelegateCommand ClearCommand { get; private set; }

        public DelegateCommand<Customer> SelectCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand AllCommand { get; private set; }

        public DelegateCommand PreviousCommand { get; private set; }

        public DelegateCommand NextCommand { get; private set; }

        public DelegateCommand AddDepositCommand { get; private set; }

        public DelegateCommand UpdateDepositCommand { get; private set; }

        public DelegateCommand<Customer> DeleteCommand { get; private set; }

        public DelegateCommand<Customer> ViewCommand { get; private set; }


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

        private bool _DepositIsReadOnly = false;
        public bool DepositIsReadOnly
        {
            get { return _DepositIsReadOnly; }
            set
            {
                _DepositIsReadOnly = value;
                OnPropertyChanged(nameof(DepositIsReadOnly));

                if (value)
                {
                    DepositBackground = new SolidColorBrush(Colors.LightGray);
                    AddDepositCommandVisibility = Visibility.Visible;
                }
                else
                {
                    DepositBackground = new SolidColorBrush(Colors.White);
                    AddDepositCommandVisibility = Visibility.Collapsed;
                }
            }
        }

        private SolidColorBrush _DepositBackground = new SolidColorBrush(Colors.White);
        public SolidColorBrush DepositBackground
        {
            get { return _DepositBackground; }
            set
            {
                _DepositBackground = value;
                OnPropertyChanged(nameof(DepositBackground));
            }
        }

        private Visibility _AddDepositCommandVisibility = Visibility.Collapsed;
        public Visibility AddDepositCommandVisibility
        {
            get { return _AddDepositCommandVisibility; }
            set
            {
                _AddDepositCommandVisibility = value;
                OnPropertyChanged(nameof(AddDepositCommandVisibility));
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

                if (SOURCE.FirstOrDefault(s => string.Compare(s.Phone, value, true) == 0 && string.Compare(s.Id, Id, true) != 0) != null)
                {
                    IsPhoneRepeat = true;
                }
                else
                {
                    IsPhoneRepeat = false;
                }
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

                if (SOURCE.FirstOrDefault(s => string.Compare(s.IM, value, true) == 0 && string.Compare(s.Id, Id, true) != 0) != null)
                {
                    IsIMRepeat = true;
                }
                else
                {
                    IsIMRepeat = false;
                }
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

        private bool _PopupIsOpen;
        public bool PopupIsOpen
        {
            get { return _PopupIsOpen; }
            set
            {
                _PopupIsOpen = value;
                OnPropertyChanged(nameof(PopupIsOpen));
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
                    case nameof(Phone):
                        if (IsPhoneRepeat && !string.IsNullOrEmpty(Phone))
                        {
                            ret = " * 电话重复请检查！！！";
                        }
                        break;
                    case nameof(IM):
                        if (IsIMRepeat && !string.IsNullOrEmpty(IM))
                        {
                            ret = " * IM重复请检查！！！";
                        }
                        break;
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
