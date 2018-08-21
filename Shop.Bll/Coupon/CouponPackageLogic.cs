using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Dal;
using Shop.Entity;
using System.Web.Mvc;
namespace Shop.Bll
{
    public class CouponPackageLogic
    {

        public static IEnumerable<SelectListItem> GetSelectOption()
        {
            dbContext db = new dbContext();
            IEnumerable<SelectListItem> result;
            SelectListItem item = new SelectListItem();
            TManager manager = ManagerLogic.GetLoginInfo();
            TManagerRole role = RoleLogic.GetEntity(manager.ManagerRoleGuid);
            if (role.IsSystem)//运营商/超管/总部管理员
            {
                // result=db.Set<TBusiness>().Where(whereLambda)
                result = from u in db.TBusinessCoupon
                         where u.OperatorGuid == role.OperatorGuid
                         orderby u.CreateTime descending
                         select new SelectListItem
                         {
                             Value = u.Guid.ToString().ToLower(),
                             Text = u.Title
                         };

                return result;
            }
            else
            {
                result = from c in db.TBusinessCoupon
                         where c.Guid == role.BusinessGuid
                         orderby c.CreateTime descending
                         select new SelectListItem
                         {
                             Text = c.Title,
                             Value = c.Guid.ToString()
                         };

                return result;
            }
        }

        public static TCouponPackage GetEntity(Guid guid)
        {
            dbContext db = new dbContext();

            return db.TCouponPackage.Where(c => c.Guid == guid).FirstOrDefault();
        }

        public static bool Add(TCouponPackage model, string itemGuid, out string msg)
        {
            if (string.IsNullOrEmpty(itemGuid))
            {
                msg = "请选择代金券!";
                return false;
            }
            dbContext db = new dbContext();
            if (model.Guid == Guid.Empty)
            {
                model.Guid = Guid.NewGuid();
                model.CreateTime = DateTime.Now;
                db.TCouponPackage.Add(model);
                db.SaveChanges();
                msg = "添加成功!";
                //    return true;
            }
            else
            {
                TCouponPackage oldModel = db.TCouponPackage.Where(c => c.Guid == model.Guid).FirstOrDefault();
                if (oldModel == null)
                {
                    msg = "该代金券包不存在，可能已经被删除!";
                    return false;
                }
                oldModel.Title = model.Title;
                oldModel.Price = model.Price;
                oldModel.Point = model.Point;
                oldModel.Memo = model.Memo;
            }
            List<TCouponPackageItem> itemList = db.TCouponPackageItem.Where(c => c.CouponPackageGuid == model.Guid).ToList();
            if (itemList.Count > 0)
            {
                db.TCouponPackageItem.RemoveRange(itemList);
                db.SaveChanges();
                itemList.Clear();
            }

            string[] itemGuidArray = itemGuid.Split(',');
            for (int i = 0; i < itemGuidArray.Length; i++)
            {
                TCouponPackageItem item = new TCouponPackageItem();
                item.Guid = Guid.NewGuid();
                item.CouponPackageGuid = model.Guid;
                item.OperatorGuid = model.OperatorGuid;
                item.Type = 0;
                item.Guid = Guid.NewGuid();
                item.ObjectGuid = new Guid(itemGuidArray[i]);
                itemList.Add(item);

            }
            db.TCouponPackageItem.AddRange(itemList);
            db.SaveChanges();
            msg = "保存成功!";
            return true;
        }


        public static List<TCouponPackageItem> GetItemList(Guid CouponPackageGuid)
        {
            dbContext db = new dbContext();
            return db.TCouponPackageItem.Where(c => c.CouponPackageGuid == CouponPackageGuid).ToList();
        }

        public static bool Delete(Guid Guid, out string msg)
        {
            dbContext db = new dbContext();
            msg = "删除成功!";
            List<TCouponPackageItem> list = db.TCouponPackageItem.Where(c => c.CouponPackageGuid == Guid).ToList();
            if (list.Count > 0)
            {
                db.TCouponPackageItem.RemoveRange(list);
            }

            TCouponPackage packAge = db.TCouponPackage.Where(c => c.Guid == Guid).FirstOrDefault();
            if (packAge == null)
            {
                msg = "删除失败,该代金券包不存在！";
                return false;
            }
            db.TCouponPackage.Remove(packAge);
            db.SaveChanges();
            return true;
        }
    }
}
