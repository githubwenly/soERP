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
    
    public partial class PlanOrder
    {
        public PlanOrder()
        {
            this.Product = new HashSet<Product>();
        }
    
        public int PlanOrderid { get; set; }
        public string Product_name { get; set; }
        public Nullable<int> Production { get; set; }
        public Nullable<System.DateTime> Product_time { get; set; }
        public string Handlers { get; set; }
        public Nullable<int> Orderid { get; set; }
        public Nullable<int> Usersid { get; set; }
        public string Tips { get; set; }
    
        public virtual OrderManager OrderManager { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
