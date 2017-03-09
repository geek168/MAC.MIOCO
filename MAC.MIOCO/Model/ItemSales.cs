using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAC.MIOCO.Model
{
    public class ItemSales
    {
        public string Id { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public int ItemType { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public decimal SoldPirce { get; set; }

        public decimal StockPrice { get; set; }

        public decimal Profit { get; set; }

        public DateTime SoldTime { get; set; }
    }
}
