using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAC.MIOCO.Model
{
    public class ItemMaster
    {
        public string Id { get; set; }

        public string ItemId { get; set; }

        public string ItemName { get; set; }

        public int ItemSize { get; set; }

        public int ItemType { get; set; }

        public int StockCount { get; set; }

        public int SalesCount { get; set; }

        public decimal StockPrice { get; set; }

        public decimal Price { get; set; }
    }
}
