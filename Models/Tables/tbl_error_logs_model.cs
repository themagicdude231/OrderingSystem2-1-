using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models.Tables
{
    public class tbl_error_logs_model
    {
        public int Error_ID { get; set; }
        public string Error_Description { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Edited_At { get; set; }
    }
}