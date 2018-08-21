using System.Web.Mvc;

namespace AdminWeb.Areas.CityLife
{
    public class CityLifeAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "CityLife";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "CityLife_default",
                "CityLife/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}