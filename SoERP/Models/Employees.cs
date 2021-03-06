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
    
    public partial class Employees
    {
        public Employees()
        {
            this.Salary = new HashSet<Salary>();
        }
    
        public int EmployeesId { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public Nullable<int> Age { get; set; }
        public Nullable<int> Department { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public string Nationals { get; set; }
        public string Domicile { get; set; }
        public string Address { get; set; }
        public string IdNumber { get; set; }
        public Nullable<System.DateTime> Workingtime { get; set; }
        public string Photo { get; set; }
    
        public virtual Department Department1 { get; set; }
        public virtual ICollection<Salary> Salary { get; set; }
    }
}
