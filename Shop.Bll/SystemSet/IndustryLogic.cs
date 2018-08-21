using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using System.Web.Mvc;
namespace Shop.Bll
{
    public class IndustryLogic
    {

        //public List<sele>

        public static bool Add(TIndustry model, out string msg)
        {
            msg = "保存成功!";
            dbContext db = new dbContext();
            if (model.Guid == Guid.Empty)
            {
                model.Guid = Guid.NewGuid();
                db.TIndustry.Add(model);
                db.SaveChanges();
                return true;

            }

            TIndustry oldIndustry = db.TIndustry.Where(c => c.Guid == model.Guid).FirstOrDefault();
            if (oldIndustry == null)
            {
                msg = "该文件不存在，可能已经被删除!";
                return false;
            }
            oldIndustry.Title = model.Title;
            oldIndustry.Sort = model.Sort;
            oldIndustry.ParentGuid = model.ParentGuid;
            if (oldIndustry.Icon != model.Icon)
            {
                CommonLogic.DeleteImages(oldIndustry.Icon);

            }
            oldIndustry.Icon = model.Icon;
            db.SaveChanges();
            return true;
        }


        public static bool Delete(Guid Guid, out string msg)
        {
            dbContext db = new dbContext();

            msg = "删除成功！";
            List<TBusiness> businessList = db.TBusiness.Where(c => c.IndustryGuid == Guid).ToList();
            if (businessList.Count() > 0)
            {
                msg = "该行业包含商户数据,无法删除!";
                return false;
            }
            TIndustry industry = db.TIndustry.Where(c => c.Guid == Guid).FirstOrDefault();
            int industryListCount = db.TIndustry.Where(c => c.ParentGuid == c.Guid).Count();
            if (industryListCount > 0)
            {
                msg = "该行业包含子级行业,无法删除!";
                return false;
            }
            db.TIndustry.Remove(industry);
            db.SaveChanges();
            return true;
        }


        public static IEnumerable<SelectListItem> GetDropDownList(Guid OperatorGuid)
        {
            dbContext db = new dbContext();
            var s = from c in db.TIndustry
                    where c.OperatorGuid == OperatorGuid

                    select (new SelectListItem() { Text = c.Title, Value = c.Guid.ToString() });

            return (IQueryable<SelectListItem>)s;
        }
    }
}
