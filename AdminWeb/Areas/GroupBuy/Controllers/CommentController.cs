using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Bll;
using Shop.Dal;
using Shop.Entity;
using Shop.Help;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
using Shop.Dal.Emnu;
using System.Data;
namespace AdminWeb.Areas.GroupBuy.Controllers
{
    public class CommentController : AdminBaseController
    {
        // GET: GroupBuy/Comment
        public ActionResult List(int pageIndex = 1)
        {
            string where = CommonLogic.GetWhereCondition();
            where += string.Format(" and Type={0} and ParentGuid is null", (int)CommentType.GroupBuy);
            if (!string.IsNullOrEmpty(Request.Form["title"]))
            {
                where += string.Format(" and  Content like '%{0}%'", Request.Form["title"]);
            }
            int pageSize = 15;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            if (!Request.IsAjaxRequest())
            {

            }
            string table = string.Format(@"(select c.OperatorGuid,c.Content,c.Images,c.StartLevel,c.SubmitTime
,c.Status,c.ParentGuid,c.OppositionCount,c.SupportCount,o.OrderNumber,c.Guid,
b.Title,m.Name,m.HeadImg,c.Type
 from TComment c 
left join TRushBuyOrder o on c.ObjectGuid=o.Guid 
left join TRushBuy b on o.RushBuyGuid=b.Guid
left join TMember m on c.MemberGuid=m.Guid

)Tab");
            ViewData["NowPageSize"] = pageSize;
            int totol;
            DalGeneric<object> dal = new DalGeneric<object>();
            DataSet ds = dal.GetPaged(pageIndex, pageSize, table, "*", where, " SubmitTime desc", out totol);

            string whereReply = string.Empty;
            if (ds.Tables[0].Rows.Count > 0)
            {
                whereReply += " 1=1 and ParentGuid in (";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {

                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        whereReply += string.Format("'{0}'", ds.Tables[0].Rows[i]["Guid"]);
                    }
                    else { whereReply += string.Format("'{0}',", ds.Tables[0].Rows[i]["Guid"]); }

                }
                whereReply += ")";
            }
            ViewData["Reply"] = new DataTable();
            if (!string.IsNullOrEmpty(whereReply))
            {
                int replyTotal;
                ViewData["Reply"] = dal.GetPaged(pageIndex, int.MaxValue, table, "*", whereReply, " SubmitTime desc", out replyTotal).Tables[0];

            }
            PagedList<DataRow> pageList = new PagedList<DataRow>(ds.Tables[0].Select(), pageIndex, pageSize, totol);
            return View(pageList);
        }

        [HttpPost]
        public ActionResult Reply(Guid guid)
        {
            string content = Request.Form["content"];
            TComment com = new TComment();
            com.Guid = Guid.NewGuid();
            com.Content = content;
            com.ParentGuid = guid;
            com.StartLevel = 0;
            com.Status = 0;
            com.SubmitTime = DateTime.Now;
            com.OppositionCount = 0;
            com.SupportCount = 0;
            com.OperatorGuid = this.OperatorGuid;
            DalGeneric<TComment> dal = new DalGeneric<TComment>();
            int Qon = dal.Add(com);
            if (Qon > 0)
            {
                return Json(new { succ = true });
            }
            else
            {
                return Json(new { succ = false });
            }
        }
    }
}