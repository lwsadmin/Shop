//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Shop.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class TManagerRole
    {
        public System.Guid Guid { get; set; }
        public System.Guid OperatorGuid { get; set; }
        public Nullable<System.Guid> BusinessGuid { get; set; }
        public Nullable<System.Guid> ChainStoreGuid { get; set; }
        public string ManageRange { get; set; }
        public string Title { get; set; }
        public string ManageRole { get; set; }
        public bool IsSystem { get; set; }
        public bool IsBusinessSuper { get; set; }
        public string Action { get; set; }
        public string Remark { get; set; }
        public System.DateTime AddTime { get; set; }
    }
}