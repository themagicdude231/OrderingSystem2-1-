using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models.Tables
{
    public class tbl_menus_model
    {
        public int Menu_ID { get; set; }
        public int Vendor_Id { get; set; }
        public int Menu_Status_Id { get; set; }
        public string Menu_Name { get; set; }
        public string Menu_Description { get; set; }
        public decimal Menu_Price { get; set; }
        public string Menu_Image { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Edited_At { get; set; }
    }
}