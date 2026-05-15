using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models.Tables
{
    public class tbl_orderitems_model
    {
        public int OrderItem_ID { get; set; }
        public int Order_Id { get; set; }
        public int Menu_Id { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Edited_At { get; set; }
    }
}