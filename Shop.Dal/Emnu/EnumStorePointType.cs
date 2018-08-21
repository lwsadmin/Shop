using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Dal.Emnu
{
    public enum EnumStorePointType
    {
        /// <summary>
        ///  购买积分 BuyPoint
        /// </summary>
        购买积分 = 1,

        /// <summary>
        ///  快捷消费
        /// </summary>
        QuickConsume = 2,

        /// <summary>
        ///  商品消费
        /// </summary>
        GoodsConsume = 3,

        /// <summary>
        /// 增加计次
        /// </summary>
        AddCount = 4,

        /// <summary>
        ///  赠送会员积分
        /// </summary>
        GiftMemberPoint = 5,

        /// <summary>
        ///  订单消费
        /// </summary>
        OrderConsume = 6,

        /// <summary>
        ///  销售积分
        /// </summary>
        SellPoint = 7,
        /// <summary>
        /// 团购积分
        /// </summary>
        GroupOrderConsume = 8
    }
}
