using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using OrderingSystem2.Models.Tables;

namespace OrderingSystem2.Models.Maps
{
    public class tbl_Payment_Status_Map : EntityTypeConfiguration<tbl_payment_status_model>
    {
        public tbl_Payment_Status_Map()
        {
            HasKey(i => i.Payment_Status_ID);
            ToTable("tbl_payment_status");
        }
    }
}