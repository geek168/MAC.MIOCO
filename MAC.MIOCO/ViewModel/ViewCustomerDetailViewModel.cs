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
    public class ViewCustomerDetailViewModel : ViewModelBase
    {
        public ViewCustomerDetailViewModel(Window window, Customer customer)
        {
            CloseCommand = new DelegateCommand(() =>
            {
                window.Close();
            });

            //ITEMSALESSOURCE = new ObservableCollection<ItemSales>(SqlServerCompactService.GetItemSales());
            //ItemSalesColletion = new ObservableCollection<ItemSales>(ITEMSALESSOURCE.Skip(ItemSalesPageIndex * PAGESIZE).Take(PAGESIZE));
            //ItemSalesPreviousCommand = new DelegateCommand(() =>
            //{
            //    ItemSalesPageIndex--;
            //    ItemSalesColletion = new ObservableCollection<ItemSales>(ITEMSALESSOURCE.Skip(ItemSalesPageIndex * PAGESIZE).Take(PAGESIZE));
            //}, () => { return ItemSalesPageIndex > 0 ? true : false; });

            //ItemSalesNextCommand = new DelegateCommand(() =>
            //{
            //    ItemSalesPageIndex++;
            //    ItemSalesColletion = new ObservableCollection<ItemSales>(ITEMSALESSOURCE.Skip(ItemSalesPageIndex * PAGESIZE).Take(PAGESIZE));
            //}, () => { return (ItemSalesPageIndex + 1) * PAGESIZE < ITEMSALESSOURCE.Count() ? true : false; });

            ITEMSALESSOURCE = new ObservableCollection<ItemSales>(SqlServerCompactService.GetItemSales(customer.Id));
            ItemSalesColletion = new ObservableCollection<ItemSales>(ITEMSALESSOURCE.Skip(ItemSalesPageIndex * PAGESIZE).Take(PAGESIZE));
            SearchCommand = new DelegateCommand(() =>
            {
                var s = ITEMSALESSOURCE.Where(t => t.ItemSalesId.IndexOf(SearchItemSalesId, StringComparison.OrdinalIgnoreCase) >= 0);
                ItemSalesPageIndex = 0;
                ItemSalesColletion = new ObservableCollection<ItemSales>(s.Skip(ItemSalesPageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return string.IsNullOrEmpty(SearchItemSalesId) ? false : true; });

            AllCommand = new DelegateCommand(() =>
            {
                SearchItemSalesId = "";
                ItemSalesColletion = new ObservableCollection<ItemSales>(ITEMSALESSOURCE.Where(t => t.ItemSalesId.IndexOf(SearchItemSalesId, StringComparison.OrdinalIgnoreCase) >= 0).Skip(ItemSalesPageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return string.IsNullOrEmpty(SearchItemSalesId) ? false : true; });

            ItemSalesPreviousCommand = new DelegateCommand(() =>
            {
                ItemSalesPageIndex--;
                ItemSalesColletion = new ObservableCollection<ItemSales>(ITEMSALESSOURCE.Where(t => t.ItemSalesId.IndexOf(SearchItemSalesId, StringComparison.OrdinalIgnoreCase) >= 0).Skip(ItemSalesPageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return ItemSalesPageIndex > 0 ? true : false; });

            ItemSalesNextCommand = new DelegateCommand(() =>
            {
                ItemSalesPageIndex++;
                ItemSalesColletion = new ObservableCollection<ItemSales>(ITEMSALESSOURCE.Where(t => t.ItemSalesId.IndexOf(SearchItemSalesId, StringComparison.OrdinalIgnoreCase) >= 0).Skip(ItemSalesPageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return (ItemSalesPageIndex + 1) * PAGESIZE < ITEMSALESSOURCE.Where(t => t.ItemSalesId.IndexOf(SearchItemSalesId, StringComparison.OrdinalIgnoreCase) >= 0).Count() ? true : false; });


            DEPOSITDETAILSOURCE = new ObservableCollection<DepositDetail>(SqlServerCompactService.GetDepositDetail(customer.Id));
            DepositDetailColletion = new ObservableCollection<DepositDetail>(DEPOSITDETAILSOURCE.Skip(DepositDetailPageIndex * PAGESIZE).Take(PAGESIZE));
            DepositDetailPreviousCommand = new DelegateCommand(() =>
            {
                DepositDetailPageIndex--;
                DepositDetailColletion = new ObservableCollection<DepositDetail>(DEPOSITDETAILSOURCE.Skip(DepositDetailPageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return DepositDetailPageIndex > 0 ? true : false; });

            DepositDetailNextCommand = new DelegateCommand(() =>
            {
                DepositDetailPageIndex++;
                DepositDetailColletion = new ObservableCollection<DepositDetail>(DEPOSITDETAILSOURCE.Skip(DepositDetailPageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return (DepositDetailPageIndex + 1) * PAGESIZE < ITEMSALESSOURCE.Count() ? true : false; });
        }


        public DelegateCommand CloseCommand { get; private set; }

        private int PAGESIZE = 20;

        #region ItemSales

        public DelegateCommand ItemSalesPreviousCommand { get; private set; }

        public DelegateCommand ItemSalesNextCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand AllCommand { get; private set; }

        private ObservableCollection<ItemSales> ITEMSALESSOURCE = new ObservableCollection<ItemSales>();

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

        private int _ItemSalesPageIndex = 0;
        public int ItemSalesPageIndex
        {
            get { return _ItemSalesPageIndex; }
            set
            {
                if (value >= 0)
                {
                    _ItemSalesPageIndex = value;
                }
            }
        }


        private string _SearchItemSalesId = "";
        public string SearchItemSalesId
        {
            get { return _SearchItemSalesId; }
            set
            {
                _SearchItemSalesId = value;
                OnPropertyChanged(nameof(SearchItemSalesId));
            }
        }

        #endregion

        #region DepositDetail

        public DelegateCommand DepositDetailPreviousCommand { get; private set; }

        public DelegateCommand DepositDetailNextCommand { get; private set; }

        private ObservableCollection<DepositDetail> DEPOSITDETAILSOURCE = new ObservableCollection<DepositDetail>();

        private ObservableCollection<DepositDetail> _DepositDetailColletion = new ObservableCollection<DepositDetail>();
        public ObservableCollection<DepositDetail> DepositDetailColletion
        {
            get { return _DepositDetailColletion; }
            set
            {
                _DepositDetailColletion = value;
                OnPropertyChanged(nameof(DepositDetailColletion));
            }
        }

        private int _DepositDetailPageIndex = 0;
        public int DepositDetailPageIndex
        {
            get { return _DepositDetailPageIndex; }
            set
            {
                if (value >= 0)
                {
                    _DepositDetailPageIndex = value;
                }
            }
        }

        #endregion
    }
}
