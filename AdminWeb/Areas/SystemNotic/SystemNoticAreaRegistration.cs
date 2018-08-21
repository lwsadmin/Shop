using System.Web.Mvc;

namespace AdminWeb.Areas.SystemNotic
{
    public class SystemNoticAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SystemNotic";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SystemNotic_default",
                "SystemNotic/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}