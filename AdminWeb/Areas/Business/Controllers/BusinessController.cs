using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Mvc;

using Shop.Help;
using Shop.Entity;
using Shop.Dal;
using Shop.Bll;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;

namespace Shop.AdminWeb.Areas.Business.Controllers
{
    public class BusinessController : AdminBaseController
    {
        [ValidateInput(false)]
        public ActionResult List(int id = 1)
        {
            string where = CommonLogic.GetWhereCondition();

            if (!string.IsNullOrEmpty(Request.QueryString["BusinessName"]))
            {
                where += string.Format(" and BusinessName like '%{0}%'", Request.QueryString["BusinessName"]);
            }
            if (!string.IsNullOrEmpty(Request.QueryString["Industry"]))
            {
                where += string.Format(" and IndustryGuid='{0}'", Request.QueryString["Industry"]);
            }
            DalGeneric<TBusiness> dalBusiness = new DalGeneric<TBusiness>();
            string table = @"(select b1.Guid,b1.OperatorGuid,b1.Sort,b1.StoreCount,b1.Contact
,b1.RegisterTime,b1.DueTime,b1.BusinessName ,b1.Tel,b1.industryGuid,
i.Title from tbusiness b1 
left join TIndustry i on b1.IndustryGuid=i.Guid)tab";
            int pageSize = 15;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            int total;
            DataSet ds = dalBusiness.GetPaged(id, pageSize, table, "*", where, "sort desc, registertime desc", out total);
            PagedList<DataRow> pageList = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            if (Request.IsAjaxRequest())
            {
                return View("_Table", pageList);
            }
            ViewData["Industry"] = IndustryLogic.GetDropDownList(this.OperatorGuid);
            return View(pageList);
        }
        public ActionResult Add(Guid? id)
        {
            TBusiness business = new TBusiness();
            business.OperatorGuid = this.OperatorGuid;
            business.ProvinceId = 1; business.CityID = 1; business.DistrictId = 1;

            ViewData["Industry"] = IndustryLogic.GetDropDownList(this.OperatorGuid);
            DalGeneric<TBusiness> dal = new DalGeneric<TBusiness>();
            if (id != null)
                business = dal.GetBy(c => c.Guid == id);
            return View(business);
        }
        [HttpPost]
        public JsonResult Add(TBusiness model)
        {
            TBusiness business = new TBusiness();

            return Json(new
            {
                succ = BusinessLogic.Add(model)
            });

        }
        [HttpPost]
        public JsonResult GetAddressSlect(int id, int type)
        {
            string option = string.Empty;
            switch (type)
            {
                case 0:
                    option = AddressSelect.GetCity(id.ToString());
                    break;
                case 1:
                    option = AddressSelect.GetDis(id.ToString());
                    break;

            }
            return Json(new { succ = true, str = option });
        }
    }
}