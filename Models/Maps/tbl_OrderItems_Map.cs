using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using OrderingSystem2.Models.Tables;

namespace OrderingSystem2.Models.Maps
{
    public class tbl_OrderItems_Map : EntityTypeConfiguration<tbl_orderitems_model>
    {
        public tbl_OrderItems_Map()
        {
            HasKey(i => i.OrderItem_ID);
            ToTable("tbl_orderitems");
        }
    }
}