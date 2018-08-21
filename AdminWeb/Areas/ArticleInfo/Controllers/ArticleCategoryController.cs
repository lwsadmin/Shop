using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Shop.Bll;
using Shop.Dal;
using Shop.Entity;
using Shop.Help;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
using Shop.Dal.Emnu;
namespace Shop.AdminWeb.Areas.ArticleInfo.Controllers
{
    public class ArticleCategoryController : AdminBaseController
    {
        // GET: cms/ArticleCategory
        public ActionResult List(int id = 1)
        {
            string where = string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);
            if (!string.IsNullOrEmpty(Request["title"]))
            {
                where += string.Format(" and title like'%{0}%'", Request["title"]);
            }

            int pageSize = 15;
            if (!string.IsNullOrEmpty(Request.Form["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request.Form["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            DalGeneric<TArticleCategory> Dal = new DalGeneric<TArticleCategory>();
           
            string table = string.Format(@"( select c.*,b.BusinessName from TArticleCategory
 c left join TBusiness b on c.BusinessGuid = b.Guid) Tab");
            int total;
            DataSet ds = Dal.GetPaged(id, pageSize, table, "*", where, "sort asc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            if (Request.IsAjaxRequest())
            {
                return View("_Table", pagelist);
            }
            return View(pagelist);

        }

        [HttpPost]
        public JsonResult Add()
        {
            TArticleCategory category = new TArticleCategory();
            if (!string.IsNullOrEmpty(Request.Form["Guid"].ToString()))
            {
                category.Guid = new Guid(Request.Form["Guid"]);
            }
            category.OperatorGuid = this.OperatorGuid;
            category.BusinessGuid = this.BusinessGuid;
            category.ChainStoreGuid = this.ChainStoreGuid;
            category.Title = Request.Form["Title"];
            category.Sort = Convert.ToInt32(Request.Form["Sort"]);
            category.Remark = Request.Form["Remark"];


            string s = "";
            if (string.IsNullOrWhiteSpace(Request.QueryString["ss"]))
            {
                s = $"1212313";
            }
            string msg = string.Empty;
            return Json(new { succ = ArticleCategoryLogic.Add(category, out msg), msg = msg });
        }

        public ActionResult Add(Guid? guid)
        {
            TArticleCategory goodsCategory = new TArticleCategory();
            string where = CommonLogic.GetWhereCondition();
            // List<usp_SelectBaseTypeBusinessGuid_Result> list = TBaseTypeLogic.GetSelectBusinessGuid(this.BusinessGuid, (int)CateGory.Article);//, "Guid", "Title");
            ViewData["CateGory"] = ArticleCategoryLogic.GetDropdownList(this.BusinessGuid);
            if (guid != null)
            {
                goodsCategory = ArticleCategoryLogic.GetModel((Guid)guid);
            }
            else
            {
                goodsCategory.BusinessGuid = this.BusinessGuid;
                goodsCategory.OperatorGuid = this.OperatorGuid;
                goodsCategory.ChainStoreGuid = this.ChainStoreGuid;
            }
            return PartialView("_Add", goodsCategory);
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            Guid gd = new Guid(id);
            string msg = string.Empty;
            bool flag = ArticleCategoryLogic.Delete(id, out msg);
            return Json(new { succ = flag, msg = msg });
        }
    }
}