using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Help;
using Shop.Dal;
using Shop.Dal.Emnu;
using System.Web;
using System.Data;
using System.Data.Sql;
using System.Web.Mvc;
namespace Shop.Bll
{
    public class GoodsCategoryLogic
    {

        public TGoodsCategory GetModel(Guid guid)
        {
            dbContext db = new dbContext();
            return db.TGoodsCategory.Where(c => c.Guid == guid).FirstOrDefault();
        }
        public bool Add(TGoodsCategory GoodsCategory, out string msg)
        {
            if (GoodsCategory == null)
            {
                msg = "错误!没有检测到分类信息!";
                return false;
            }
            dbContext db = new dbContext();
            if (GoodsCategory.Guid == Guid.Empty)
            {

                GoodsCategory.Guid = Guid.NewGuid();
                GoodsCategory.AddTime = DateTime.Now;
                db.TGoodsCategory.Add(GoodsCategory);
                db.SaveChanges();
                msg = "添加成功!";
                return true;
            }
            else
            {
                TGoodsCategory oldCate = db.TGoodsCategory.Where(c => c.Guid == GoodsCategory.Guid).FirstOrDefault();
                if (oldCate == null)
                {
                    msg = "该分类不存在,可能已经被删除!";
                    return false;
                }
                oldCate.Brand = GoodsCategory.Brand;
                oldCate.Title = GoodsCategory.Title;
                oldCate.Remark = GoodsCategory.Remark;
                oldCate.ParentGuid = GoodsCategory.ParentGuid;
                oldCate.Sort = GoodsCategory.Sort;
                if (db.GetValidationErrors().Count() == 0)
                {
                    db.SaveChanges();
                    msg = "修改成功!";
                    return true;
                }
                else
                {
                    msg = db.GetValidationErrors().FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                    return false;
                }
            }
        }


        public bool Delete(Guid guid, out string msg)
        {
            dbContext db = new dbContext();
            TGoodsCategory model = db.TGoodsCategory.Where(c => c.Guid == guid).FirstOrDefault();
            if (model == null)
            {
                msg = "该分类不存在,可能已经被删除!";
                return false;
            }
            if (db.TGoodsCategory.Where(c => c.ParentGuid == guid).Count() > 0)
            {
                msg = "该分类下存在子级分类,请先删除其子分类!";
                return false;
            }
            db.TGoodsCategory.Remove(model);
            db.SaveChanges();
            msg = "删除成功!";
            return true;

        }




        public object GetJsonByParentGuid(Guid guid)
        {
            dbContext db = new dbContext();

            var data = from c in db.TGoodsCategory
                       where c.ParentGuid == guid
                       select new
                       {
                           Guid = c.Guid,
                           ParentGuid = c.ParentGuid,
                           Title = c.Title,
                           Sort = c.Sort,
                           Remark = c.Remark.ToString(),
                           AddTime = c.AddTime//.Year + "-" + c.AddTime.Month + "-" + c.AddTime.Day
                       };

            return data;

        }

        public SelectList GetSelectGoodsCategory(Guid OperatorGuid, Guid businessGuid, int Type)
        {
            dbContext db = new dbContext();
            List<usp_GoodsCategory> goodsCate = db.Database.SqlQuery<usp_GoodsCategory>
                (string.Format("exec [usp_SelectGoodsCategory] '{0}' ,'{1}',{2}", OperatorGuid, businessGuid, Type)).ToList();
            //var usp_SelectBaseTypeChild = db.usp_SelectGoodCategoryBusinessGuid(businessGuid.ToString(), type);
            foreach (usp_GoodsCategory row in goodsCate)
            {
                if (row.Level > 0)
                {
                    row.Title = "┣" + row.Title;
                }
                for (int i = 0; i < row.Level; i++)
                {
                    row.Title = HttpUtility.HtmlDecode("&nbsp;&nbsp;") + row.Title;
                }
            }
            SelectList selectList = new SelectList(goodsCate, "Guid", "Title");
            //List<SelectListItem> data = goodsCate.Select(c => new SelectListItem
            //{
            //    Text = c.Title,
            //    Value = c.Guid.ToString()
            //}).ToList();
            return selectList;
        }
    }
}
