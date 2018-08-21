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


namespace AdminWeb.Areas.SystemNotic.Controllers
{
    public class AnnouncementController : AdminBaseController
    {
        // GET: SystemNotic/Announcement
        public ActionResult List(int pageIndex = 1)
        {

            string where = string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);
            string table = string.Format(@"(select * from TAnnouncement)tab");
            if (!string.IsNullOrEmpty(Request["title"]))
            {
                where += string.Format(" and title like'%{0}%'", Request["title"]);
            }
            if (!string.IsNullOrEmpty(Request["TimeFrom"]))
            {
                where += string.Format(" and CreateTime>='{0} 00:00:00'", Request["TimeFrom"]);
            }
            if (!string.IsNullOrEmpty(Request["TimeTo"]))
            {
                where += string.Format(" and CreateTime<='{0} 23:59:59'", Request["TimeTo"]);
            }
            int total; int pageSize = 15;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            DalGeneric<object> Dal = new DalGeneric<object>();
            DataSet ds = Dal.GetPaged(pageIndex, pageSize, table, "*", where, "CreateTime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), pageIndex, pageSize, total);
            return View(pagelist);
        }

        public JsonResult Delete()
        {
            string msg = string.Empty;
            bool flag = AnnouncementLogic.Delete(new Guid(Request.Form["Guids"]), out msg);
            return Json(new { succ = flag, msg = msg });
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add()
        {
            TAnnouncement cement = new TAnnouncement();
            cement.Title = Request.Form["Title"];
            cement.AnnouncementContent = Request.Form["hidContent"];
            cement.OperatorGuid = this.OperatorGuid;
            bool succ = AnnouncementLogic.Add(cement);
            return Json(new { succ = succ });
        }
    }
}