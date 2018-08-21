using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Areas.CityLife.Controllers
{
    public class LifeController : Controller
    {
        // GET: CityLife/Life
        public ActionResult MessageList(int id=1)
        {
            return View();
        }
    }
}