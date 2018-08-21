using System.Web.Mvc;

namespace AdminWeb.Areas.Set
{
    public class SetAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Set";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Set_default",
                "Set/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}