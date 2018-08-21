using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Bll;
using Shop.Dal;
using Shop.Entity;
using Shop.Help;
using System.Data;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
using Shop.Dal.Emnu;
namespace AdminWeb.Areas.Gift.Controllers
{
    public class GiftController : AdminBaseController
    {
        // GET: Gift/Gift
        public ActionResult List(int id = 1)
        {
          
            if (!Request.IsAjaxRequest())
            {
                ViewDataAdd();
            }
            //类别是运营商建，礼品归属到商户。
            string where = CommonLogic.GetWhereCondition();               // string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);
            DalGeneric<TGift> Dal = new DalGeneric<TGift>();
            if (!string.IsNullOrEmpty(Request.Form["title"]))
            {
                where += string.Format(" and Name like '%{0}%'", Request.Form["title"].ToString());
            }
            if (!string.IsNullOrEmpty(Request.Form["CateGory"]))
            {
                where += string.Format(" and  GiftCategoryGuid= '{0}'", Request.Form["CateGory"].ToString());
            }
            if (!string.IsNullOrEmpty(Request.Form["Business"]))
            {
                where += string.Format(" and  BusinessGuid= '{0}'", Request.Form["Business"].ToString());
            }
            int pageSize = 10;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;

            string table = @"(select g.Name,g.Image,g.Point,g.Status,
g.SingleLimit,g.StockCount,b.Title,g.Guid,g.OperatorGuid,g.sort,g.GiftCategoryGuid,
g.createTime,g.ExchangeCount,c.BusinessName,g.BusinessGuid
from TGift g join TGiftCategory b on g.GiftCategoryGuid=b.guid
left join TBusiness c on g.BusinessGuid=c.Guid) Tab";
            int total;
            DataSet ds = Dal.GetPaged(id, pageSize, table, "*", where, "sort asc,createTime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            return View(pagelist);
        }


        public ActionResult Add(string id)
        {
            ViewDataAdd();
            DalGeneric<TGift> dal = new DalGeneric<TGift>();
            TGift gift = new TGift();
            if (!string.IsNullOrEmpty(id))
                gift = dal.GetBy(c => c.Guid == new Guid(id));
            else
            {
                gift.ExchangeType = "TakeSelf";
                gift.OperatorGuid = this.OperatorGuid;
                gift.BusinessGuid = this.BusinessGuid;
            }
            return View(gift);
        }

        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add(TGift model)
        {
            string msg = string.Empty;
            model.ExchangeType = Request["ExchangeType"];
            bool succ = GiftLogic.Add(model, out msg);
            return Json(new { succ = succ, msg = msg });

        }
        public void ViewDataAdd()
        {

            string where = string.Format(" and Type={0}", CateGory.Gift);
            ViewData["CateGory"] = GiftCategoryLogic.GetDropdownList(this.BusinessGuid);
            ViewData.Add("Business", BusinessLogic.GetSelect(this.manger.ManagerRoleGuid));
        }
    }
}