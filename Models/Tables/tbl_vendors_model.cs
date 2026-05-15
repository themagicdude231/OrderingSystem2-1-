using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models.Tables
{
    public class tbl_vendors_model
    {
        public int Vendor_ID { get; set; }
        public int User_Id { get; set; }
        public int Store_Category_Id { get; set; }
        public string Vendor_Storename { get; set; }
        public string Vendor_Store_Address { get; set; }
        public string Vendor_Store_Picture { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Edited_At { get; set; }
    }
}