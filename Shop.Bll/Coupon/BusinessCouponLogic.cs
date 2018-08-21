using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
namespace Shop.Bll
{
    public class BusinessCouponLogic
    {
        public static TBusinessCoupon GetEntity(Guid guid)
        {
            dbContext db = new dbContext();
            return db.TBusinessCoupon.Where(c => c.Guid == guid).FirstOrDefault();
        }

        public static bool Add(TBusinessCoupon model, out string msg)
        {
            dbContext db = new dbContext();
            msg = "保存成功!";
            if (model.Guid == Guid.Empty)
            {
                model.Guid = Guid.NewGuid();
                model.CreateTime = DateTime.Now;
                model.Status = -1;
                if (string.IsNullOrEmpty(model.Image))
                {
                    model.Image = "/Content/cms/img/coupon.jpg";
                }
                db.TBusinessCoupon.Add(model);
                db.SaveChanges();
                return true;
            }
            TBusinessCoupon oldCoupon = db.TBusinessCoupon.Where(c => c.Guid == model.Guid).FirstOrDefault();
            if (oldCoupon == null)
            {
                msg = "该商户代金券不存在,可能已经被删除!";
                return false;
            }
            if (oldCoupon.MemberReceiveCount == 0)
            {
                oldCoupon.Value = model.Value;
                oldCoupon.UseValueLimit = model.UseValueLimit;
                oldCoupon.ValidityType = model.ValidityType;
                oldCoupon.BeginTime = model.BeginTime;
                oldCoupon.EndTime = model.EndTime;
                oldCoupon.ValidityDay = model.ValidityDay;
                oldCoupon.BusinessGuid = model.BusinessGuid;
            }
            oldCoupon.Title = model.Title;
            oldCoupon.IsOverlay = model.IsOverlay;
            oldCoupon.Introduce = model.Introduce;
            oldCoupon.SigleReceiveCount = model.SigleReceiveCount;
            oldCoupon.TotalCount = model.TotalCount;
            if (!oldCoupon.Image.Equals(model.Image))
            {
                CommonLogic.DeleteImages(oldCoupon.Image);
            }
            oldCoupon.Image = model.Image;
            db.SaveChanges();
            msg = "修改成功!";
            return true;
        }

        public static bool SetStatus(Guid guid, int status, out string msg)
        {
            dbContext db = new dbContext();
            TBusinessCoupon coupon = db.TBusinessCoupon.Where(c => c.Guid == guid).FirstOrDefault();
            if (coupon == null)
            {
                msg = "该商户代金券不存在，可能已经被删除!";
                return false;
            }
            coupon.Status = status;
            db.SaveChanges();
            msg = "保存成功!";
            return true;
        }

        public static bool Delete(string Guids, out string msg)
        {
            string[] guid = Guids.Split();

            dbContext db = new dbContext();
            var list = from c in db.TBusinessCoupon
                       where Guids.Contains(c.Guid.ToString()) &&
                       c.MemberReceiveCount == 0 && c.MemberUseCount == 0
                       select c;
            if (list.Count() > 0)
            {
                db.TBusinessCoupon.RemoveRange(list);
                db.SaveChanges();
                msg = "删除成功!"; return true;
            }
            else { msg = "代金券已存在领取或使用记录,不能删除!"; return false; }

        }
    }
}
