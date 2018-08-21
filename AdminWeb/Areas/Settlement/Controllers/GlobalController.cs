using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.AdminWeb.Controllers;
using Shop.Bll;
using Shop.Entity;
namespace AdminWeb.Areas.Settlement.Controllers
{
    public class GlobalController : AdminBaseController
    {
        // GET: Settlement/Global
        public ActionResult Index()
        {
            string GlobalType = ParameterLogic.GetParameter("GlobalType", this.OperatorGuid);
            string PointPrice = ParameterLogic.GetParameter("PointPrice", this.OperatorGuid);
            string PointGlobal = ParameterLogic.GetParameter("PointGlobal", this.OperatorGuid);
            string CouponGlobal = ParameterLogic.GetParameter("CouponGlobal", this.OperatorGuid);


            ViewData["GlobalType"] = string.IsNullOrEmpty(GlobalType) ? "0" : GlobalType;
            ViewData["PointPrice"] = string.IsNullOrEmpty(PointPrice) ? "1" : PointPrice;
            ViewData["PointGlobal"] = string.IsNullOrEmpty(GlobalType) ? "false" : GlobalType;
            ViewData["CouponGlobal"] = string.IsNullOrEmpty(PointPrice) ? "false" : PointPrice;


            return View();
        }

        public JsonResult Save()
        {
            string[] Par = Request.Form.AllKeys;
            Guid operatorGuid = this.OperatorGuid;
            for (int i = 0; i < Par.Length; i++)
            {
                ParameterLogic.SaveParameter(Par[i], Request[Par[i]], operatorGuid);
            }
            return Json(new { succ = true, msg = "保存成功!" });
        }
    }
}