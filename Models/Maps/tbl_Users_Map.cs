using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using OrderingSystem2.Models.Tables;

namespace OrderingSystem2.Models.Maps
{
    public class tbl_Users_Map : EntityTypeConfiguration<tbl_users_model>
    {
        public tbl_Users_Map()
        {
            HasKey(i => i.User_ID);
            ToTable("tbl_users");
        }
    }
}