using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Shop.Entity;
using Shop.Bll;
using Shop.Dal;
using System.Data;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
using System.Text;
using Shop.Help;
namespace AdminWeb.Areas.Coupon.Controllers
{
    public class CouponPackageController : AdminBaseController
    {
        // GET: Coupon/CouponPackage
        public ActionResult List(int pageIndex = 1)
        {
            int pageSize = 10;
            DalGeneric<TCouponPackage> dal = new DalGeneric<TCouponPackage>();

            string where = string.Format("1=1 and OperatorGuid='{0}'", this.OperatorGuid);

            if (!string.IsNullOrEmpty(Request["title"]))
            {
                where += string.Format(" and title like '%{0}%'", Request["title"].Trim());
            }
            string sqlTable = string.Format(@"(select Guid,Title,OperatorGuid,
Price,Point,Memo,CreateTime from TCouponPackage) Tab");
            int total;
            DataSet ds = dal.GetPaged(pageIndex, pageSize, sqlTable, "*", where, " CreateTime desc", out total);

            DataRow[] DataRowArray = ds.Tables[0].Select();
            // IQueryable<DataRow> row= DataRowArray;
            PagedList<DataRow> pageListDataRow = new PagedList<DataRow>(DataRowArray, pageIndex, pageSize);

            return View(pageListDataRow);
        }

        public ActionResult Add(Guid? id)
        {
            TCouponPackage packAge = new TCouponPackage();
            ViewData["SelectListItem"] = CouponPackageLogic.GetSelectOption();
            if (id == null)
            {
                packAge.OperatorGuid = this.OperatorGuid;
            }
            else
            {
                packAge = CouponPackageLogic.GetEntity(new Guid(id.ToString()));

                List<TCouponPackageItem> list = CouponPackageLogic.GetItemList(packAge.Guid);
                string str = string.Empty;
                foreach (TCouponPackageItem item in list)
                {
                    str += (string.Format("{0},", item.ObjectGuid.ToString()));
                }
                ViewData["list"] = str;
            }
            return View(packAge);
        }
        public ActionResult Save(TCouponPackage model)
        {
            string msg = string.Empty;
            bool succ = true;
            string itemGuid = Request["searchablems2side__dx"];
            succ = CouponPackageLogic.Add(model, itemGuid, out msg);
            if (succ)
            {
                return Redirect("/coupon/couponpackage/list");
            }
            return Json(new { succ = succ, msg = msg });
        }

        public JsonResult Delete(string id)
        {
            string msg = "删除失败!";
            if (!string.IsNullOrEmpty(id))
            {
                bool succ = CouponPackageLogic.Delete(new Guid(id), out msg);
                return Json(new { succ = true, msg = msg });
            }
            else { return Json(new { succ = false, msg = msg }); }
        }
    }
}