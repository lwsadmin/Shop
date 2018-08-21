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
using Shop.Bll;

namespace AdminWeb.Areas.Statistics.Controllers
{
    public class MemberPointController : AdminBaseController
    {
        public ActionResult List(int id = 1)
        {

            //    string where = string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);
            string where = CommonLogic.GetWhereCondition();
            string table = string.Format(@"(select n.Operatetime,n.OperatorGuid,n.ChainStoreGuid,
n.Balance,n.BusinessGuid,n.BillNumber,n.Type,n.Way,n.UserAccount,c.Name,m.Mobile,m.NickName 
from TMemberPointNote n left join tmember m
on n.MemberGuid=m.Guid left join TChainStore c 
on n.ChainStoreGuid=c.Guid)tab");
            int PageSize = 10;
            if (!string.IsNullOrEmpty(Request.Form["PageSize"]))
            {
                PageSize = Convert.ToInt32(Request.Form["PageSize"]);
            }
            ViewData["PageSize"] = PageSize;

            if (!string.IsNullOrEmpty(Request["TimeFrom"]))
            {
                where += string.Format(" and Operatetime>='{0} 00:00:00'", Request["TimeFrom"]);
            }
            if (!string.IsNullOrEmpty(Request["TimeTo"]))
            {
                where += string.Format(" and Operatetime<='{0} 23:59:59'", Request["TimeTo"]);
            }
            int total;
            DalGeneric<object> Dal = new DalGeneric<object>();
            DataSet ds = Dal.GetPaged(id, PageSize, table, "*", where, "Operatetime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, PageSize, total);
            return View(pagelist);
        }
    }
}
