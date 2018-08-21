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
using AdminWeb.App_Start;

namespace Shop.AdminWeb.Areas.ArticleInfo.Controllers
{
    public class ArticleController : AdminBaseController
    {
        // GET: cms/Article
        //    [CMSFilter(MenuName = "Article", ActionName = "Article")]
        public ActionResult List(int id = 1)
        {

            //不作商户限制，资讯商户也可以查看
            string where = string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);
            DalGeneric<TArticle> Dal = new DalGeneric<TArticle>();
            if (!string.IsNullOrEmpty(Request.Form["title"]))
            {
                where += string.Format(" and title like '%{0}%'", Request.Form["title"].ToString());
            }
            if (!string.IsNullOrEmpty(Request.Form["CateGory"]))
            {
                where += string.Format(" and  categoryguid= '{0}'", Request.Form["CateGory"].ToString());
            }
            int pageSize = 15;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            string table = @"(select c.Guid,c.Title ,c.SmallImg,c.author,c.Target,c.OperatorGuid,c.Souce,
c.Sort,c.Click,c.AddTime,c.status,c.categoryguid,g.Title  ClassTitle from 
TArticle c join TArticleCategory g on c.categoryguid=g.guid) Tab";
            int total; string fields = "Guid,Title,SmallImg,author,Target,OperatorGuid,Souce,Sort,Click,AddTime,status,categoryguid,ClassTitle";
            DataSet ds = Dal.GetPaged(id, pageSize, table, fields, where, "sort asc,addtime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Table", pagelist);
            }
            else
            {
                ViewDataAdd();
                return View(pagelist);
            }

        }
        [HttpGet]
        public ActionResult Add(string id)
        {
            ViewDataAdd();
            DalGeneric<TArticle> dal = new DalGeneric<TArticle>();
            TArticle article = new TArticle();
            if (!string.IsNullOrEmpty(id))
                article = dal.GetBy(c => c.Guid == new Guid(id));
            else
            {
                article.OperatorGuid = this.OperatorGuid;
                article.BusinessGuid = this.BusinessGuid;
                article.ChainStoreGuid = this.ChainStoreGuid;
                article.Level = ""; article.Target = "_self";
            }
            return View(article);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add(TArticle model)
        {
            model.Level = (!string.IsNullOrEmpty(Request.Form["Level"]) ? Request.Form["Level"] : "");
            bool flag = ArticleLogic.Add(model);
            return Json(new { succ = flag });

        }
        public JsonResult Delete()
        {
            bool flag = ArticleLogic.Delete(Request.Form["Guids"]);
            return Json(new { succ = flag });
        }
        public void ViewDataAdd()
        {
            string where = CommonLogic.GetWhereCondition();
            where += string.Format(" and Type={0}", CateGory.Article);
            ViewData["CateGory"] = ArticleCategoryLogic.GetDropdownList(this.BusinessGuid);
        }
    }
}