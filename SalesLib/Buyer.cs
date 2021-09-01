using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesLib
{
    public class Buyer
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public uint Discount { get; set; }

        public Buyer()
        {
            Id = 0;
            Name = "Unknown";
            Discount = 0;
        }
    }
}
