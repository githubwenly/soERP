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
    
    public partial class Purchase_brrowing
    {
        public int Brrowingid { get; set; }
        public Nullable<System.DateTime> Brrowingdate { get; set; }
        public Nullable<int> Purchaseid { get; set; }
        public string Product_name { get; set; }
        public string Purchase_returnsnumber { get; set; }
        public Nullable<decimal> Purchase_returnsprice { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<decimal> Amount_payable { get; set; }
        public Nullable<decimal> Real_pay { get; set; }
        public Nullable<decimal> Unpaid_amount { get; set; }
        public string Handlers { get; set; }
        public Nullable<int> Usersid { get; set; }
        public string Tips { get; set; }
    
        public virtual Purchase_order Purchase_order { get; set; }
        public virtual Users Users { get; set; }
    }
}
