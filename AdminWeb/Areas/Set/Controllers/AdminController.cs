using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Mvc;
using Shop.Help;
using Shop.Entity;
using Shop.Dal;
using Webdiyer.WebControls.Mvc;
using System.Web;
using Shop.AdminWeb.Controllers;
using Shop.Bll;
namespace AdminWeb.Areas.Set.Controllers
{
    public class AdminController : AdminBaseController
    {
        // GET: cms/Home

        public ActionResult Center()
        {
            return View();
        }
        public ActionResult List(int id = 1)
        {

            dbContext db = new dbContext();
            DalGeneric<TManager> dg = new DalGeneric<TManager>();
            int total; int pageSize = 15;
            string table = @"(select m.Guid,m.BusinessGuid,m.ChainStoreGuid,m.Name,m.Phone,
m.LoginName,m.AddTime,m.OperatorGuid,m.Email,m.LastLoginTime,m.ManagerRoleGuid,
r.Title RoleTitle,r.IsSystem,b.BusinessName
from TManager m join TManagerRole r
 on m.ManagerRoleGuid = r.Guid
 left join TBusiness b on r.BusinessGuid=b.Guid
) as Tab";
            string where = CommonLogic.GetWhereCondition();
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            if (!string.IsNullOrEmpty(Request.Form["loginName"]))
            {
                where += string.Format(" and (loginName like'%{0}%' or Name like '%{0}%')", Request.Form["loginName"].ToString());
            }
            if (!string.IsNullOrEmpty(Request["Business"]))
            {
                where += string.Format(" and BusinessGuid ='{0}'", Request.Form["Business"]);
            }
            if (!string.IsNullOrEmpty(Request["RoleList"]))
            {
                where += string.Format(" and ManagerRoleGuid ='{0}'", Request.Form["RoleList"]);
            }
            DataSet result = dg.GetPaged(id, pageSize, table, "*", where, "addTime desc  ", out total);
            PagedList<DataRow> arts = new PagedList<DataRow>(result.Tables[0].Select(), id, pageSize, total);
            if (Request.IsAjaxRequest())
            {
                return View("_Table", arts);
            }
            ViewData.Add("Business", BusinessLogic.GetSelect(this.manger.ManagerRoleGuid));
            ViewData.Add("RoleList", RoleLogic.GetSelectRole(where));
            return View(arts);
        }
        public ActionResult Edit(string id)
        {

            ViewBag.RoleList = RoleLogic.GetManageRole();
            TManager model = new DalGeneric<TManager>().GetBy(c => c.Guid == new Guid(id));
            return PartialView("Edit", model);
        }

        [HttpPost]
        public JsonResult Add()
        {
            ManagerLogic managerLogic = new ManagerLogic();
            TManager manager = new TManager();
            if (!string.IsNullOrEmpty(Request.Form["Guid"]))
            {
                manager.Guid = new Guid(Request.Form["Guid"]);
            }
            manager.OperatorGuid = this.OperatorGuid;
            manager.BusinessGuid = this.BusinessGuid;
            manager.ChainStoreGuid = this.ChainStoreGuid;
            manager.Name = Request.Form["Name"];
            manager.Phone = Request.Form["Phone"];
            manager.ManagerRoleGuid = new Guid(Request.Form["ManagerRoleGuid"]);
            manager.Email = Request.Form["Email"];

            manager.LoginName = Request.Form["LoginName"];
            if (!string.IsNullOrEmpty(Request.Form["LoginPwd"].ToString()))
            {
                manager.LoginPwd = Request.Form["LoginPwd"];
            }
            manager.Remark = Request.Form["Remark"];
            manager.Status = 0;
            bool flag = ManagerLogic.Add(manager);
            return Json(new { succ = flag });
        }
        public JsonResult Delete()
        {
            string guids = string.Empty; string msg = string.Empty;
            if (!string.IsNullOrEmpty(Request.Form["Guids"]))
            {
                guids = Request.Form["Guids"].ToString();
            }
            bool flag = ManagerLogic.Delete(guids, out msg);
            return Json(new { succ = flag, msg = msg });
        }
    }
}