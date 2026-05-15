using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models
{
    public class MenuItem
    {
        public int Menu_ID { get; set; }
        public int Vendor_ID { get; set; }
        public decimal Menu_Price { get; set; }
        public int Quantity { get; set; }
    }
}