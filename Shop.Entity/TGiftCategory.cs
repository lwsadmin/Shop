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
    
    public partial class TGiftCategory
    {
        public System.Guid Guid { get; set; }
        public System.Guid OperatorGuid { get; set; }
        public System.Guid BusinessGuid { get; set; }
        public System.Guid ChainStoreGuid { get; set; }
        public string Title { get; set; }
        public Nullable<System.Guid> ParentGuid { get; set; }
        public int Sort { get; set; }
        public string Remark { get; set; }
        public Nullable<System.DateTime> AddTime { get; set; }
        public string Brand { get; set; }
    }
}