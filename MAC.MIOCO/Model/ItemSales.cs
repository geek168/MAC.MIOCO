using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAC.MIOCO.Model
{
    public class ItemSales : ItemMaster
    {
        public string ItemSalesId { get; set; }

        public int SalesType { get; set; }

        public string CustomerId { get; set; }

        public decimal DepositForUpdate { get; set; }

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

        private int _SalesCount;
        public int SalesCount
        {
            get { return _SalesCount; }
            set
            {
                _SalesCount = value;
                OnPropertyChanged(nameof(SalesCount));
            }
        }

        
    }
}
