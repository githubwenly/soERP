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
    
    public partial class Good_Quality
    {
        public int QualityId { get; set; }
        public Nullable<System.DateTime> Quality_date { get; set; }
        public Nullable<int> Purchase_orderid { get; set; }
        public string Goodsname { get; set; }
        public string Quality_state { get; set; }
        public Nullable<int> Quality_end { get; set; }
        public string Handlers { get; set; }
        public Nullable<int> Usersid { get; set; }
        public string Tips { get; set; }
    
        public virtual Purchase_order Purchase_order { get; set; }
        public virtual Users Users { get; set; }
    }
}
