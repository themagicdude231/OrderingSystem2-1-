using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using OrderingSystem2.Models.Tables;

namespace OrderingSystem2.Models.Maps
{
    public class tbl_Payment_Methods_Map : EntityTypeConfiguration<tbl_payment_methods_model>
    {
        public tbl_Payment_Methods_Map()
        {
            HasKey(i => i.Payment_Method_ID);
            ToTable("tbl_payment_methods");
        }
    }
}