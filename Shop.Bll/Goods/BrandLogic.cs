using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Help;
using Shop.Dal;
using Shop.Dal.Emnu;
using System.Web.Mvc;
namespace Shop.Bll
{
    public class BrandLogic
    {
        public static bool Add(TBrand brand, out string msg)
        {
            dbContext db = new dbContext();

            if (brand.Guid == Guid.Empty)
            {
                brand.Guid = Guid.NewGuid();
            }
            TBrand b = db.TBrand.Where(c => c.OperatorGuid == brand.OperatorGuid && c.Title == brand.Title).FirstOrDefault();
            if (b == null)
            {
                db.TBrand.Add(brand);
                db.SaveChanges();
                msg = "添加成功!";
                return true;
            }
            msg = "已经存在该品牌名称，请勿重复添加!";
            return false;
        }

        public static bool Edit(TBrand brand, out string msg)
        {
            dbContext db = new dbContext();
            TBrand oldBrand = db.TBrand.Where(c => c.Guid == brand.Guid).FirstOrDefault();
            if (oldBrand == null)
            {
                msg = "该品牌不存在,可能已经被删除!";
                return false;
            }

            oldBrand.Title = brand.Title;
            oldBrand.Sort = brand.Sort;
            oldBrand.Url = brand.Url;
            if (oldBrand.Logo != brand.Logo)
            {
                CommonLogic.DeleteImages(oldBrand.Logo);
            }
            oldBrand.Logo = brand.Logo;
            oldBrand.Remark = brand.Remark;
            msg = "修改成功!";
            db.SaveChanges();
            return true;
        }

        public static bool Delete(Guid guid, out string msg)
        {
            dbContext db = new dbContext();

            TBrand brand = db.TBrand.Where(c => c.Guid == guid).FirstOrDefault();
            if (brand == null)
            {
                msg = "该品牌不存在,可能已经被删除!";
                return false;
            }
            if (!string.IsNullOrEmpty(brand.Logo))
            {
                CommonLogic.DeleteImages(brand.Logo);
            }
            db.TBrand.Remove(brand);
            db.SaveChanges();
            msg = "删除成功!";
            return true;
        }

        public static List<SelectListItem> GetBrandSelectList(Guid OperatorGuid)
        {
            dbContext db = new dbContext();
            List<SelectListItem> data = db.TBrand.Where(c => c.OperatorGuid == OperatorGuid).OrderBy(c => c.Sort).Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Guid.ToString()
            }).ToList();
            //List<SelectListItem> itemlist = new List<SelectListItem>();
            //foreach (var item in data)
            //{
            //    SelectListItem s = new SelectListItem();
            //    s.Text = item.Title;
            //    s.Value = item.Guid.ToString();
            //    itemlist.Add(s);
            //}
          
            return data;
        }
    }

}
