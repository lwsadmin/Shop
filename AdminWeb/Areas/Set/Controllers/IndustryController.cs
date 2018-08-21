using Shop.AdminWeb.Controllers;
using Shop.Bll;
using Shop.Dal;
using Shop.Entity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;

namespace AdminWeb.Areas.Set.Controllers
{
    public class IndustryController : AdminBaseController
    {
        // GET: Set/Industry
        public ActionResult List(int id = 1)
        {
            string where = string.Format("1=1 and OperatorGuid='{0}'", this.OperatorGuid);
            dbContext db = new dbContext();
            DalGeneric<object> dg = new DalGeneric<object>();
            int total; int pageSize = 15;
       
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            string table = string.Format(@"(select i.Guid,i.Title, i.OperatorGuid,i.Sort,
            i.Icon,i.parentGuid from TIndustry i) as Tab");
            DataSet result = dg.GetPaged(id, pageSize, table, "*", where, "sort asc  ", out total);
            PagedList<DataRow> arts = new PagedList<DataRow>(result.Tables[0].Select(), id, pageSize, total);
            if (Request.IsAjaxRequest())
            {
                return View("_Table", arts);
            }
            return View(arts);
        }

        [HttpPost]
        public JsonResult AddSave()
        {
            TIndustry industry = new TIndustry();
            if (!string.IsNullOrEmpty(Request.Form["Guid"]))
            {
                industry.Guid = new Guid(Request.Form["Guid"]);
            }
            industry.Title = Request.Form["Title"].ToString();
            industry.Icon = Request.Form["Icon"].ToString();
            industry.Sort = Convert.ToInt32(Request.Form["Sort"]);
            industry.OperatorGuid = this.OperatorGuid;
            string msg = string.Empty;
            bool flag = IndustryLogic.Add(industry, out msg);
            return Json(new { succ = flag, msg = msg });

        }

        public JsonResult Delete(string guid)
        {
            string msg = string.Empty;
            bool flag = IndustryLogic.Delete(new Guid(guid), out msg);
            return Json(new { succ = flag, msg = msg });
        }
    }
}