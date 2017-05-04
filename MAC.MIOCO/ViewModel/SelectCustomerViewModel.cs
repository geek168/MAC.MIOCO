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
    public class SelectCustomerViewModel : ViewModelBase
    {
        public Customer CheckedList { get; private set; }

        public SelectCustomerViewModel(Window window)
        {
            BindData();

            CloseCommand = new DelegateCommand(() =>
            {
                window.Close();

                CheckedList = SOURCE.Where(s => s.IsChecked).FirstOrDefault();
            });

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

            SelectCommand = new DelegateCommand<Customer>(s =>
            {
                if (s != null)
                {
                    s.IsChecked = !s.IsChecked;
                }
                SOURCE.Where(t => t.Id != s.Id).ToList().ForEach(t => t.IsChecked = false);
            });
        }

        private void BindData()
        {
            SOURCE = new ObservableCollection<Customer>(SqlServerCompactService.GetData("Customer").Cast<Customer>());
            SOURCE = new ObservableCollection<Customer>(SOURCE.OrderByDescending(s => s.UpdateTime));
            CustomerColletion = new ObservableCollection<Customer>(SOURCE.Skip(PageIndex * PAGESIZE).Take(PAGESIZE));
        }

        public DelegateCommand CloseCommand { get; private set; }

        public DelegateCommand<Customer> SelectCommand { get; private set; }

        public DelegateCommand SearchCommand { get; private set; }

        public DelegateCommand AllCommand { get; private set; }

        public DelegateCommand PreviousCommand { get; private set; }

        public DelegateCommand NextCommand { get; private set; }

        private ObservableCollection<Customer> SOURCE = new ObservableCollection<Customer>();

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
    }
}
