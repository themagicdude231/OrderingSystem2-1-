using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using OrderingSystem2.Models.Tables;

namespace OrderingSystem2.Models.Maps
{
    public class tbl_Payments_Map : EntityTypeConfiguration<tbl_payments_model>
    {
       public tbl_Payments_Map()
        {
            HasKey(i => i.Payment_ID);
            ToTable("tbl_payments");
        }
    }
}