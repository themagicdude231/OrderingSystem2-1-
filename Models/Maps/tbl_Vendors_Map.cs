using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using OrderingSystem2.Models.Tables;

namespace OrderingSystem2.Models.Maps
{
    public class tbl_Vendors_Map : EntityTypeConfiguration<tbl_vendors_model>
    {
        public tbl_Vendors_Map()
        {
            HasKey(i => i.Vendor_ID);
            ToTable("tbl_vendors");
        }
    }
}