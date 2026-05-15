using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OrderingSystem2.Models.Tables
{
    public class tbl_users_model
    {
        public int User_ID { get; set; }
        public string User_Fullname { get; set; }
        public string User_Email { get; set; }
        public string User_Address { get; set; }
        public string User_Username { get; set; }
        public string User_Password { get; set; }
        public bool User_isVendor { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Edited_At { get; set; }
    }
}