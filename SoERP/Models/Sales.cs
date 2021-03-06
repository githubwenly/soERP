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
    
    public partial class Sales
    {
        public Sales()
        {
            this.Goods_Delivery = new HashSet<Goods_Delivery>();
            this.Purchase_Return = new HashSet<Purchase_Return>();
            this.Sales_Brrowing = new HashSet<Sales_Brrowing>();
            this.Sales_delivery = new HashSet<Sales_delivery>();
        }
    
        public int SalesId { get; set; }
        public Nullable<System.DateTime> Sales_date { get; set; }
        public Nullable<int> ProductsId { get; set; }
        public Nullable<int> Customersid { get; set; }
        public Nullable<int> Orderid { get; set; }
        public Nullable<int> Sales_number { get; set; }
        public Nullable<decimal> SumPrice { get; set; }
        public string Settlement_method { get; set; }
        public Nullable<decimal> Amount_payable { get; set; }
        public Nullable<decimal> Real_pay { get; set; }
        public Nullable<decimal> Unpaid_amount { get; set; }
        public string Salse_state { get; set; }
        public string Handlers { get; set; }
        public Nullable<int> Usersid { get; set; }
        public string Tips { get; set; }
    
        public virtual Customer Customer { get; set; }
        public virtual ICollection<Goods_Delivery> Goods_Delivery { get; set; }
        public virtual OrderManager OrderManager { get; set; }
        public virtual Products Products { get; set; }
        public virtual ICollection<Purchase_Return> Purchase_Return { get; set; }
        public virtual ICollection<Sales_Brrowing> Sales_Brrowing { get; set; }
        public virtual ICollection<Sales_delivery> Sales_delivery { get; set; }
        public virtual Users Users { get; set; }
    }
}
