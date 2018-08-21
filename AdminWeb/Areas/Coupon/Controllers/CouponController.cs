using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Shop.Entity;
using Shop.Help;
using Shop.Dal;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
using Shop.Bll;
using Shop.Dal.Emnu;
using Shop.ViewModel;

namespace AdminWeb.Areas.Coupon.Controllers
{
    public class CouponController : AdminBaseController
    {
        // GET: Coupon/Coupon
        public ActionResult List(int pageIndex = 1)
        {
            string where = string.Format("1=1 and OperatorGuid='{0}'", this.OperatorGuid);
            string table = string.Format(@"(select * from tcoupon)Tab");

            int totol;
            DalGeneric<object> dal = new DalGeneric<object>();
            DataSet ds = dal.GetPaged(pageIndex, 12, table, "*", where, "CreateTime desc", out totol);
            PagedList<DataRow> pgaeList = new PagedList<DataRow>(ds.Tables[0].Select(), pageIndex, 12, totol);
            return View(pgaeList);
        }

        public ActionResult Add(Guid? id)
        {
            TCoupon coupon = new TCoupon();
            if (id != null)
            {
                Guid couponGuid = new Guid(id.ToString());
                coupon = CouponLogic.GetEntity(couponGuid);
            }
            else { coupon.OperatorGuid = this.OperatorGuid; }
            return View(coupon);
        }

        [ValidateInput(false)]
        public JsonResult Save(TCoupon coupon)
        {
            //coupon.IsOverlay = Convert.ToBoolean(Request.Form["IsOverlay"]);
            string msg = string.Empty;
            bool flag = CouponLogic.Add(coupon, out msg);
            return Json(new { succ = flag });
        }
    }
}