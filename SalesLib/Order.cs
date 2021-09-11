using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesLib
{
    public class Order
    {
        public uint Id { get; set; }
        public uint Buyer_id { get; set; }
        public uint Seller_id { get; set; }
        public string Date { get; set; }
        public uint Product_id { get; set; }
        public uint Amount { get; set; }
        public uint Total_price { get; set; }

    }
}
