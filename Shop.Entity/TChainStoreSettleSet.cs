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
    
    public partial class TChainStoreSettleSet
    {
        public System.Guid Guid { get; set; }
        public System.Guid OperatorGuid { get; set; }
        public Nullable<System.Guid> ChainStoreGuid { get; set; }
        public System.Guid MemberCategoryGuid { get; set; }
        public decimal OffLineConsumeDiscount { get; set; }
        public decimal OffLineConsumeRebate { get; set; }
        public decimal OnLineConsumeRebate { get; set; }
        public decimal PointPaidRebate { get; set; }
        public decimal BusinessProfitProportion { get; set; }
        public decimal MemberProfitProportion { get; set; }
        public decimal MemberSaleProfitProportion { get; set; }
        public string Memo { get; set; }
    }
}
