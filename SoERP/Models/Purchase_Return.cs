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
    
    public partial class Purchase_Return
    {
        public int Purchase_returnid { get; set; }
        public Nullable<int> Purchase_num { get; set; }
        public Nullable<decimal> Purchase_monry { get; set; }
        public Nullable<int> Purchaseid { get; set; }
        public string Handlers { get; set; }
        public Nullable<int> Usersid { get; set; }
        public string Tips { get; set; }
        public Nullable<System.DateTime> Returndate { get; set; }
    
        public virtual Sales Sales { get; set; }
        public virtual Users Users { get; set; }
    }
}
