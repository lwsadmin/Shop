using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Help;
using Shop.Entity;
using Newtonsoft.Json;
using Shop.Bll;
using System.Web.Routing;

namespace Shop.AdminWeb.Controllers
{
    public class AdminBaseController : Controller
    {
        // GET: AdminBase

        protected AdminBaseController()
        {

        }
        protected Guid OperatorGuid
        {
            get { return manger.OperatorGuid; }
        }
        protected Guid BusinessGuid
        {
            get { return (Guid)manger.BusinessGuid; }
        }
        protected Guid ChainStoreGuid
        {
            get { return (Guid)manger.ChainStoreGuid; }
        }
        protected TManager manger
        {
            get;
            set;
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            string logMsg = string.Format(filterContext.Exception.Message + "----" + filterContext.Exception.StackTrace + "---" + DateTime.Now);

        //   filterContext.Exception.d
            LogLogic.WriteErrorLog(logMsg);
            base.OnException(filterContext);
        }
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            this.manger = ManagerLogic.GetLoginInfo();
            if (manger == null)
            {
                Safe.DelCookie("LoginInfo");
                filterContext.Result = new RedirectResult("/home/loginout");
                return;
            }
        }
    }
}