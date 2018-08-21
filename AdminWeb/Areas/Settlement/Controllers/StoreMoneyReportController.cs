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
namespace AdminWeb.Areas.Settlement.Controllers
{
    public class StoreMoneyReportController : AdminBaseController
    {
        // GET: Settlement/StoreMoneyReport
        public ActionResult List(int id = 1)
        {
            string where = CommonLogic.GetWhereCondition();
            if (!string.IsNullOrEmpty(Request["Business"]))
            {
                where += string.Format(" and BusinessGuid='{0}'", Request["Business"]);
            }
            if (!string.IsNullOrEmpty(Request["storeName"]))
            {
                where += string.Format(" and Name like'%{0}%'", Request["storeName"]);
            }
            if (!string.IsNullOrEmpty(Request["SettType"]))
            {
                if (Request["SettType"] == "0")
                {
                    where += string.Format(" and SettlementMoney>'0'");
                }
                else { where += string.Format(" and SettlementMoney<'0'"); }
            }
            ViewData.Add("Business", BusinessLogic.GetSelect(this.manger.ManagerRoleGuid));
            DalGeneric<TChainStore> dalTChainStore = new DalGeneric<TChainStore>();

            string table = @"(select n.OperatorGuid,n.AllValue,n.BillNumber,
n.BusinessGuid,n.ChainStoreGuid,n.OperatorTime,c.Name,c.SettlementMoney,b.BusinessName,
n.Type,n.Way,n.Memo from TChainStoreCapitalNote n
left join TChainStore c on n.ChainStoreGuid=c.Guid
left join TBusiness b on n.BusinessGuid=b.Guid)tab";
            int total;
            DataSet ds = dalTChainStore.GetPaged(1, 10, table, "*", where, "OperatorTime desc", out total);
            PagedList<DataRow> pageList = new PagedList<DataRow>(ds.Tables[0].Select(), id, 10);
            return View(pageList);
        }
    }
}