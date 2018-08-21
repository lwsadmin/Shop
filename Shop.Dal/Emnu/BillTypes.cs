using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shop.Dal.Emnu
{
    public enum BillTypes
    {
        /// <summary>
        ///  快捷消费 --QuickConsume
        /// </summary>
        快捷消费 = 1,

        /// <summary>
        ///  商品消费 GoodsConsume
        /// </summary>
        商品消费 = 2,

        /// <summary>
        ///  增加计次 AddCount
        /// </summary>
        增加计次 = 3,

        /// <summary>
        ///  扣除计次 CutCount
        /// </summary>
        扣除计次 = 4,

        /// <summary>
        ///  会员充值 MemberRecharge
        /// </summary>
        会员充值 = 5,

        /// <summary>
        ///  会员登记 MemberRegister
        /// </summary>
        会员登记 = 6,

        /// <summary>
        ///  店面充值 StoreRecharge
        /// </summary>
        店面充值 = 7,

        /// <summary>
        ///  店面购买积分  StoreBuyPoint
        /// </summary>
        店面购买积分 = 8,

        /// <summary>
        /// 店面购买短信  StoreBySms
        /// </summary>
        店面购买短信 = 9,

        /// <summary> 
        ///  店面结算 StoreSettlement
        /// </summary> 
        店面结算 = 10,

        /// <summary>
        ///  赠送会员积分 GiftMemberPoint
        /// </summary>
        赠送会员积分 = 11,

        /// <summary>
        ///  扣除会员积分 CutMemberPoint
        /// </summary>
        扣除会员积分 = 12,

        /// <summary>
        ///  商品订购  GoodsOrder
        /// </summary>
        商品订购 = 13,

        /// <summary>
        ///  礼品订购  GiftExchange
        /// </summary>
        礼品订购 = 14,

        /// <summary>
        ///  购买优惠券包  BuyCouponPackage
        /// </summary>
        购买优惠券包 = 15,

        /// <summary>
        /// 团购  GroupOrder
        /// </summary>
        团购 = 16,

        /// <summary>
        ///  会员签到 MemberSign
        /// </summary>
        会员签到 = 17,
        //会员提现  DrawMoney
        会员提现 = 18,
        购买短信 = 19
    }
}
