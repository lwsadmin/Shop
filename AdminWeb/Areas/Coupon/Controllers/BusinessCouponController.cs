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
    public class BusinessCouponController : AdminBaseController
    {
        // GET: Coupon/BusinessCoupon
        public ActionResult List(int pageIndex = 1)
        {
            string where = CommonLogic.GetWhereCondition();

            if (!string.IsNullOrEmpty(Request.Form["title"]))
            {
                where += string.Format(" and title like'%{0}%'", Request.Form["title"]);
            }
            if (!string.IsNullOrEmpty(Request.Form["Business"]))
            {
                where += string.Format(" and BusinessGuid='{0}'", Request.Form["Business"]);
            }
            if (!string.IsNullOrEmpty(Request["Status"]))
            {
                where += string.Format(" and status='{0}'", Request.Form["Status"]);
            }
            string table = string.Format(@"(select c.Guid,c.TotalCount, c.OperatorGuid, c.Title,b.BusinessName,b.Guid as BusinessGuid, c.Image,c.Value,c.UseValueLimit,c.MemberReceiveCount
,c.status,c.createtime from TBusinessCoupon c
left join TBusiness b on c.BusinessGuid=b.Guid)Tab");
            ViewDataAdd();
            int total;
            DalGeneric<object> dal = new DalGeneric<object>();
            DataSet ds = dal.GetPaged(pageIndex, 12, table, "*", where, "CreateTime desc", out total);
            PagedList<DataRow> pgaeList = new PagedList<DataRow>(ds.Tables[0].Select(), pageIndex, 12, total);
            return View(pgaeList);
        }

        public ActionResult Add(Guid? id)
        {
            TBusinessCoupon businessCoupon = new TBusinessCoupon();
            ViewDataAdd();
            if (id == null)
            {
                businessCoupon.OperatorGuid = this.OperatorGuid;
            }
            else
            {
                businessCoupon = BusinessCouponLogic.GetEntity(new Guid(id.ToString()));
            }
            return View(businessCoupon);

        }

        [ValidateInput(false)]
        public JsonResult Save(TBusinessCoupon model)
        {
            string msg = string.Empty;
            bool success = BusinessCouponLogic.Add(model, out msg);
            return Json(new { succ = success, msg = msg });
        }


        public JsonResult Delete(string guids)
        {
            string msg = string.Empty;
            bool succ = BusinessCouponLogic.Delete(guids, out msg);

            return Json(new { succ = succ, msg = msg });
        }

        public JsonResult SetStatus(string guid, int status)
        {
            string msg = string.Empty;
            bool success = BusinessCouponLogic.SetStatus(new Guid(guid), status, out msg);
            return Json(new { succ = success, msg = msg });
        }

        public void ViewDataAdd()
        {
            ViewData.Add("Business", BusinessLogic.GetSelect(this.manger.ManagerRoleGuid));
        }
    }
}