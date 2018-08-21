using Shop.Bll;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.Areas.SystemNotic.Controllers
{
    public class EmailSendController : Controller
    {
        // GET: SystemNotic/EmailSend
        public ActionResult Index()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult SendEmail()
        {
            string Content = Request.Form["content"];
            string Reciver = Request.Form["reicver"];
            string msg = string.Empty;
            SmsSendLogic.Send(Content, Reciver, out msg);

            ViewData["msg"] = MvcHtmlString.Create("layer.msg('邮件发送成功!', { icon: 1 });");
            return View("Index");

        }
    }
}