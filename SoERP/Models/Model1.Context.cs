﻿//------------------------------------------------------------------------------
// <auto-generated>
//    此代码是根据模板生成的。
//
//    手动更改此文件可能会导致应用程序中发生异常行为。
//    如果重新生成代码，则将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace SoERP.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class ErpDBEntities : DbContext
    {
        public ErpDBEntities()
            : base("name=ErpDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Announcements> Announcements { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Dermal> Dermal { get; set; }
        public DbSet<Employees> Employees { get; set; }
        public DbSet<Good_Quality> Good_Quality { get; set; }
        public DbSet<Goods> Goods { get; set; }
        public DbSet<Goods_Delivery> Goods_Delivery { get; set; }
        public DbSet<Goods_inventory> Goods_inventory { get; set; }
        public DbSet<Lnven> Lnven { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<OrderManager> OrderManager { get; set; }
        public DbSet<PlanOrder> PlanOrder { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Product_Put> Product_Put { get; set; }
        public DbSet<Product_Quality> Product_Quality { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Purchase__Brrowing> Purchase__Brrowing { get; set; }
        public DbSet<Purchase_brrowing> Purchase_brrowing { get; set; }
        public DbSet<Purchase_order> Purchase_order { get; set; }
        public DbSet<Purchase_Put> Purchase_Put { get; set; }
        public DbSet<Purchase_Return> Purchase_Return { get; set; }
        public DbSet<Purchase_returns> Purchase_returns { get; set; }
        public DbSet<Salary> Salary { get; set; }
        public DbSet<Sales> Sales { get; set; }
        public DbSet<Sales_Brrowing> Sales_Brrowing { get; set; }
        public DbSet<Sales_delivery> Sales_delivery { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<sysdiagrams> sysdiagrams { get; set; }
        public DbSet<Transport> Transport { get; set; }
        public DbSet<Users> Users { get; set; }
    }
}
