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

namespace AdminWeb.Areas.Statistics.Controllers
{
    public class GiftExchangeNoteController : AdminBaseController
    {
        public ActionResult List(int id = 1)
        {

            string where = string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);
            string table = string.Format(@"(select n.OperatorGuid,n.BillNumber,
n.OpertorTime,n.Memo,n.UserAccount,c.Name,
m.Mobile,m.LoginName,m.NickName from TGiftExchangeNote n left join TMember m on n.MemberGuid=m.Guid
left join TChainStore c on n.ChainStoreGuid=c.Guid
)tab");
            int PageSize = 10;
            if (!string.IsNullOrEmpty(Request.Form["PageSize"]))
            {
                PageSize = Convert.ToInt32(Request.Form["PageSize"]);
            }
            ViewData["PageSize"] = PageSize;
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
            int total;
            DalGeneric<object> Dal = new DalGeneric<object>();
            DataSet ds = Dal.GetPaged(id, PageSize, table, "*", where, "OpertorTime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, PageSize, total);
            return View(pagelist);
        }
    }
}
