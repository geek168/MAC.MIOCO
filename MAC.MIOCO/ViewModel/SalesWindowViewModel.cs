using MAC.MIOCO;
using MAC.MIOCO.Command;
using MAC.MIOCO.Model;
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


        private ObservableCollection<ItemMaster> SOURCE = new ObservableCollection<ItemMaster>();

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

            //SOURCE = new ObservableCollection<ItemMaster>
            //{
            //    new ItemMaster {ItemId="MIC123456789", ItemName="春装上衣--ABC110", ItemSize =3, ItemType = 0, StockCount=10, SalesCount=1, StockPrice=250 , Price = 300},
            //    new ItemMaster {ItemId="MIC123456ABC", ItemName="夏季装上衣--GDA110", ItemSize =5, ItemType = 1, StockCount=0, SalesCount=1, StockPrice=116 ,Price=150},
            //    new ItemMaster {ItemId="MIC12345633A", ItemName="冬装大衣--ABC110", ItemSize =3, ItemType = 2, StockCount=9, SalesCount=1, StockPrice=56 ,Price=60},
            //    new ItemMaster {ItemId="MIC123456891", ItemName="秋装连衣裙ABC110", ItemSize =7, ItemType = 3, StockCount=3, SalesCount=1, StockPrice=218 ,Price=220},
            //    new ItemMaster {ItemId="MIC123456301", ItemName="春装坎肩ABC110", ItemSize =7, ItemType = 4, StockCount=0, SalesCount=1, StockPrice=380,Price=385 },
            //    new ItemMaster {ItemId="MIC123456EFT", ItemName="夏装披风加99生命ABC110", ItemSize =7, ItemType = 5, StockCount=5, SalesCount=1, StockPrice=158 ,Price=160},
            //    new ItemMaster {ItemId="MIC123456GHJ", ItemName="秋装上衣ABC110", ItemSize =7, ItemType = 6, StockCount=6, SalesCount=1, StockPrice=188,Price=200 },
            //    new ItemMaster {ItemId="MIC12345699A", ItemName="冬装上衣ABC110", ItemSize =5, ItemType = 7, StockCount=9, SalesCount=1, StockPrice=66 ,Price=100},
            //    new ItemMaster {ItemId="MIC123456QWR", ItemName="完美装上衣ABC110", ItemSize =7, ItemType = 8, StockCount=0, SalesCount=1, StockPrice=999,Price=1111 },
            //    new ItemMaster {ItemId="MIC123456GTL", ItemName="无敌漂亮美少女上衣11111111111111111111111111ABC11011", ItemSize =3, ItemType = 0, StockCount=2, SalesCount=1, StockPrice=19999,Price=20000 },
            //    new ItemMaster {ItemId="MIC123456GTL", ItemName="巴拉巴拉小魔仙ABC110", ItemSize =3, ItemType = 0, StockCount=2, SalesCount=1, StockPrice=9999,Price=10000 },
            //    new ItemMaster {ItemId="MIC123456GTL", ItemName="旅居下ABC110", ItemSize =11, ItemType = 0, StockCount=2, SalesCount=1, StockPrice=8888,Price=9999 },
            //    new ItemMaster {ItemId="MIC123456GTL", ItemName="无敌浩克ABC110", ItemSize =15, ItemType = 0, StockCount=2, SalesCount=1, StockPrice=6666,Price=8888 },
            //    new ItemMaster {ItemId="MIC123456GTL", ItemName="钢铁侠ABC110", ItemSize =13, ItemType = 0, StockCount=2, SalesCount=1, StockPrice=454547 ,Price=99999999}
            //};

            SOURCE = new ObservableCollection<ItemMaster>(SqlServerCompactService.GetData("ItemMaster").Cast<ItemMaster>());

            SelectCommand = new DelegateCommand<ItemMaster>(s =>
            {
                if (s != null)
                {
                    if (s.StockCount == 0)
                    {
                        ClearItem();
                    }
                    else
                    {
                        ItemId = s.ItemId;
                        ItemName = s.ItemName;
                        ItemSize = s.ItemSize;
                        ItemType = s.ItemType;
                        StockPrice = s.StockPrice;
                        Price = s.Price;
                        Count = 1;
                    }
                }
            });

            ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));

            PreviousCommand = new DelegateCommand(() =>
            {
                PageIndex--;
                ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Where(t => t.ItemId.Contains(SearchItemId)).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            },()=> { return PageIndex > 0 ? true : false; });

            NextCommand = new DelegateCommand(() =>
            {
                PageIndex++;
                ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Where(t => t.ItemId.Contains(SearchItemId)).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            },()=> { return (PageIndex + 1) * PAGESIZE < SOURCE.Where(t => t.ItemId.Contains(SearchItemId)).Count() ? true : false; });

            SearchCommand = new DelegateCommand(() =>
            {
                var s = SOURCE.Where(t => t.ItemId.Contains(SearchItemId));
                PageIndex = 0;
                ItemMasterColletion = new ObservableCollection<ItemMaster>(s.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return string.IsNullOrEmpty(SearchItemId) ? false : true; });

            AllCommand = new DelegateCommand(() =>
            {
                SearchItemId = "";
                ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Where(t => t.ItemId.Contains(SearchItemId)).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return string.IsNullOrEmpty(SearchItemId) ? false : true; });

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

        public DelegateCommand<ItemMaster> SelectCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand AllCommand { get; private set; }

        public DelegateCommand PreviousCommand { get; private set; }

        public DelegateCommand NextCommand { get; private set; }

        public DelegateCommand SoldCommand { get; private set; }

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

        private ObservableCollection<ItemMaster> _ItemMasterColletion;
        public ObservableCollection<ItemMaster> ItemMasterColletion
        {
            get { return _ItemMasterColletion; }
            set
            {
                _ItemMasterColletion = value;
                OnPropertyChanged(nameof(ItemMasterColletion));
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
            }
        }

        private string _SearchItemId = "";
        public string SearchItemId
        {
            get { return _SearchItemId; }
            set
            {
                _SearchItemId = value;
                OnPropertyChanged(nameof(SearchItemId));
            }
        }

        #endregion
    }
}
