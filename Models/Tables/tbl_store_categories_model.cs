using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models.Tables
{
    public class tbl_store_categories_model
    {
        public int Store_Category_ID { get; set; }
        public string Category_Description { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Edited_At { get; set; }
    }
}