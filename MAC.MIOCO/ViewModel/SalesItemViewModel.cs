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
            SOURCE = new ObservableCollection<ItemSales>(SqlServerCompactService.GetItemSales(DateTime.Now, 0));
            TotalCount = SOURCE.Count.ToString();
            TotalProfit = SOURCE.Sum(s => s.SoldPirce - (s.SalesCount * s.StockPrice)).ToString();

            ItemSalesColletion = new ObservableCollection<ItemSales>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));

            CloseCommand = new DelegateCommand(() =>
            {
                window.Close();
            });

            AnalyseCommand = new DelegateCommand<string>(s =>
            {
                ItemSalesColletion.Clear();

                int t = 0;
                switch (s)
                {
                    case "0":
                        t = 0;
                        break;
                    case "1":
                        t = 1;
                        break;
                    case "2":
                        t = 2;
                        break;
                }

                SOURCE = new ObservableCollection<ItemSales>(SqlServerCompactService.GetItemSales(SelectedDate, t));
                TotalCount = SOURCE.Count.ToString();
                TotalProfit = SOURCE.Sum(k => k.SoldPirce - (k.SalesCount * k.StockPrice)).ToString();
                ItemSalesColletion = new ObservableCollection<ItemSales>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            });

            PreviousCommand = new DelegateCommand(() =>
            {
                PageIndex--;
                ItemSalesColletion = new ObservableCollection<ItemSales>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return PageIndex > 0 ? true : false; });

            NextCommand = new DelegateCommand(() =>
            {
                PageIndex++;
                ItemSalesColletion = new ObservableCollection<ItemSales>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return (PageIndex + 1) * PAGESIZE < SOURCE.Count() ? true : false; });
        }

        public DelegateCommand CloseCommand { get; private set; }

        public DelegateCommand<string> AnalyseCommand { get; private set; }

        public DelegateCommand PreviousCommand { get; private set; }

        public DelegateCommand NextCommand { get; private set; }

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

        private DateTime _SelectedDate = DateTime.Now;
        public DateTime SelectedDate
        {
            get { return _SelectedDate; }
            set
            {
                _SelectedDate = value;
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

    }
}
