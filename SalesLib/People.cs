using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesLib
{
    public class People
    {
        public uint Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }
        public string Phone { get; set; }
        public string Type_discount { get; set; }
        public uint Discount { get; set; }

        public People()
        {
            Id = 0;
            First_name = "Unknown";
            Last_name = "Unknown";
            Phone = "Unknown";
            Type_discount = "None";
            Discount = 0;
        }
    }
}
