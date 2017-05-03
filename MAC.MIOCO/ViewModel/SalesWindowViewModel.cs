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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public SalesWindowViewModel(Window window)
        {
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
                ItemSalesColletion = new ObservableCollection<ItemSales>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            });

            SelectCommand = new DelegateCommand<ItemSales>(s =>
            {
                if (s != null)
                {
                    //if (s.StockCount == 0)
                    //{
                    //    ClearItem();
                    //}
                    //else
                    //{
                        ItemId = s.ItemId;
                        ItemName = s.ItemName;
                        ItemSize = s.ItemSize;
                        ItemType = s.ItemType;
                        StockPrice = s.StockPrice;
                        Price = s.Price;
                        Count = s.SalesCount;
                        SliderMaximum = s.StockCount;

                        SelectedItemSales = s;
                    //}
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
            });

            StockCommand = new DelegateCommand(() =>
            {
                StockWindow w = new StockWindow();
                StockViewModel model = new StockViewModel(w);
                w.DataContext = model;
                w.Owner =  App.Current.MainWindow;
                w.ShowDialog();
            });

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
                message.Append("本次共计售出：" + Count + "件，赚得：" + (SoldPirce - (StockPrice * Count)) + "元！");
                message.Append(Environment.NewLine);
                message.Append("是否确认售出？");
                if (MessageBox.Show(window ,message.ToString(), "确认售出点“Yes”，否则点“No”", MessageBoxButton.YesNo, MessageBoxImage.Warning, MessageBoxResult.Yes) == MessageBoxResult.Yes)
                {
                    var ss = 0;
                }
            }, () =>
            {
                if (!string.IsNullOrEmpty(ItemId) && !string.IsNullOrEmpty(ItemName) && SoldPirce >= 0)
                {
                    return true;
                }
                else
                { return false; }
            });
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

        private ObservableCollection<ItemSales> _ItemSalesColletion;
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

        #endregion
    }
}
