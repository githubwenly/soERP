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
    
    public partial class Transport
    {
        public Transport()
        {
            this.OrderManager = new HashSet<OrderManager>();
        }
    
        public int Transportid { get; set; }
        public string TransportName { get; set; }
    
        public virtual ICollection<OrderManager> OrderManager { get; set; }
    }
}
