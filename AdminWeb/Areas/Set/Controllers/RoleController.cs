using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Entity;
using Shop.Dal;
using Shop.Bll;
using Shop.Help;
using System.Data;
using Webdiyer.WebControls.Mvc;
using System.Xml;
using Shop.Dal.xml;
using Shop.AdminWeb.Controllers;

namespace AdminWeb.Areas.Set.Controllers
{

    public class RoleController : AdminBaseController
    {
        // GET: cms/Role
        public ActionResult List(int id = 1)
        {
            dbContext db = new dbContext();
            DalGeneric<TManagerRole> DAL = new DalGeneric<TManagerRole>();
            int total; int pageSize = 15;
            string where = RoleLogic.GetManageRoleWhere();
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            if (!string.IsNullOrEmpty(Request["Business"]))
            {
                where += string.Format("  and BusinessGuid='{0}'", Request["Business"].Trim());
            }
            if (!string.IsNullOrEmpty(Request["title"]))
            {
                where += string.Format("  and title like'%{0}%'", Request["title"].Trim());
            }
            string table = string.Format(@"(select r.Guid,r.Title,r.AddTime,r.Remark
,b.BusinessName,r.IsSystem,r.OperatorGuid,r.BusinessGuid
 from TManagerRole r
left join TBusiness b on
r.BusinessGuid=b.Guid where r.IsSystem=0)Tab");
            LogLogic.WriteErrorLog(where);
            DataSet ds = DAL.GetPaged(id, pageSize, table, "*", where, "addTime desc", out total);

            PagedList<DataRow> RowList = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);

            if (Request.IsAjaxRequest())
            {
                return View("_Table", RowList);
            }
            ViewData["Business"] = BusinessLogic.GetSelect(this.manger.ManagerRoleGuid);
            return View(RowList);
        }
        [HttpGet]
        public ActionResult Add(string id)
        {
            TManagerRole role = new TManagerRole();
            role.ManageRole = "";
            DalGeneric<TManagerRole> dal = new DalGeneric<TManagerRole>();
            if (!string.IsNullOrEmpty(id))
            {
                role = dal.GetBy(c => c.Guid == new Guid(id));
            }
            else
            {
                role.OperatorGuid = this.OperatorGuid;
            }
            XmlDocument Xml = new XmlData().MenuXml;
            XmlNodeList FirstMenuNodeList = Xml.SelectNodes("//Menu//First");
            ViewBag.NodeList = FirstMenuNodeList;
            Dictionary<string, string> actionDic = new Dictionary<string, string>();
            if (!string.IsNullOrEmpty(role.Action))
            {
                List<Power> PowerList = (List<Power>)JsonHelper.JsonToObject(role.Action, new List<Power>());
                foreach (Power item in PowerList)
                {
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        actionDic.Add(item.Name, item.ActionString == null ? "" : item.ActionString);
                    }
                }
            }
            ViewData["Action"] = actionDic;
            ViewData["ManageRole"] = RoleLogic.GetManageRole();
            ViewData["Business"] = BusinessLogic.GetSelect(this.manger.ManagerRoleGuid);
            return View(role);
        }
        [HttpPost]
        public JsonResult Add(TManagerRole role)
        {
            if (!string.IsNullOrEmpty(Request["searchablems2side__dx"]))
            {
                string s = Request["searchablems2side__dx"];
                role.ManageRole = Request["searchablems2side__dx"];
            }
            RoleLogic roleBll = new RoleLogic();
            List<Power> PowerList = new List<Power>();
            XmlDocument xml = new XmlData().MenuXml;
            string[] requestMenu = Request.Form["requestMenu"].Split(',');
            
            for (int i = 0; i < requestMenu.Length; i++)
            {
                Power pw = new Power();
                pw.Name = requestMenu[i];
                if (!string.IsNullOrEmpty(Request.Form[requestMenu[i]]))
                {
                    pw.ActionString = Request.Form[requestMenu[i]];
                }
                else { pw.ActionString = ""; }
                PowerList.Add(pw);
            }
           
            string RoleActionJsonString = JsonHelper.ObjectToJson(PowerList);

            // TempData["str"] = RoleActionJsonString;
            role.Action = RoleActionJsonString;
            bool flag = roleBll.Add(role);
            return Json(new { succ = flag });
        }


        public JsonResult Delete()
        {
            string guid = Request["Guids"];
            string msg = string.Empty;
            bool flag = RoleLogic.Delete(guid, out msg);
            return Json(new { succ = flag, msg = msg });
        }

    }
}