using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using Shop.Bll;
using Shop.Dal;
using Shop.Entity;
using Shop.Dal.Emnu;
using Shop.AdminWeb.Controllers;
using System.Data;
using Shop.Bll.ArticleInfo;

namespace Shop.AdminWeb.Areas.ArticleInfo.Controllers
{
    public class CommentController : AdminBaseController
    {
        // GET: ArticleInfo/Comment
        public ActionResult List(int id = 1)
        {
            //不作商户限制，资讯商户也可以查看
            string where = string.Format("1=1 and operatorGuid='{0}' and type=1 and parentGuid is null", this.OperatorGuid);
            DalGeneric<TComment> Dal = new DalGeneric<TComment>();
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
            string table = @"(select c.OperatorGuid,a.Title,c.SupportCount,c.Type,c.parentGuid, c.Guid,c.Content,c.Status,c.SubmitTime,m.HeadImg,m.Name from TComment c 
left join TMember m on c.MemberGuid=m.Guid left join TArticle a on c.ObjectGuid=a.Guid) Tab";
            int total;
            DataSet ds = Dal.GetPaged(id, pageSize, table, "*", where, "SubmitTime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);

            return View(pagelist);

        }


        public JsonResult Reply()
        {
            string Guid = Request.Form["guid"];
            string Content = Request.Form["content"];

            CommentLogic logic = new CommentLogic();
            string msg = "";
            bool flag = logic.Reply(new Guid(Guid), Content, out msg);
            return Json(new { succ = flag, msg = msg });
        }

        public JsonResult Change()
        {
            string Guid = Request.Form["guid"];


            CommentLogic logic = new CommentLogic();
            string msg = "";
            bool flag = logic.ChangeStatus(new Guid(Guid), out msg);
            return Json(new { succ = flag, msg = msg });
        }
    }
}