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

namespace AdminWeb.Areas.SystemNotic.Controllers
{
    public class SmsSendController : AdminBaseController
    {
        // GET: SystemNotic/SmsSend
        public ActionResult Index()
        {
            return View();
        }

    }
}