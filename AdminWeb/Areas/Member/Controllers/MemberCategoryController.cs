using Shop.Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Shop.Entity;
using System.Web.Mvc;
using System.Data.Sql;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
using Shop.Bll;
namespace AdminWeb.Areas.Member.Controllers
{
    public class MemberCategoryController : AdminBaseController
    {
        // GET: Member/MemberCategory
        public ActionResult List(int id = 1)
        {
            string where = string.Format("1=1 and operatorGuid='{0}' ", this.OperatorGuid);
            if (!string.IsNullOrEmpty(Request["title"]))
            {
                where += string.Format(" and title like'%{0}%'", Request["title"]);
            }
            DalGeneric<TMemberCategory> Dal = new DalGeneric<TMemberCategory>();
            int total;
            string table = string.Format(@"(select c.guid,c.operatorGuid,c.Title,c.SoldMoney,c.DefaultPoint,
c.DefaultValue,c.RecommendPoint,c.UpgradeNeedPoint
 from Tmembercategory c) Tab");
            int pageSize = 15;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            DataSet ds = Dal.GetPaged(id, pageSize, table, "*", where, "defaultPoint asc,UpgradeNeedPoint asc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            return View(pagelist);
        }

        public ActionResult Add(string id)
        {
            DalGeneric<TMemberCategory> dal = new DalGeneric<TMemberCategory>();
            TMemberCategory memberCategory = new TMemberCategory();
            if (!string.IsNullOrEmpty(id))
                memberCategory = dal.GetBy(c => c.Guid == new Guid(id));
            else
            {
                memberCategory.OperatorGuid = this.OperatorGuid;
            }
            return View(memberCategory);
        }

        [ValidateInput(false)]
        [HttpPost]
        public JsonResult AddSave(TMemberCategory model)
        {
            string msg = string.Empty;
            bool flag = MemberCategoryLogic.Add(model, out msg);
            return Json(new { succ = flag, msg = msg });
        }
    }
}