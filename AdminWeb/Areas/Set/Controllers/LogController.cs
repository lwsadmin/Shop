using Shop.Dal;
using Shop.Entity;
using Shop.AdminWeb.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
namespace AdminWeb.Areas.Set.Controllers
{
    public class LogController : AdminBaseController
    {
        // GET: SystemSet/Log
        public ActionResult List(int id = 1)
        {
            //不作商户限制，建议管理员直属
            string where = string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);
            string table = string.Format(@"(select * from tlog)tab");

            if (!string.IsNullOrEmpty(Request["title"]))
            {
                where += string.Format(" and Content like'%{0}%'", Request["title"]);
            }
            if (!string.IsNullOrEmpty(Request["TimeFrom"]))
            {
                where += string.Format(" and OperatorTime>='{0} 00:00:00'", Request["TimeFrom"]);
            }
            if (!string.IsNullOrEmpty(Request["TimeTo"]))
            {
                where += string.Format(" and OperatorTime<='{0} 23:59:59'", Request["TimeTo"]);
            }
            int total; int pageSize = 15;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            DalGeneric<object> Dal = new DalGeneric<object>();
            DataSet ds = Dal.GetPaged(id, pageSize, table, "*", where, "OperatorTime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            if (Request.IsAjaxRequest())
            {
                return PartialView("_Table", pagelist);
            }
            return View(pagelist);
        }


    }
}