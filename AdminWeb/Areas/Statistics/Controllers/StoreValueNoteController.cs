using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Shop.Entity;
using Shop.Dal;
using Shop.Bll;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
namespace AdminWeb.Areas.Statistics.Controllers
{
    public class StoreValueNoteController : AdminBaseController
    {
        // GET: Statistics/StoreValueNote
        public ActionResult List(int id = 1)
        {
            string where = CommonLogic.GetWhereCondition();
            ViewDataAdd();
            string table = string.Format(@"(	
select n.OperateTime,n.OperatorGuid ,n.BillNumber,n.UserAccount,
c.BusinessGuid,b.BusinessName,
n.Type,n.Value,n.DoneValue,n.PayValue,n.Memo,c.Name from TChainStoreValueNote n
left join TChainStore c on n.ChainStoreGuid=c.Guid
left join TBusiness b on c.BusinessGuid=b.Guid)tab");
            if (!string.IsNullOrEmpty(Request["Name"]))
            {
                where += string.Format(" and Name like'%{0}%'", Request["Name"]);
            }
            if (!string.IsNullOrEmpty(Request["Business"]))
            {
                where += string.Format(" and BusinessGuid='{0}'", Request["Business"]);
            }
            if (!string.IsNullOrEmpty(Request["TimeFrom"]))
            {
                where += string.Format(" and OperateTime>='{0} 00:00:00'", Request["TimeFrom"]);
            }
            if (!string.IsNullOrEmpty(Request["TimeTo"]))
            {
                where += string.Format(" and OperateTime<='{0} 23:59:59'", Request["TimeTo"]);
            }
            int PageSize = 10;
            if (!string.IsNullOrEmpty(Request.Form["PageSize"]))
            {
                PageSize = Convert.ToInt32(Request.Form["PageSize"]);
            }
            ViewData["PageSize"] = PageSize;
            int total;
            DalGeneric<object> Dal = new DalGeneric<object>();
            DataSet ds = Dal.GetPaged(id, PageSize, table, "*", where, "OperateTime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, PageSize, total);
            return View(pagelist);
        }

        public void ViewDataAdd()
        {
            //IEnumerable<SelectListItem> list = BusinessLogic.GetSelect();
            ViewData.Add("Business", BusinessLogic.GetSelect(this.manger.ManagerRoleGuid));
        }
    }
}