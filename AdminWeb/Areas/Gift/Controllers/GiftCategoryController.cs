using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Shop.Bll;
using Shop.Dal;
using Shop.Entity;
using Shop.Help;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
using Shop.Dal.Emnu;

namespace AdminWeb.Areas.Gift.Controllers
{
    public class GiftCategoryController : AdminBaseController
    {
        // GET: Gift/GiftCategory
        public ActionResult List(int id = 1)
        {
          
            string where = CommonLogic.GetWhereCondition();
            if (!string.IsNullOrEmpty(Request["title"]))
            {
                where += string.Format(" and title like'%{0}%'", Request["title"]);
            }
            int pageSize = 10;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            DalGeneric<TGiftCategory> Dal = new DalGeneric<TGiftCategory>();
            // List<usp_SelecTGiftCategoryBusinessGuid_Result> list = new List<usp_SelecTGiftCategoryBusinessGuid_Result>();
            ViewData["CateGory"] = GiftCategoryLogic.GetDropdownList(this.BusinessGuid);
            string table = string.Format(@"(select c.* from TGiftCategory c) Tab");
            int total;
            DataSet ds = Dal.GetPaged(id, pageSize, table, "*", where, "sort asc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            return View(pagelist);
        }


        [HttpPost]
        public JsonResult Add(TGiftCategory model)
        {
            TGiftCategory giftCatgory = new TGiftCategory();
            if (!string.IsNullOrEmpty(Request.Form["Guid"].ToString()))
            {
                giftCatgory.Guid = new Guid(Request.Form["Guid"]);
            }
            else
            {
                giftCatgory.OperatorGuid = this.OperatorGuid;
                giftCatgory.BusinessGuid = this.BusinessGuid;
                giftCatgory.ChainStoreGuid = this.ChainStoreGuid;
            }
         //   giftCatgory.Type = (int)CateGory.Gift;
            giftCatgory.Title = Request.Form["Title"];
            giftCatgory.Sort = Convert.ToInt32(Request.Form["Sort"]);
            giftCatgory.Remark = Request.Form["Remark"];


            string msg = string.Empty;

            return Json(new { succ = GiftCategoryLogic.Add(giftCatgory, out msg), msg = msg });
        }
    }
}