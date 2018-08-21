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
    public class StoreSetController : AdminBaseController
    {
        // GET: Settlement/StoreSet
        public ActionResult List(int pageIndex = 1)
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

            string table = string.Format(@"(select c.Guid,c.OperatorGuid,c.Name,c.AvailablePoint,c.CreateTime,
c.AvailableValue,c.BusinessGuid,b.BusinessName from TChainStore c
left join TBusiness b on c.BusinessGuid=b.Guid)tab");
            int total;
            DataSet ds = dalTChainStore.GetPaged(1, 10, table, "*", where, "CreateTime desc", out total);
            PagedList<DataRow> pageList = new PagedList<DataRow>(ds.Tables[0].Select(), pageIndex, 10);
            return View(pageList);
        }

        public ActionResult Set(string id)
        {
            ViewData["id"] = id;
            DalGeneric<TChainStore> dalTChainStore = new DalGeneric<TChainStore>();
            string where = string.Format("1=1 and OperatorGuid='{0}'", this.OperatorGuid);
            string table = string.Format(@"(select s.*,c.Title from TChainStoreSettleSet s
left join TMemberCategory c on 
s.MemberCategoryGuid=c.Guid
where s.ChainStoreGuid is null and
s.MemberCategoryGuid not in 
(select MemberCategoryGuid from TChainStoreSettleSet where ChainStoreGuid='{0}')
 union 
 select  s.*,c.Title from TChainStoreSettleSet s
left join TMemberCategory c on 
s.MemberCategoryGuid=c.Guid
where s.ChainStoreGuid='{0}')tab", id);
            int total;
            DataSet ds = dalTChainStore.GetPaged(1, int.MaxValue, table, "*", where, " Title desc", out total);
            PagedList<DataRow> pageList = new PagedList<DataRow>(ds.Tables[0].Select(), 1, int.MaxValue);

            return PartialView("Set", pageList);
        }


        public JsonResult SetSave()
        {

            TChainStoreSettleSet StoreSettleSet = new TChainStoreSettleSet();

            StoreSettleSet.MemberCategoryGuid = new Guid(Request.Form["MemberCategoryGuid"]);
            StoreSettleSet.ChainStoreGuid = new Guid(Request.Form["ChainStoreGuid"]);
            StoreSettleSet.OperatorGuid = this.OperatorGuid;
            StoreSettleSet.Memo = "";

            StoreSettleSet.OffLineConsumeDiscount = Convert.ToDecimal(Request.Form["OffLineConsumeDiscount"]);
            StoreSettleSet.OffLineConsumeRebate = Convert.ToDecimal(Request.Form["OffLineConsumeRebate"]);
            StoreSettleSet.OnLineConsumeRebate = Convert.ToDecimal(Request.Form["OnLineConsumeRebate"]);
            StoreSettleSet.PointPaidRebate = Convert.ToDecimal(Request.Form["PointPaidRebate"]);
            StoreSettleSet.BusinessProfitProportion = Convert.ToDecimal(Request.Form["BusinessProfitProportion"]);
            StoreSettleSet.MemberProfitProportion = Convert.ToDecimal(Request.Form["MemberProfitProportion"]);


            bool flag = StoreCapitalSetLogic.SaveSet(StoreSettleSet);


            return Json(new {succ= flag });
        }
    }
}