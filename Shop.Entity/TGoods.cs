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
    
    public partial class TGoods
    {
        public System.Guid Guid { get; set; }
        public System.Guid OperatorGuid { get; set; }
        public System.Guid BusinessGuid { get; set; }
        public System.Guid ChainStoreGuid { get; set; }
        public System.Guid GoodsCategoryGuid { get; set; }
        public Nullable<System.Guid> BrandGuid { get; set; }
        public string Barcode { get; set; }
        public string Title { get; set; }
        public decimal MarketPrice { get; set; }
        public decimal OnLinePrice { get; set; }
        public Nullable<bool> AllowDistribution { get; set; }
        public Nullable<decimal> DistributionMoney { get; set; }
        public bool IsTakeOut { get; set; }
        public int Status { get; set; }
        public int Sort { get; set; }
        public string StoreCount { get; set; }
        public string SellCount { get; set; }
        public string Detail { get; set; }
        public System.DateTime AddTime { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string Parameter3 { get; set; }
        public string Parameter4 { get; set; }
        public string Parameter5 { get; set; }
        public string Parameter6 { get; set; }
        public string Parameter7 { get; set; }
        public string Parameter8 { get; set; }
        public string Parameter9 { get; set; }
        public string Parameter10 { get; set; }
    }
}
