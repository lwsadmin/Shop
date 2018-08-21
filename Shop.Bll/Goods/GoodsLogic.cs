using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.ViewModel;
using System.Data.Entity;

namespace Shop.Bll
{
    public class GoodsLogic
    {
        public bool Add(GoodsModel model, out string msg)
        {
            dbContext db = new dbContext();
            msg = string.Empty;
            DbContextTransaction tran = db.Database.BeginTransaction();
            try
            {
                if (model.goods.Guid == Guid.Empty)
                {
                    model.goods.Guid = Guid.NewGuid();
                    model.goods.AddTime = DateTime.Now;

                    db.TGoods.Add(model.goods);
                    db.SaveChanges();
                    for (int i = 0; i < model.goodsImges.Count; i++)
                    {
                        model.goodsImges[i].Guid = Guid.NewGuid();
                        model.goodsImges[i].UploadTime = DateTime.Now;
                        model.goodsImges[i].ObjectGuid = model.goods.Guid;
                    }
                    db.TImage.AddRange(model.goodsImges);
                    if (db.GetValidationErrors().Count() == 0)
                    {
                        db.SaveChanges();
                        msg = "添加成功！";
                        tran.Commit();
                        return true;
                    }
                    else
                    {
                        tran.Rollback();
                        msg = db.GetValidationErrors().FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                        return false;
                    }
                }
                else
                {
                    TGoods old = db.TGoods.Where(c => c.Guid == model.goods.Guid).FirstOrDefault();
                    if (old == null)
                    {
                        msg = "该商品不存在,可能已经被删除!";
                        return false;
                    }
                    old.Title = model.goods.Title;
                    old.BusinessGuid = model.goods.BusinessGuid;
                    old.OnLinePrice = model.goods.OnLinePrice;
                    old.MarketPrice = model.goods.MarketPrice;
                    old.GoodsCategoryGuid = model.goods.GoodsCategoryGuid;
                    old.BrandGuid = model.goods.BrandGuid;
                    old.AllowDistribution = model.goods.AllowDistribution;
                    old.DistributionMoney = model.goods.DistributionMoney;
                    old.Status = model.goods.Status;
                    old.Sort = model.goods.Sort;
                    old.StoreCount = model.goods.StoreCount;
                    old.Detail = model.goods.Detail;

                    List<TImage> oldImageList = db.TImage.Where(c => c.ObjectGuid == model.goods.Guid).ToList();
                    foreach (var item in oldImageList)
                    {
                        item.Url = model.goodsImges.Where(c => c.Guid == item.Guid).FirstOrDefault().Url;
                    }
                    db.SaveChanges();
                    msg = "保存成功！";
                    tran.Commit();
                    return true;

                }
            }
            catch (Exception e)
            {
                tran.Rollback();
                LogLogic.WriteErrorLog(e.StackTrace + e.Message);
                msg = e.Message;
                return false;
            }

        }




    }
}
