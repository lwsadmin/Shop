using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Help;
using Shop.Dal;
using Shop.Dal.Emnu;
namespace Shop.Bll
{
    public class GiftLogic
    {
        public static bool Add(TGift gift, out string msg)
        {
            msg = "保存成功!";
            dbContext db = new dbContext();
            if (gift.Guid == Guid.Empty)
            {
 
                gift.Guid = Guid.NewGuid();
                gift.CreateTime = DateTime.Now;
                gift.Status = 0;
                gift.ExchangeCount = 0;
                db.TGift.Add(gift);
                db.SaveChanges();
                return true;
            }

            TGift old = db.TGift.Where(c => c.Guid == gift.Guid).FirstOrDefault();
            if (old == null)
            {
                msg = "该礼品不存在,可能已经被删除!";
                return false;
            }

            old.Name = gift.Name;
            old.GiftCategoryGuid = gift.GiftCategoryGuid;
            old.Image = gift.Image;
            if (!string.IsNullOrEmpty(old.Image) && !old.Image.Equals(gift.Image))
            {
                CommonLogic.DeleteImages(old.Image);
            }
            old.BusinessGuid = gift.BusinessGuid;
            old.SingleLimit = gift.SingleLimit;
            old.Point = gift.Point;
            old.Sort = gift.Sort;
            old.Memo = gift.Memo;
            old.Status = gift.Status;
            old.ExchangeAddress = gift.ExchangeAddress;
            old.ExchangeNotes = gift.ExchangeNotes;
            old.ExchangeType = gift.ExchangeType;
            old.Description = gift.Description;
            old.StockCount = gift.StockCount;
            db.SaveChanges();
            return true;
        }
    }
}
