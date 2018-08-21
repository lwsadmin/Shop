using System.Web.Mvc;

namespace Shop.AdminWeb.Areas.ArticleInfo
{
    public class ArticleInfoAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "ArticleInfo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "ArticleInfo_default",
                "ArticleInfo/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}