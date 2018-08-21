using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.ViewModel;
using Shop.Dal.Emnu;
namespace Shop.Bll
{
    public class ChainStoreLogic
    {

        public static TChainStore GetEntity(Guid guid)
        {
            dbContext db = new dbContext();
            return db.TChainStore.Where(c => c.Guid == guid).FirstOrDefault();
        }
        public static bool Add(StoreView view)
        {
            dbContext db = new dbContext();

            TChainStore model = view.store;
            if (model.Guid == Guid.Empty)
            {
                model.Guid = Guid.NewGuid();
                model.CreateTime = DateTime.Now;
                db.TChainStore.Add(model);
            }
            if (view.storeImges.Where(c => c.Url != "" && c.Url != null).Count() > 0)
            {
                for (int i = 0; i < view.storeImges.Where(c => c.Url != "").Count(); i++)
                {
                    view.storeImges[i].Type = (int)ImageType.门店;
                    view.storeImges[i].UploadTime = DateTime.Now;
                    view.storeImges[i].Sort = 0;
                    view.storeImges[i].ObjectGuid = model.Guid;
                }
                db.TImage.AddRange(view.storeImges.Where(c => c.Url != "" && c.Url != null));
            }
            TBusiness business = db.TBusiness.Where(c => c.Guid == model.BusinessGuid).FirstOrDefault();
            business.StoreCount += 1;
            db.SaveChanges();
            return true;
        }
        public static bool Edit(StoreView view, out string msg)
        {
            dbContext db = new dbContext();
            TChainStore model = view.store;
            TChainStore oldStore = db.TChainStore.Where(c => c.Guid == model.Guid).FirstOrDefault();
            if (oldStore == null)
            {
                msg = "该门店不存在，可能已经被删除!";
                return false;
            }
            oldStore.Name = model.Name;
            oldStore.BusinessGuid = model.BusinessGuid;
            oldStore.Contact = model.Contact;
            oldStore.Mobile = model.Mobile;
            oldStore.AveragePrice = model.AveragePrice;
            oldStore.TakeoutPrice = model.TakeoutPrice;

            oldStore.IsTakeOut = model.IsTakeOut;
            oldStore.IsTakeoutSms = model.IsTakeoutSms;
            oldStore.Sort = model.Sort;
            if (model.Image != oldStore.Image && !string.IsNullOrEmpty(oldStore.Image))
            {
                CommonLogic.DeleteImages(oldStore.Image);
            }
            oldStore.Image = model.Image;
            oldStore.ProvinceId = model.ProvinceId;
            oldStore.CityId = model.CityId;
            oldStore.DistrictId = model.DistrictId;
            oldStore.Address = model.Address;
            oldStore.Memo = model.Memo;
            oldStore.Introduce = model.Introduce;
            if (db.GetValidationErrors().Count() == 0)
            {
                List<TImage> oldList = db.TImage.Where(c => c.ObjectGuid == model.Guid).ToList();
                if (oldList != null && oldList.Count > 0)
                {
                    db.TImage.RemoveRange(oldList);
                }

                if (view.storeImges != null && view.storeImges.Count > 0)
                {
                    db.TImage.AddRange(view.storeImges);
                }
                db.SaveChanges();
                msg = "保存成功!";
                return true;
            }
            else
            {
                msg = db.GetValidationErrors().FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                return false;
            }

        }
        public static bool SaveMap(Guid guid, string lng, string lat, out string msg)
        {
            dbContext db = new dbContext();
            TChainStore store = db.TChainStore.Where(c => c.Guid == guid).FirstOrDefault();
            if (store == null)
            { msg = "该门店不存在，可能已经被删除!"; return false; }
            store.Longitude = lng;
            store.Latitude = lat;
            db.SaveChanges();
            msg = "地图位置标记成功!";
            return true;
        }

    }
}
