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
    public class SelectItemMasterViewModel : ViewModelBase
    {

        public List<ItemMaster> CheckedList { get; private set; }

        public SelectItemMasterViewModel(Window window)
        {
            BindData();

            CloseCommand = new DelegateCommand(() => 
            {
                window.Close();

                CheckedList = SOURCE.Where(s => s.IsChecked).ToList();
            });

            SearchCommand = new DelegateCommand(() =>
            {
                var s = SOURCE.Where(t => t.ItemId.IndexOf(SearchItemId, StringComparison.OrdinalIgnoreCase) >= 0);
                PageIndex = 0;
                ItemMasterColletion = new ObservableCollection<ItemMaster>(s.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return string.IsNullOrEmpty(SearchItemId) ? false : true; });

            AllCommand = new DelegateCommand(() =>
            {
                SearchItemId = "";
                ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Where(t => t.ItemId.IndexOf(SearchItemId, StringComparison.OrdinalIgnoreCase) >= 0).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return string.IsNullOrEmpty(SearchItemId) ? false : true; });


            PreviousCommand = new DelegateCommand(() =>
            {
                PageIndex--;
                ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Where(t => t.ItemId.IndexOf(SearchItemId, StringComparison.OrdinalIgnoreCase) >= 0).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return PageIndex > 0 ? true : false; });

            NextCommand = new DelegateCommand(() =>
            {
                PageIndex++;
                ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Where(t => t.ItemId.IndexOf(SearchItemId, StringComparison.OrdinalIgnoreCase) >= 0).Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
            }, () => { return (PageIndex + 1) * PAGESIZE < SOURCE.Where(t => t.ItemId.IndexOf(SearchItemId, StringComparison.OrdinalIgnoreCase) >= 0).Count() ? true : false; });

            SelectCommand = new DelegateCommand<ItemMaster>(s =>
            {
                if (s != null)
                {
                    s.IsChecked = !s.IsChecked;
                }
            });
        }

        private void BindData()
        {
            SOURCE = new ObservableCollection<ItemMaster>(SqlServerCompactService.GetData("ItemMaster").Cast<ItemMaster>());
            SOURCE = new ObservableCollection<ItemMaster>(SOURCE.OrderByDescending(s => s.UpdateTime));
            ItemMasterColletion = new ObservableCollection<ItemMaster>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
        }

        public DelegateCommand CloseCommand { get; private set; }

        public DelegateCommand<ItemMaster> SelectCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand AllCommand { get; private set; }

        public DelegateCommand PreviousCommand { get; private set; }

        public DelegateCommand NextCommand { get; private set; }

        private ObservableCollection<ItemMaster> SOURCE = new ObservableCollection<ItemMaster>();

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

    }
}
