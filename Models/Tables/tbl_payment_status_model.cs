using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models.Tables
{
    public class tbl_payment_status_model
    {
        public int Payment_Status_ID { get; set; }
        public string Payment_Status_Description { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Edited_At { get; set; }
    }
}