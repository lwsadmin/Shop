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
    
    public partial class TRushBuyOrder
    {
        public System.Guid Guid { get; set; }
        public System.Guid RushBuyGuid { get; set; }
        public string OrderNumber { get; set; }
        public System.Guid MemberGuid { get; set; }
        public System.Guid BusinessGuid { get; set; }
        public System.Guid OperatorGuid { get; set; }
        public System.DateTime BeginTime { get; set; }
        public System.DateTime EndTime { get; set; }
        public int Count { get; set; }
        public decimal TotalMoney { get; set; }
        public decimal TotalPay { get; set; }
        public decimal ValuePaid { get; set; }
        public decimal WeiXinPaid { get; set; }
        public decimal AliPaid { get; set; }
        public decimal PointPaid { get; set; }
        public decimal Point { get; set; }
        public int Status { get; set; }
        public string RefundNO { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}
