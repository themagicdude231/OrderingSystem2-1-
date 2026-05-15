using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using OrderingSystem2.Models.Tables;

namespace OrderingSystem2.Models.Maps
{
    public class tbl_Error_Logs_Map : EntityTypeConfiguration<tbl_error_logs_model>
    {
        public tbl_Error_Logs_Map()
        {
            HasKey(i => i.Error_ID);
            ToTable("tbl_error_logs");
        }
    }
}