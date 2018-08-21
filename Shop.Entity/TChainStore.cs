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
    
    public partial class TChainStore
    {
        public System.Guid Guid { get; set; }
        public System.Guid OperatorGuid { get; set; }
        public string Name { get; set; }
        public System.Guid BusinessGuid { get; set; }
        public bool IsSystem { get; set; }
        public string Contact { get; set; }
        public string Mobile { get; set; }
        public string Image { get; set; }
        public int ProvinceId { get; set; }
        public int CityId { get; set; }
        public int DistrictId { get; set; }
        public string Address { get; set; }
        public string Longitude { get; set; }
        public string Latitude { get; set; }
        public bool IsTakeOut { get; set; }
        public decimal AveragePrice { get; set; }
        public decimal TakeoutPrice { get; set; }
        public bool IsTakeoutSms { get; set; }
        public int AvailableSmsCount { get; set; }
        public int TotalSmsCount { get; set; }
        public decimal TotalPoint { get; set; }
        public decimal AvailablePoint { get; set; }
        public decimal TotalValue { get; set; }
        public decimal AvailableValue { get; set; }
        public decimal SettlementMoney { get; set; }
        public string Memo { get; set; }
        public int Sort { get; set; }
        public string Introduce { get; set; }
        public System.DateTime CreateTime { get; set; }
    }
}