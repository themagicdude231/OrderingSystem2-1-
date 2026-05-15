using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models
{
    public class VendorGroupModel
    {
        public int Vendor_ID { get; set; }
        public List<MenuItem> Items { get; set; }
    }
}