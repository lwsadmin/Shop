using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using System.Data;
using Shop.Dal.Emnu;
using Shop.Bll;
using Shop.Dal;
using Shop.Entity;
using Shop.AdminWeb.Controllers;

namespace AdminWeb.Areas.GoodsManage.Controllers
{
    public class BrandController : AdminBaseController
    {
        // GET: GoodsManage/Brand
        public ActionResult List(int id = 1)
        {
            DalGeneric<TBrand> Dal = new DalGeneric<TBrand>();
            string where = string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);
            if (!string.IsNullOrEmpty(Request.Form["title"]))
            {
                where += string.Format(" and title like '%{0}%'", Request.Form["title"].ToString());
            }
            if (!string.IsNullOrEmpty(Request.Form["GoodsCategory"]))
            {
                where += string.Format(@" and CHARINDEX(CONVERT(nvarchar(500), Guid),(select Brand from TGoodsCategory where
  Guid = '{0}'))> 0", Request.Form["GoodsCategory"]);
            }
            int total; int pageSize = 15;
            string table = string.Format(@"(select b.Guid, b.Logo,b.Title,
b.Url, b.OperatorGuid,b.Sort,b.remark 
from TBrand b ) Tab");
            if (!string.IsNullOrEmpty(Request.QueryString["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request.QueryString["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            DataSet ds = Dal.GetPaged(id, pageSize, table, "*", where, "sort asc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            if (Request.IsAjaxRequest())
            {
                return View("_Table", pagelist);
            }
            SetViewData();
            return View(pagelist);
        }

        [HttpPost]
        public JsonResult AddPost(TBrand brand)
        {
            string msg = string.Empty;
            bool flag = false;
            if (brand.OperatorGuid == Guid.Empty)
            {
                brand.OperatorGuid = this.OperatorGuid;
            }
            if (brand.Guid == Guid.Empty)
            {
                flag = BrandLogic.Add(brand, out msg);
            }
            else
            {
                flag = BrandLogic.Edit(brand, out msg);
            }

            return Json(new { success = flag, msg = msg });
        }


        public JsonResult Delete(Guid guid)
        {
            string msg = string.Empty;
            bool flag = BrandLogic.Delete(guid, out msg);
            return Json(new { succ = flag, msg = msg });
        }
        private void SetViewData()
        {
            TManagerRole role = RoleLogic.GetEntity(this.manger.ManagerRoleGuid);
            GoodsCategoryLogic logic = new GoodsCategoryLogic();
            if (role.IsSystem)//运营商/超管
            {
                ViewData["CateGory"] = logic.GetSelectGoodsCategory(this.OperatorGuid, new Guid(), 0);
            }
            else
            {
                ViewData["CateGory"] = logic.GetSelectGoodsCategory(this.OperatorGuid, this.BusinessGuid, 1);
            }
        }
    }
}