using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Bll;
namespace Shop.Bll
{
    public class CouponLogic
    {
        public static bool Add(TCoupon coupon, out string msg)
        {
            dbContext db = new dbContext();
            if (coupon.Guid == Guid.Empty)
            {
                coupon.Guid = Guid.NewGuid();
                coupon.CreateTime = DateTime.Now;
                if (string.IsNullOrEmpty(coupon.Image))
                {
                    coupon.Image = "/Content/cms/img/coupon.jpg";
                }
                coupon.Status = 0;
                msg = "添加成功!";
                db.TCoupon.Add(coupon);
            }
            else
            {
                msg = "保存成功!";
                TCoupon OldCoupon = db.TCoupon.Where(c => c.Guid == coupon.Guid).FirstOrDefault();
                if (OldCoupon.ParticipateStoreCount > 0 || OldCoupon.MemberUseCount > 0 || OldCoupon.MemberReceiveCount > 0)
                {
                    msg = "该代金券已经有商户参与或者领取记录,部分数据不能修改!";
                }
                else
                {
                    OldCoupon.Value = coupon.Value;
                    OldCoupon.UseValueLimit = coupon.UseValueLimit;
                }
                OldCoupon.Title = coupon.Title;
                OldCoupon.Status = coupon.Status;
                OldCoupon.TotalCount = coupon.TotalCount;
                OldCoupon.SigleReceiveCount = coupon.SigleReceiveCount;
                OldCoupon.ValidityType = coupon.ValidityType;
                OldCoupon.ValidityDay = coupon.ValidityDay;
                OldCoupon.BeginTime = coupon.BeginTime;
                OldCoupon.EndTime = coupon.EndTime;
                if (!OldCoupon.Image.Equals(coupon.Image))
                {
                    CommonLogic.DeleteImages(OldCoupon.Image);
                }
                OldCoupon.Image = coupon.Image;
                OldCoupon.Introduce = coupon.Introduce;
            }
            db.SaveChanges();
            return true;
        }

        public static TCoupon GetEntity(Guid guid)
        {
            dbContext db = new dbContext();
            return db.TCoupon.Where(c => c.Guid == guid).FirstOrDefault();
        }
    }
}
