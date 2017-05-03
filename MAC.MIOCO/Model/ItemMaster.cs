using MAC.MIOCO.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAC.MIOCO.Model
{
    public class ItemMaster : INotifyPropertyChanged
    {
        private bool _IsChecked;
        public bool IsChecked
        {
            get { return _IsChecked; }
            set
            {
                _IsChecked = value;
                OnPropertyChanged(nameof(IsChecked));
            }
        }

        public string Id { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public int ItemSize { get; set; }

        public int ItemType { get; set; }

        public int StockCount { get; set; }

        //public int SalesCount { get; set; }

        public decimal StockPrice { get; set; }

        public decimal Price { get; set; }

        public DateTime UpdateTime { get; set; }

        public string Color { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
