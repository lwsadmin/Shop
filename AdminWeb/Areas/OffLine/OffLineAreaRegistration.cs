using System.Web.Mvc;

namespace AdminWeb.Areas.OffLine
{
    public class OffLineAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "OffLine";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "OffLine_default",
                "OffLine/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}