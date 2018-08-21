using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Dal;
using Shop.Bll;
namespace Shop.Bll
{
    public class TRushBuyLogic
    {
        public static TRushBuy GetEntity(Guid guid)
        {
            DalGeneric<TRushBuy> dal = new DalGeneric<TRushBuy>();
            return dal.GetBy(c => c.Guid == guid);
        }

        public static bool Add(TRushBuy model, out string msg)
        {
            dbContext db = new dbContext();
            if (model.Guid == Guid.Empty)
            {
                model.Guid = Guid.NewGuid();
                model.AddTime = DateTime.Now;
                db.TRushBuy.Add(model);
                db.SaveChanges();
                msg = "添加成功!";
                return true;
            }

            TRushBuy oldModel = db.TRushBuy.Where(c => c.Guid == model.Guid).FirstOrDefault();
            if (oldModel == null)
            {
                msg = "该抢购活动不存在,可能已经被删除!"; return false;
            }

            oldModel.Title = model.Title;
            oldModel.Price = model.Price;
            oldModel.MarketPrice = model.MarketPrice;
            oldModel.Status = model.Status;
            oldModel.StoreCount = model.StoreCount;
            if (oldModel.Images != model.Images)
            {
                CommonLogic.DeleteImages(oldModel.Images);
            }
            oldModel.Images = model.Images;
            oldModel.SellCount = model.SellCount;
            oldModel.DefaltSellCount = model.DefaltSellCount;
            oldModel.NeedBook = model.NeedBook;
            oldModel.Sort = model.Sort;
            oldModel.BeginTime = model.BeginTime;
            oldModel.EndTime = model.EndTime;
            oldModel.Detail = model.Detail;
            oldModel.BusinessGuid = model.BusinessGuid;
            db.SaveChanges();
            msg = "保存成功!";
            return true;
        }

        public static bool Delete(string guids)
        {
            return true;
        }

        public bool ReplyComment(Guid guid)
        {


            return true;
        }
    }
}
