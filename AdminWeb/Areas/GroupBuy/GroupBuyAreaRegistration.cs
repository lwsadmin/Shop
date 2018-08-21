using System.Web.Mvc;

namespace AdminWeb.Areas.GroupBuy
{
    public class GroupBuyAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GroupBuy";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GroupBuy_default",
                "GroupBuy/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}