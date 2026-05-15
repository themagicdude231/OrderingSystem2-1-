using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity.ModelConfiguration;
using OrderingSystem2.Models.Tables;

namespace OrderingSystem2.Models.Maps
{
    public class tbl_Store_Categories_Map : EntityTypeConfiguration<tbl_store_categories_model>
    {
        public tbl_Store_Categories_Map()
        {
            HasKey(i => i.Store_Category_ID);
            ToTable("tbl_store_categories");
        }
    }
}