//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class Product
    {
        public Product()
        {
            this.Product_Put = new HashSet<Product_Put>();
            this.Product_Quality = new HashSet<Product_Quality>();
        }
    
        public int Productid { get; set; }
        public string Product_name { get; set; }
        public Nullable<int> Productcount { get; set; }
        public Nullable<System.DateTime> Product_time { get; set; }
        public Nullable<int> Product_pihao { get; set; }
        public Nullable<int> Departmentid { get; set; }
        public Nullable<int> PlanOrderid { get; set; }
        public string State { get; set; }
        public Nullable<int> Usersid { get; set; }
        public string Tips { get; set; }
    
        public virtual Department Department { get; set; }
        public virtual PlanOrder PlanOrder { get; set; }
        public virtual ICollection<Product_Put> Product_Put { get; set; }
        public virtual ICollection<Product_Quality> Product_Quality { get; set; }
        public virtual Users Users { get; set; }
    }
}
