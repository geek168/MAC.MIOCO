using MAC.MIOCO.Command;
using MAC.MIOCO.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MAC.MIOCO.ViewModel
{
    public class SalesItemViewModel : ViewModelBase
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public SalesItemViewModel(Window window)
        {
            SOURCE = new ObservableCollection<ItemSales>
            {
                new ItemSales {ItemId="MIC123456789", ItemName="春装上衣--ABC110", ItemType = 0, CustomerName="王先生", SoldPirce = 300, StockPrice = 100, Profit = 200, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456ABC", ItemName="夏季装上衣--GDA110", ItemType = 1, CustomerName="李女士", SoldPirce=150, StockPrice = 100, Profit = 50, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC12345633A", ItemName="冬装大衣--ABC110", ItemType = 2, CustomerName="", SoldPirce=60, StockPrice = 40, Profit = 20, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456891", ItemName="秋装连衣裙ABC110", ItemType = 3, CustomerName="", SoldPirce=220, StockPrice = 200, Profit = 20, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456301", ItemName="春装坎肩ABC110", ItemType = 4, CustomerName="", SoldPirce=385, StockPrice = 188, Profit = 200, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456EFT", ItemName="夏装披风加99生命ABC110", ItemType = 5, CustomerName="", SoldPirce=160, StockPrice = 100, Profit = 60, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456GHJ", ItemName="秋装上衣ABC110", ItemType = 6, CustomerName="", SoldPirce=200, StockPrice = 100, Profit = 100, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC12345699A", ItemName="冬装上衣ABC110", ItemType = 7, CustomerName="", SoldPirce=100, StockPrice = 100, Profit = 0, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456QWR", ItemName="完美装上衣ABC110", ItemType = 8, CustomerName="", SoldPirce=1111, StockPrice = 999, Profit = 200, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456GTL", ItemName="无敌漂亮美少女上衣11111111111111111111111111ABC11011", ItemType = 0, CustomerName="", SoldPirce=20000,StockPrice = 1999,  Profit = 200, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456GTL", ItemName="巴拉巴拉小魔仙ABC110", ItemType = 0, CustomerName="", SoldPirce=10000, StockPrice = 8888, Profit = 1222, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456GTL", ItemName="旅居下ABC110", ItemType = 0, CustomerName="", SoldPirce=9999, StockPrice = 6666, Profit = 3333, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456GTL", ItemName="无敌浩克ABC110", ItemType = 0, CustomerName="", SoldPirce=8888, StockPrice = 8811, Profit = 77, SoldTime = DateTime.Now},
                new ItemSales {ItemId="MIC123456GTL", ItemName="钢铁侠ABC110", ItemType = 0, CustomerName="", SoldPirce=99999999, StockPrice = 10000, Profit = 98999, SoldTime = DateTime.Now}
            };
            TotalCount = SOURCE.Count.ToString();
            TotalProfit = SOURCE.Sum(s => s.Profit).ToString();
            

            ItemSalesColletion = new ObservableCollection<ItemSales>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));

            CloseCommand = new DelegateCommand(() =>
            {
                window.Close();
            });
        }

        public DelegateCommand CloseCommand { get; private set; }

        private ObservableCollection<ItemSales> SOURCE = new ObservableCollection<ItemSales>();

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

        private string _TotalCount;
        public string TotalCount
        {
            get { return _TotalCount; }
            set
            {
                _TotalCount = value;
                OnPropertyChanged(nameof(TotalCount));
            }
        }

        private string _TotalProfit;
        public string TotalProfit
        {
            get { return _TotalProfit; }
            set
            {
                _TotalProfit = value;
                OnPropertyChanged(nameof(TotalProfit));
            }
        }

    }
}
