using Shop.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Shop.ViewModel;
using System.Data;
using Shop.Entity;
using Shop.Dal;
using Webdiyer.WebControls.Mvc;

namespace AdminWeb.Areas.Statistics.Controllers
{
    public class ChainStoreSmsNoteController : Controller
    {
        // GET: Statistics/ChainStoreSmsNote
        public ActionResult List(int buyPageIndex = 1, int sendPageIndex = 1)
        {
            CommonDataRow model = new CommonDataRow();
            string where = CommonLogic.GetWhereCondition();
            DalGeneric<Object> Dal = new DalGeneric<Object>();
            int BuyTotal, SendTotal;

            int buyPageSize = 10;
            if (!string.IsNullOrEmpty(Request.Form["buyPageSize"]))
            {
                buyPageSize = Convert.ToInt32(Request.Form["buyPageSize"]);
            }
            ViewData["buyPageSize"] = buyPageSize;


            int sendPageSize = 10;
            if (!string.IsNullOrEmpty(Request.Form["sendPageSize"]))
            {
                sendPageSize = Convert.ToInt32(Request.Form["sendPageSize"]);
            }
            ViewData["sendPageSize"] = sendPageSize;
            string BuyWhere = "", SendWhere = "";
            if (!string.IsNullOrEmpty(Request["StoreName"]))
            {
                BuyWhere += string.Format(" and Name like '%{0}%'", Request["StoreName"]);
            }
            if (!string.IsNullOrEmpty(Request["BuyTimeFrom"]))
            {
                BuyWhere += string.Format(" and OperateTime >= '{0} 00:00:00'", Request["SendTimeFrom"]);
            }
            if (!string.IsNullOrEmpty(Request["BuyTimeTo"]))
            {
                BuyWhere += string.Format(" and OperateTime <= '{0} 23:59:59'", Request["SendTimeTo"]);
            }
            string buyTable = string.Format(@"(select n.OperatorGuid,n.ChainStoreGuid,n.Count,
n.ShouldPay,n.FactPay,n.UserAccount,n.OperateTime,c.AvailableSmsCount,
c.Name,n.Memo from TChainStoreBuySmsNote n left join TChainStore c
on n.ChainStoreGuid=c.Guid)Tab");
            DataSet BuyDs = Dal.GetPaged(buyPageIndex, sendPageSize, buyTable, "*", where, "OperateTime desc", out BuyTotal);
            model.RowList1 = new PagedList<DataRow>(BuyDs.Tables[0].Select(), buyPageIndex, sendPageSize, BuyTotal);


            if (!string.IsNullOrEmpty(Request["Content"]))
            {
                SendWhere += string.Format(" and SmsContent like '%{0}%'", Request["Content"]);
            }
            if (!string.IsNullOrEmpty(Request["SendTimeFrom"]))
            {
                SendWhere += string.Format(" and SendTime >= '{0} 00:00:00'", Request["SendTimeFrom"]);
            }
            if (!string.IsNullOrEmpty(Request["SendTimeTo"]))
            {
                SendWhere += string.Format(" and SendTime <= '{0} 23:59:59'", Request["SendTimeTo"]);
            }
            string sendTable = string.Format(@"(select n.OperatorGuid,n.BusinessGuid,n.ChainStoreGuid,
n.Mobiles, n.SmsContent, n.SendTime, n.Status, n.Memo, c.Name,n.SmsCount
 from TChainStoreSmsSendNote n left join TChainStore c on n.ChainStoreGuid = c.Guid)Tab");
            DataSet SendDs = Dal.GetPaged(sendPageIndex, sendPageSize, sendTable, "*", where + SendWhere, " SendTime desc ", out SendTotal);
            model.RowList2 = new PagedList<DataRow>(SendDs.Tables[0].Select(), sendPageIndex, sendPageSize, SendTotal);

            return View(model);
        }
        public string GetSearchWhere()
        {
            string where = string.Empty;

            return where;
        }

        // GET: Statistics/ChainStoreSmsNote/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Statistics/ChainStoreSmsNote/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Statistics/ChainStoreSmsNote/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Statistics/ChainStoreSmsNote/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Statistics/ChainStoreSmsNote/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Statistics/ChainStoreSmsNote/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Statistics/ChainStoreSmsNote/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
