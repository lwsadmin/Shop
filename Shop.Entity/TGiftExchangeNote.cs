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
    
    public partial class TGiftExchangeNote
    {
        public System.Guid Guid { get; set; }
        public System.Guid OperatorGuid { get; set; }
        public string BillNumber { get; set; }
        public System.Guid MemberGuid { get; set; }
        public decimal TotalPoint { get; set; }
        public string RelationBillNumber { get; set; }
        public string UserAccount { get; set; }
        public System.Guid ChainStoreGuid { get; set; }
        public System.DateTime OpertorTime { get; set; }
        public string Memo { get; set; }
    }
}
