using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAC.MIOCO.Model
{
    public class Customer : INotifyPropertyChanged
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

        public string Name { get; set; }

        public string Phone { get; set; }

        public string IM { get; set; }

        public decimal Deposit { get; set; }

        public int Discount { get; set; }

        public string Remark { get; set; }

        public DateTime UpdateTime { get; set; }


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
