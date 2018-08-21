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
namespace AdminWeb.Areas.OffLine.Controllers
{
    public class RechargeController : AdminBaseController
    {
        // GET: OffLine/Recharge
        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult MemberRecharge()
        {

            return Json(new { success = true, message = 1 });
        }
    }
}  