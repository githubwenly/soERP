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
    
    public partial class OrderManager
    {
        public OrderManager()
        {
            this.PlanOrder = new HashSet<PlanOrder>();
            this.Sales = new HashSet<Sales>();
        }
    
        public int Orderid { get; set; }
        public Nullable<int> CustomerID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public Nullable<int> Ordercount { get; set; }
        public string State { get; set; }
        public Nullable<System.DateTime> Ordertime { get; set; }
        public Nullable<System.DateTime> Delivery_date { get; set; }
        public Nullable<int> Transport { get; set; }
        public Nullable<decimal> Order_price { get; set; }
        public string Handlers { get; set; }
        public Nullable<int> Usersid { get; set; }
        public string Tips { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual Products Products { get; set; }
        public virtual Transport Transport1 { get; set; }
        public virtual Users Users { get; set; }
        public virtual ICollection<PlanOrder> PlanOrder { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
    }
}