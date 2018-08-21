using System.Web.Mvc;

namespace AdminWeb.Areas.GoodsManage
{
    public class GoodsManageAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "GoodsManage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "GoodsManage_default",
                "GoodsManage/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}