using Shop.Bll;
using Shop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdminWeb.App_Start
{
    public class CMSFilter : ActionFilterAttribute
    {


        public string MenuName { get; set; }
        public string ActionName { get; set; } = "";
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            if (!string.IsNullOrEmpty(MenuName))
            {
                TManager model = ManagerLogic.GetLoginInfo();
                if (model == null)
                {
                    filterContext.Result = new RedirectResult("/home/loginout");
                    return;
                }

                if (RoleLogic.CheckPower(model.ManagerRoleGuid, MenuName) == false)//没有权限
                {
                    //  Safe.DelCookie("LoginInfo");
                    filterContext.Result = new RedirectResult("/home/loginout");
                    return;
                }
            }

        }
    }
}