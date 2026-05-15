using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using OrderingSystem2.Models.Tables;
using OrderingSystem2.Models.Maps;

namespace OrderingSystem2.Models.Context
{
    public class pickNServeContext : DbContext
    {
        static pickNServeContext()
        {
            Database.SetInitializer<pickNServeContext>(null);
        }
        public pickNServeContext() : base("Name=pickNServeDb") { }
        public virtual DbSet<tbl_users_model> tbl_Users { get; set; }
        public virtual DbSet<tbl_error_logs_model> tbl_ErrorLogs { get; set; }
        public virtual DbSet<tbl_store_categories_model> tbl_Store_Categories { get; set; }
        public virtual DbSet<tbl_vendors_model> tbl_Vendors { get; set; }
        public virtual DbSet<tbl_menus_model> tbl_Menus { get; set; }
        public virtual DbSet<tbl_menus_status_model> tbl_Menus_Status { get; set; }
        public virtual DbSet<tbl_orders_model> tbl_Orders { get; set; }
        public virtual DbSet<tbl_payment_status_model> tbl_Payment_Status { get; set; }
        public virtual DbSet<tbl_payment_methods_model> tbl_Payment_Methods { get; set; }
        public virtual DbSet<tbl_orderitems_model> tbl_OrderItems { get; set; }
        public virtual DbSet<tbl_payments_model> tbl_Payments { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new tbl_Users_Map());
            modelBuilder.Configurations.Add(new tbl_Error_Logs_Map());
            modelBuilder.Configurations.Add(new tbl_Store_Categories_Map());
            modelBuilder.Configurations.Add(new tbl_Vendors_Map());
            modelBuilder.Configurations.Add(new tbl_Menus_Map());
            modelBuilder.Configurations.Add(new tbl_Menus_Status_Map());
            modelBuilder.Configurations.Add(new tbl_Orders_Map());
            modelBuilder.Configurations.Add(new tbl_Payment_Status_Map());
            modelBuilder.Configurations.Add(new tbl_Payment_Methods_Map());
            modelBuilder.Configurations.Add(new tbl_OrderItems_Map());
            modelBuilder.Configurations.Add(new tbl_Payments_Map());
        }
    }
}