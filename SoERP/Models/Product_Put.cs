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
    
    public partial class Product_Put
    {
        public Product_Put()
        {
            this.Goods_inventory = new HashSet<Goods_inventory>();
        }
    
        public int Putid { get; set; }
        public Nullable<int> Productid { get; set; }
        public Nullable<System.DateTime> Put_date { get; set; }
        public Nullable<int> Putnumber { get; set; }
        public string State { get; set; }
        public string Handlers { get; set; }
        public Nullable<int> Usersid { get; set; }
        public string Note { get; set; }
    
        public virtual ICollection<Goods_inventory> Goods_inventory { get; set; }
        public virtual Product Product { get; set; }
    }
}
