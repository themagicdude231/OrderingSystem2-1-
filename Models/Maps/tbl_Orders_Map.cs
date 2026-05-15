using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using OrderingSystem2.Models.Tables;

namespace OrderingSystem2.Models.Maps
{
    public class tbl_Orders_Map : EntityTypeConfiguration<tbl_orders_model>
    {
        public tbl_Orders_Map()
        {
            HasKey(i => i.Order_ID);
            ToTable("tbl_orders");
        }
    }
}