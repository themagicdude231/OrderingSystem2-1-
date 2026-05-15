using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models
{
    public class userOrder
    {
        public List<VendorGroupModel> groupedOrders { get; set; }
        public int paymentMethodId { get; set; }
        public decimal totalAmount { get; set; }
    }
}