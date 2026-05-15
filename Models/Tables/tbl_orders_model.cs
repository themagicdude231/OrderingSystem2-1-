using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models.Tables
{
    public class tbl_orders_model
    {
        public int Order_ID { get; set; }
        public int User_Id { get; set; }
        public int Vendor_Id { get; set; }
        public decimal Order_Total_Amount { get; set; }
        public DateTime Order_Date { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Edited_At { get; set; }
    }
}