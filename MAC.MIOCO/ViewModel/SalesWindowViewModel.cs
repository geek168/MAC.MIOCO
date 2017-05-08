using MAC.MIOCO;
using MAC.MIOCO.Command;
using MAC.MIOCO.Model;
using MAC.MIOCO.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;

namespace MAC.MIOCO.ViewModel
{
    public class SalesWindowViewModel : ViewModelBase
    {
        private int PAGESIZE = 13;
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


        private ObservableCollection<ItemSales> SOURCE = new ObservableCollection<ItemSales>();
        private ItemSales SelectedItemSales;
        private string CustomerId;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public SalesWindowViewModel(Window window)
        {
            //ItemSalesColletion.CollectionChanged += (s, e) =>
            //{
            //    if (e.Action != System.Collections.Specialized.NotifyCollectionChangedAction.Reset)
            //    {
            //        decimal amount = 0;
            //        foreach (ItemSales item in e.NewItems)
            //        {
            //            item.PropertyChanged += (ss, ee) =>
            //            {
            //                if (ee.PropertyName == "SoldPirce")
            //                {
            //                    var sss = (ItemSales)ss;
            //                }
            //            };
            //            amount += item.SoldPirce;
            //            MessageBox.Show(amount.ToString());
            //        }
            //    }
            //};

            SalesDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            Timer timer = new Timer(1000);
            timer.Elapsed += (s, e) => { SalesDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"); };
            timer.Start();

            WINDOW = window;
            LogoutCommand = new DelegateCommand(Execute, () => { return true; });
            CloseCommand = new DelegateCommand(() => { Application.Current.Shutdown(); });

            SelectItemMasterCommand = new DelegateCommand(() =>
            {
                SelectItemMasterWindow w = new SelectItemMasterWindow();
                SelectItemMasterViewModel model = new SelectItemMasterViewModel(w);
                w.DataContext = model;
                w.Owner = App.Current.MainWindow;
                w.ShowDialog();

                //SOURCE = new ObservableCollection<ItemSales>(model.CheckedList);
                SOURCE.Clear();
                model.CheckedList.ForEach(s =>
                {
                    SOURCE.Add(new ItemSales
                    {
                        Id = s.Id,
                        ItemId = s.ItemId,
                        ItemName = s.ItemName,
                        ItemSize = s.ItemSize,
                        ItemType = s.ItemType,
                        StockCount = s.StockCount,
                        SalesType = 1,
                        SalesCount = 1,
                        StockPrice = s.StockPrice,
                        Price = s.Price,
                        SoldPirce = s.Price
                    });
                });
                //ItemSalesColletion = new ObservableCollection<ItemSales>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
                ItemSalesColletion.Clear();
                SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE).ToList().ForEach(ItemSalesColletion.Add);
            });

            SelectCustomerCommand = new DelegateCommand(() =>
            {
                SelectCustomerWindow w = new SelectCustomerWindow();
                SelectCustomerViewModel model = new SelectCustomerViewModel(w);
                w.DataContext = model;
                w.Owner = App.Current.MainWindow;
                w.ShowDialog();

                if (model.CheckedList != null)
                {
                    CustomerId = model.CheckedList.Id;
                    CustomerName = model.CheckedList.Name;
                    Phone = model.CheckedList.Phone;
                    IM = model.CheckedList.IM;
                    Deposit = model.CheckedList.Deposit;
                    DiscountRate = model.CheckedList.Discount;

                    ItemSalesColletion.ToList().ForEach(s =>
                    {
                        s.SoldPirce = s.SoldPirce * DiscountRate / 100;
                    });

                    if (SelectedItemSales != null)
                    {
                        SoldPirce = SelectedItemSales.SoldPirce;
                    }
                }
            });

            ClearCustomerCommand = new DelegateCommand(() =>
            {
                CustomerId = "";
                CustomerName = "";
                Phone = "";
                IM = "";
                Deposit = 0;

                ItemSalesColletion.ToList().ForEach(s =>
                {
                    s.SoldPirce = s.Price * s.SalesCount;
                });

                if (SelectedItemSales != null)
                {
                    SoldPirce = SelectedItemSales.Price * Count;
                }

                DiscountRate = 100;
            });

            SelectCommand = new DelegateCommand<ItemSales>(s =>
            {
                if (s != null)
                {
                    SelectedItemSales = s;

                    ItemId = s.ItemId;
                    ItemName = s.ItemName;
                    ItemSize = s.ItemSize;
                    ItemType = s.ItemType;
                    StockPrice = s.StockPrice;
                    Price = s.Price;
                    Count = s.SalesCount;
                    SliderMaximum = s.StockCount;
                    SoldPirce = s.SoldPirce;
                }
            });

            RemoveCommand = new DelegateCommand<ItemSales>(s =>
            {
                SOURCE.Remove(s);
                ItemSalesColletion = new ObservableCollection<ItemSales>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
                ItemId = "";
                ItemName = "";
                ItemSize = 0;
                ItemType = 0;
                StockPrice = 0;
                Price = 0;
                Count = 1;
            });

            //ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));

            PreviousCommand = new DelegateCommand(() =>
            {
                PageIndex--;
                ItemSalesColletion = new ObservableCollection<ItemSales>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            },()=> { return PageIndex > 0 ? true : false; });

            NextCommand = new DelegateCommand(() =>
            {
                PageIndex++;
                ItemSalesColletion = new ObservableCollection<ItemSales>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            },()=> { return (PageIndex + 1) * PAGESIZE < SOURCE.Count() ? true : false; });

            SalesItemCommand = new DelegateCommand(() =>
            {
                SalesItemWindow w = new SalesItemWindow();
                SalesItemViewModel model = new SalesItemViewModel(w);
                w.DataContext = model;
                w.Owner = App.Current.MainWindow;
                w.ShowDialog();
            });

            CustomerCommand = new DelegateCommand(() =>
            {
                CustomerWindow w = new CustomerWindow();
                CustomerViewModel model = new CustomerViewModel(w);
                w.DataContext = model;
                w.Owner = App.Current.MainWindow;
                w.ShowDialog();
            }, () => { return string.IsNullOrEmpty(CustomerName); });

            StockCommand = new DelegateCommand(() =>
            {
                StockWindow w = new StockWindow();
                StockViewModel model = new StockViewModel(w);
                w.DataContext = model;
                w.Owner = App.Current.MainWindow;
                w.ShowDialog();
            }, () => { return ItemSalesColletion == null || ItemSalesColletion.Count == 0; });

            VoidCommand = new DelegateCommand(() =>
            {
                VoidWindow w = new VoidWindow();
                VoidWindowViewModel model = new VoidWindowViewModel(w);
                w.DataContext = model;
                w.Owner = App.Current.MainWindow;
                w.ShowDialog();
            });

            SoldCommand = new DelegateCommand(() =>
            {
                StringBuilder message = new StringBuilder();
                if(!string.IsNullOrEmpty(CustomerName))
                {
                    var ned = Deposit - ItemSalesColletion.Sum(s => s.SoldPirce);
                    if (ned < 0)
                    {
                        message.Append("注意！！！");
                        message.Append(Environment.NewLine);
                        message.Append("客户余额不足，还差：" + ned + "元");
                        message.Append(Environment.NewLine);
                    }
                }
                message.Append("是否确认售出？");
                if (MessageBox.Show(window, message.ToString(), "确认售出点“Yes”，否则点“No”", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    message.Clear();
                    message.Append("本次共计售出：" + ItemSalesColletion.Sum(s => s.SalesCount) + "件，赚得：" + (ItemSalesColletion.Sum(s => s.SoldPirce) - ItemSalesColletion.Sum(s => s.StockPrice * s.SalesCount)) + "元！");
                    MessageBox.Show(message.ToString());

                    if (!string.IsNullOrEmpty(CustomerId))
                    {
                        ItemSalesColletion.ToList().ForEach(s => { s.CustomerId = CustomerId; s.DepositForUpdate = Deposit; });
                    }

                    SqlServerCompactService.InsertItemSales(ItemSalesColletion.ToList());
                }

            }, () => { return ItemSalesColletion != null && ItemSalesColletion.Count > 0; });
        }

        public void Execute()
        {
            WINDOW.Close();
            AfterLogoutHandler(null, null);
        }

        private void ClearItem()
        {
            ItemId = "";
            ItemName = "";
            ItemSize = 0;
            ItemType = 0;
            StockPrice = 0;
            Price = 0;
            Count = 0;
        }

        public DelegateCommand LogoutCommand { get; private set; }
        public DelegateCommand CloseCommand { get; private set; }

        public DelegateCommand SelectItemMasterCommand { get; private set; }

        public DelegateCommand SelectCustomerCommand { get; private set; }

        public DelegateCommand ClearCustomerCommand { get; private set; }

        public DelegateCommand<ItemSales> SelectCommand { get; private set; }

        public DelegateCommand<ItemSales> RemoveCommand { get; private set; }

        public DelegateCommand PreviousCommand { get; private set; }

        public DelegateCommand NextCommand { get; private set; }

        public DelegateCommand SoldCommand { get; private set; }

        public DelegateCommand SalesItemCommand { get; private set; }

        public DelegateCommand CustomerCommand { get; private set; }

        public DelegateCommand StockCommand { get; private set; }

        public DelegateCommand VoidCommand { get; private set; }

        Window WINDOW;
        public event EventHandler AfterLogoutHandler;

        #region Binding

        private string _SalesDate;
        public string SalesDate
        {
            get { return _SalesDate; }
            set
            {
                _SalesDate = value;
                OnPropertyChanged(nameof(SalesDate));
            }
        }

        private ObservableCollection<ItemSales> _ItemSalesColletion = new ObservableCollection<ItemSales>();
        public ObservableCollection<ItemSales> ItemSalesColletion
        {
            get { return _ItemSalesColletion; }
            set
            {
                _ItemSalesColletion = value;
                OnPropertyChanged(nameof(ItemSalesColletion));
            }
        }

        private string _ItemId;
        public string ItemId
        {
            get { return _ItemId; }
            set
            {
                _ItemId = value;
                OnPropertyChanged(nameof(ItemId));
            }
        }

        private string _ItemName;
        public string ItemName
        {
            get { return _ItemName; }
            set
            {
                _ItemName = value;
                OnPropertyChanged(nameof(ItemName));
            }
        }

        private int _ItemSize = 3;
        public int ItemSize
        {
            get { return _ItemSize; }
            set
            {
                _ItemSize = value;
                OnPropertyChanged(nameof(ItemSize));
            }
        }

        private int _ItemType;
        public int ItemType
        {
            get { return _ItemType; }
            set
            {
                _ItemType = value;
                OnPropertyChanged(nameof(ItemType));
            }
        }

        private decimal _StockPrice;
        public decimal StockPrice
        {
            get { return _StockPrice; }
            set
            {
                _StockPrice = value;
                OnPropertyChanged(nameof(StockPrice));
            }
        }

        private decimal _Price;
        public decimal Price
        {
            get { return _Price; }
            set
            {
                _Price = value;
                OnPropertyChanged(nameof(Price));

                SoldPirce = value * DiscountRate / 100;
            }
        }

        private int _Count;
        public int Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                OnPropertyChanged(nameof(Count));

                SoldPirce = value * Price * DiscountRate / 100;
                if (SelectedItemSales != null)
                {
                    SelectedItemSales.SalesCount = value;
                }
            }
        }

        private decimal _SoldPirce;
        public decimal SoldPirce
        {
            get { return _SoldPirce; }
            set
            {
                _SoldPirce = value;
                OnPropertyChanged(nameof(SoldPirce));

                if (SelectedItemSales != null)
                {
                    SelectedItemSales.SoldPirce = value;
                }
            }
        }

        private int _SliderMaximum;
        public int SliderMaximum
        {
            get { return _SliderMaximum; }
            set
            {
                _SliderMaximum = value;
                OnPropertyChanged(nameof(SliderMaximum));
            }
        }

        private string _CustomerName;
        public string CustomerName
        {
            get { return _CustomerName; }
            set
            {
                _CustomerName = value;
                OnPropertyChanged(nameof(CustomerName));
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

        private decimal _Deposit;
        public decimal Deposit
        {
            get { return _Deposit; }
            set
            {
                _Deposit = value;
                OnPropertyChanged(nameof(Deposit));
            }
        }


        private int _DiscountRate = 100;
        public int DiscountRate
        {
            get { return _DiscountRate; }
            set
            {
                _DiscountRate = value;
                OnPropertyChanged(nameof(DiscountRate));
            }
        }

        #endregion
    }
}
