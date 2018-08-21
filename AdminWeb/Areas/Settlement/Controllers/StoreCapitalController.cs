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
    public class StoreCapitalController : AdminBaseController
    {
        // GET: Settlement/StoreCapital
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

            string table = @"(select c.Guid, c.Name,c.TotalPoint,c.TotalValue,
c.BusinessGuid,c.OperatorGuid,c.CreateTime,c.TotalSmsCount,
c.AvailablePoint,c.AvailableValue,c.SettlementMoney,b.BusinessName,c.AvailableSmsCount
 from TChainStore c left join TBusiness b  
 on c.BusinessGuid = b.Guid)tab";
            int total;
            DataSet ds = dalTChainStore.GetPaged(1, 10, table, "*", where, "Createtime desc", out total);
            PagedList<DataRow> pageList = new PagedList<DataRow>(ds.Tables[0].Select(), id, 10);
            return View(pageList);
        }


        [HttpPost]
        public string GetPointMoney()
        {
            decimal point = Convert.ToDecimal(Request.Form["point"]);
            decimal money = CommonLogic.GetMoneyByPoint(point, this.OperatorGuid);
            return money.ToString();
        }
        [HttpPost]
        public JsonResult BuyPoint()
        {
            string PointRechageValue = Request.Form["PointRechageValue"];
            string PointShouldInput = Request.Form["PointShouldInput"];
            string PointFactInput = Request.Form["PointFactInput"];
            string PointNote = Request.Form["PointNote"];
            string StoreGuid = Request.Form["PointGuid"];
            bool success = false; string msg = string.Empty;
            TManager manger = this.manger;
            TChainStorePointNote ChainStorePointNote = new TChainStorePointNote()
            {
                OperatorGuid = manger.OperatorGuid,
                Type = 0,//收入,
                ChainStoreGuid = new Guid(StoreGuid),
                Point = Convert.ToDecimal(PointRechageValue),
                MoneyFactPay = Convert.ToDecimal(PointFactInput),
                MoneyPayable = Convert.ToDecimal(PointShouldInput),
                UserAccount = manger.LoginName,
                Memo = PointNote
            };
            success = StoreCapitalSettlementLogic.StoreRechargePoint(ChainStorePointNote, manger, out msg);
            return Json(new
            {
                success = success,
                msg = msg
            });
        }

        public JsonResult BuyValue()
        {
            bool success = false; string msg = string.Empty;
            TManager manger = this.manger;
            TChainStoreValueNote ChainStoreValueNote = new TChainStoreValueNote()
            {

                Type = 0,//增加,
                ChainStoreGuid = new Guid(Request.Form["ValueGuid"]),
                Value = Convert.ToDecimal(Request.Form["ValueRechageValue"]),
                PayValue = Convert.ToDecimal(Request.Form["ValuePayValue"]),
                Memo = Request.Form["ValueNote"]
            };
            success = StoreCapitalSettlementLogic.StoreRechargeValue(ChainStoreValueNote, manger, out msg);
            return Json(new { succ = success, msg = msg });
        }


        public JsonResult BuySms()
        {
            bool success = false; string msg = string.Empty;
            TManager manger = this.manger;
            TChainStoreBuySmsNote BuySmsNote = new TChainStoreBuySmsNote()
            {

                OperatorGuid = this.OperatorGuid,
                ChainStoreGuid = new Guid(Request.Form["SmsGuid"]),
                ShouldPay = Convert.ToDecimal(Request.Form["SmsShouldPay"]),
                FactPay = Convert.ToDecimal(Request.Form["SmsFactPay"]),
                Memo = Request.Form["SmsNote"],
                Count = Convert.ToInt32(Request.Form["SmsRechageValue"]),
                UserAccount="admin"
            };
            success = StoreCapitalSettlementLogic.StoreBuySms(BuySmsNote, manger, out msg);
            return Json(new { succ = success, msg = msg });
        }

        public void ViewDataAdd()
        {
            //IEnumerable<SelectListItem> list = BusinessLogic.GetSelect();
            ViewData.Add("Business", BusinessLogic.GetSelect(this.manger.ManagerRoleGuid));
        }
    }
}