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
    
    public partial class Shif
    {
        public int Shif_id { get; set; }
        public string Name { get; set; }
        public int DepartmentId { get; set; }
        public string Cause { get; set; }
        public System.DateTime Applay_time { get; set; }
        public System.DateTime YStart_time { get; set; }
        public System.DateTime YEnd_time { get; set; }
        public int YTotal { get; set; }
        public System.DateTime NStart_time { get; set; }
        public System.DateTime NEnd_time { get; set; }
        public int NTotal { get; set; }
        public string Leave_Type { get; set; }
        public int Progress { get; set; }
        public int Userid { get; set; }
    
        public virtual Department Department { get; set; }
    }
}