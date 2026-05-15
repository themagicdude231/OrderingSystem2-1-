using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models.Tables
{
    public class tbl_payments_model
    {
        public int Payment_ID { get; set; }
        public int Order_Id { get; set; }
        public int Payment_Method_Id { get; set; }
        public int Payment_Status_Id { get; set; }
        public DateTime Payment_Date { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Edited_At { get; set; }
    }
}