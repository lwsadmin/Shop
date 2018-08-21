using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Dal;

namespace Shop.Bll
{
    public class ActivityLogic
    {
        public bool AddCategory(TActivityCategory model, out string msg)
        {
            msg = "添加成功!";
            dbContext db = new dbContext();
            if (db.GetValidationErrors().Count() == 0)
            {
                if (model.Guid == Guid.Empty)
                {
                    model.Guid = Guid.NewGuid();
                    db.TActivityCategory.Add(model);
                }
                else
                {
                    TActivityCategory old = db.TActivityCategory.Where(c => c.Guid == model.Guid).FirstOrDefault();
                    if (old == null)
                    {
                        msg = "该活动不存在,可能已经被删除!";
                        return false;
                    }
                    old.Title = model.Title;
                    old.Sort = model.Sort;
                    old.Remark = model.Remark;
                }
                db.SaveChanges();
                return true;
            }
            else
            {
                msg = db.GetValidationErrors().FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                return false;
            }
        }

        public TActivityCategory GetModel(Guid id)
        {
            DalGeneric<TActivityCategory> dal = new DalGeneric<TActivityCategory>();
            return dal.GetBy(c => c.Guid == id);
        }

        public bool CategoryDelete(string Guid, out string msg)
        {
            msg = "删除成功！";
            DalGeneric<TActivityCategory> dal = new DalGeneric<TActivityCategory>();
            DalGeneric<TActivity> dalAct = new DalGeneric<TActivity>();

            Guid gd = new Guid(Guid.ToString());
            TActivityCategory cateModel = dal.GetBy(c => c.Guid == gd);
            if (cateModel == null)
            { msg = "该类别不存在，可能已经被删除!"; return false; }
            int articlesCount = dalAct.GetListBy(c => c.CategoryGuid == gd).Count();
            if (articlesCount > 0)
            {
                msg = string.Format("请先删除该活动分类下的{0}条活动！", articlesCount);
                return false;
            }

            if (dal.Del(cateModel) > 0)
            { return true; }
            else { msg = "删除失败!"; return false; }

        }
    }
}
