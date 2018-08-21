using System.Web.Mvc;

namespace AdminWeb.Areas.Coupon
{
    public class CouponAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Coupon";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Coupon_default",
                "Coupon/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}