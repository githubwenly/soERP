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
    
    public partial class Sales_delivery
    {
        public int Deliveryid { get; set; }
        public Nullable<int> Delivery_num { get; set; }
        public Nullable<System.DateTime> Delivery_date { get; set; }
        public Nullable<int> Salesid { get; set; }
        public string Handlers { get; set; }
        public Nullable<int> Usersid { get; set; }
        public string Tips { get; set; }
    
        public virtual Sales Sales { get; set; }
        public virtual Users Users { get; set; }
    }
}
