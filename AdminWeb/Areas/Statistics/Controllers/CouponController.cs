using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.ViewModel;
using System.Data;
using Shop.Bll;
using Shop.Dal;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;

namespace AdminWeb.Areas.Statistics.Controllers
{
    public class CouponController : AdminBaseController
    {
        // GET: Statistics/Coupon
        public ActionResult List(int OperatorPageIndex = 1, int BusinessPageIndex = 1)
        {

            CommonDataRow model = new CommonDataRow();
            string where = string.Format(" 1=1 and OperatorGuid='{0}'", this.OperatorGuid);
            DalGeneric<Object> Dal = new DalGeneric<Object>();
            int pageSize = 12;
            string OpTable = string.Format(@"(select c.OperatorGuid,c.BeginDate,c.EndDate,c.IsLocked,c.IsUsed,
c.SendTime, m.Mobile,m.LoginName,p.Title
 from TMemberCoupon c left join TMember m on c.MemberGuid=m.Guid
 left join TCoupon p on c.CouponGuid=p.Guid
where c.IsUsed=0 and c.UsedTime is null
and c.Type=0)Tab");

            int OpTotal, BusTotal;
            DataSet BuyDs = Dal.GetPaged(OperatorPageIndex, pageSize, OpTable, "*", where, "SendTime desc", out OpTotal);
            model.RowList1 = new PagedList<DataRow>(BuyDs.Tables[0].Select(), BusinessPageIndex, pageSize, OpTotal);


            string BusTable = string.Format(@"(select c.OperatorGuid,c.BeginDate,c.EndDate,c.IsLocked,c.IsUsed,
c.SendTime, m.Mobile,m.LoginName,b.Title
 from TMemberCoupon c left join TMember m on c.MemberGuid=m.Guid
 left join TBusinessCoupon b on c.CouponGuid=b.Guid
where c.IsUsed=0 and c.UsedTime is null
and c.Type=1)Tab");
            DataSet SendDs = Dal.GetPaged(BusinessPageIndex, pageSize, BusTable, "*", where, "SendTime desc", out BusTotal);
            model.RowList2 = new PagedList<DataRow>(SendDs.Tables[0].Select(), BusinessPageIndex, pageSize, BusTotal);
            return View(model);
        }
    }
}