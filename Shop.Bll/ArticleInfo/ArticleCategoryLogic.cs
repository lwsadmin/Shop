using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Dal;
using Shop.Entity;
using Shop.Help;
using System.Web.Mvc;
namespace Shop.Bll
{
    public class ArticleCategoryLogic
    {
        public static bool Add(TArticleCategory model, out string msg)
        {
            msg = "保存成功!";
            DalGeneric<TArticleCategory> dal = new DalGeneric<TArticleCategory>();
            int count = 0;
            if (model.Guid == Guid.Empty)
            {
                model.Guid = Guid.NewGuid();
                model.AddTime = DateTime.Now;
                count = dal.Add(model);
                if (count > 0)
                    return true;
                else
                    return false;
            }
            TArticleCategory oldModel = dal.GetBy(c => c.Guid == model.Guid);
            oldModel.Title = model.Title;
            oldModel.Remark = model.Remark;
            oldModel.Sort = model.Sort;

            if (dal.Modify(oldModel, "Title", "Sort", "Remark") > 0)

                return true;
            else
                return false;
        }


        public static bool Delete(string Guid, out string msg)
        {
            msg = "删除成功！";
            DalGeneric<TArticleCategory> dal = new DalGeneric<TArticleCategory>();
            dbContext db = new dbContext();

            Guid gd = new Guid(Guid.ToString());
            TArticleCategory cateModel = dal.GetBy(c => c.Guid == gd);
            if (cateModel == null)
            { msg = "该类别不存在，可能已经被删除!"; return false; }
            int articlesCount = db.TArticle.Where(c => c.CategoryGuid == gd).Count();
            if (articlesCount > 0)
            {
                msg = "资讯类别【" + cateModel.Title + "】下包含" + articlesCount + "条资讯，不能删除!";
                return false;
            }

            if (dal.Del(cateModel) > 0)
            { return true; }
            else { msg = "删除失败!"; return false; }

        }


        public static  List<SelectListItem> GetDropdownList(Guid BusinessGuid)
        {
            dbContext db = new dbContext();
            //      SelectList list=new SelectList ()
            List<SelectListItem> itemList = new List<SelectListItem>();
            IQueryable<SelectListItem> obj = db.TArticleCategory.Where(c => c.BusinessGuid == BusinessGuid).Select(c => new SelectListItem
            {
                Text = c.Title,
                Value = c.Guid.ToString()
            });
            //foreach (var item in obj)
            //{
            //    SelectListItem selectItem = new SelectListItem();
            //    selectItem.Text = item.Title;
            //    selectItem.Value = item.Guid.ToString();
            //    itemList.Add(selectItem);
            //}
            return obj.ToList();
        }
        public static TArticleCategory GetModel(Guid guid)
        {
            dbContext db = new dbContext();
            return db.TArticleCategory.Where(c => c.Guid == guid).FirstOrDefault();
        }

        public static List<TArticleCategory> GetCategoryList()
        {
            dbContext db = new dbContext();
            return db.TArticleCategory.ToList();
        }
    }
}
