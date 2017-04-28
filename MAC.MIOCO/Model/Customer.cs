using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAC.MIOCO.Model
{
    public class Customer
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public string IM { get; set; }

        public decimal Deposit { get; set; }

        public int Discount { get; set; }

        public string Remark { get; set; }

        public DateTime UpdateTime { get; set; }
    }
}
