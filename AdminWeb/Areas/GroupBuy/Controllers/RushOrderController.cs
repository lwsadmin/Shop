using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Help;
using Shop.Dal;
using System.Data;
using System.Data.Sql;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
using Shop.Bll;
using Shop.Dal.Emnu;
using Shop.ViewModel;

namespace AdminWeb.Areas.GroupBuy.Controllers
{
    public class RushOrderController : AdminBaseController
    {
        // GET: GroupBuy/RushOrder
        public ActionResult List(int pageIndex = 1)
        {
            ViewDataAdd();

            string where = CommonLogic.GetWhereCondition();
            if (!string.IsNullOrEmpty(Request.Form["OrderNumber"]))
            {
                where += string.Format(" and OrderNumber like '%{0}%'", Request.Form["OrderNumber"]);
            }
            if (!string.IsNullOrEmpty(Request.Form["Business"]))
            {
                where += string.Format(" and BusinessGuid = '{0}'", Request.Form["Business"]);
            }
            if (!string.IsNullOrEmpty(Request["TimeFrom"]))
            {
                where += string.Format(" and CreateTime>='{0} 00:00:00'", Request["TimeFrom"]);
            }
            if (!string.IsNullOrEmpty(Request["TimeTo"]))
            {
                where += string.Format(" and CreateTime<='{0} 23:59:59'", Request["TimeTo"]);
            }
            int pageSize = 15;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            string table = string.Format(@"(select o.Guid, o.CreateTime,o.OperatorGuid,o.BusinessGuid,b.BusinessName, o.Count,o.OrderNumber,o.Status,r.Title,o.TotalMoney,o.TotalPay,o.AliPaid
,o.WeiXinPaid,o.ValuePaid,o.PointPaid,m.Name,m.Mobile from TRushBuyOrder o left join TRushBuy  r 
on o.RushBuyGuid=r.Guid left join TMember m on o.MemberGuid=m.Guid left join TBusiness b on o.BusinessGuid
=b.Guid
)Tab");
            ViewData["NowPageSize"] = pageSize;
            int total;
            DalGeneric<object> dal = new DalGeneric<object>();
            DataSet ds = dal.GetPaged(pageIndex, pageSize, table, "*", where, " CreateTime desc", out total);
            PagedList<DataRow> pageList = new PagedList<DataRow>(ds.Tables[0].Select(), pageIndex, pageSize, total);
            return View(pageList);
        }

        public void ViewDataAdd()
        {
            //IEnumerable<SelectListItem> list = BusinessLogic.GetSelect();
            ViewData.Add("Business", BusinessLogic.GetSelect(this.manger.ManagerRoleGuid));
        }
        public JsonResult GetOrderItem(Guid orderGuid)
        {

            bool flag = true;
            string msg = string.Empty;
            return Json(new { succ = flag });
        }
    }
}