using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using Shop.Help;
using Shop.Entity;
using Newtonsoft.Json;
using Shop.Dal;
using Shop.AdminWeb.Controllers;
using Webdiyer.WebControls.Mvc;
using System.Data;
using Shop.Dal.Emnu;
using Shop.Bll;

namespace AdminWeb.Areas.GoodsManage.Controllers
{
    public class GoodsCategoryController : AdminBaseController
    {
        // GET: GoodsManage/GoodsCategory
        public ActionResult List(int id = 1)
        {
            string where = string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);

            GoodsCategoryLogic logic = new GoodsCategoryLogic();

            if (!string.IsNullOrEmpty(Request["title"]))
            {
                ViewData["HasChild"] = false;
                //两边加%会导致索引失效，数据量大的时候查询较慢， 建议用union all
                where += string.Format(" and title like'%{0}%'", Request["title"].Trim());
            }
            else { ViewData["HasChild"] = true; where += string.Format(" and ParentGuid is null"); }
            DalGeneric<TGoodsCategory> Dal = new DalGeneric<TGoodsCategory>();
            int total; int pageSize = 15;
            string table = string.Format(@"(select c.Guid,c.OperatorGuid, c.parentGuid,c.Sort,c.Title,c.AddTime,b.BusinessName,
c.Remark from TGoodsCategory c left join dbo.TBusiness b
on c.BusinessGuid=b.Guid) Tab");
            DataSet ds = Dal.GetPaged(id, pageSize, table, "*", where, "sort asc,addtime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            if (!string.IsNullOrEmpty(Request.QueryString["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request.QueryString["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_Table", pagelist);
            }
            ViewDataAdd();
            return View(pagelist);
        }
        public ActionResult Add(Guid? guid)
        {
            TGoodsCategory goodsCategory = new TGoodsCategory();
            ViewBag.Brand = BrandLogic.GetBrandSelectList(this.OperatorGuid);
            GoodsCategoryLogic logic = new GoodsCategoryLogic();
            if (guid != null)
            {
                goodsCategory = logic.GetModel((Guid)guid);
            }
            else
            {
                goodsCategory.BusinessGuid = this.BusinessGuid;
                goodsCategory.OperatorGuid = this.OperatorGuid;
                goodsCategory.ChainStoreGuid = this.ChainStoreGuid;
            }
            ViewDataAdd();
            return PartialView("Add", goodsCategory);
        }

        public JsonResult AddSave(TGoodsCategory GoodsCategory)
        {
            GoodsCategoryLogic logic = new GoodsCategoryLogic();
            string msg = string.Empty;
            GoodsCategory.Brand = Request.Form["Brand"];
            bool flag = logic.Add(GoodsCategory, out msg);
            return Json(new { succ = flag, msg = msg });
        }
        public JsonResult Delete(Guid guid)
        {
            GoodsCategoryLogic logic = new GoodsCategoryLogic();
            string msg = string.Empty;
            bool flag = logic.Delete(guid, out msg);
            return Json(new { succ = flag, msg = msg });
        }

        [HttpPost]
        public JsonResult GetChildCategory(string ParentGuid)
        {
            GoodsCategoryLogic logic = new GoodsCategoryLogic();
            var list = logic.GetJsonByParentGuid(new Guid(ParentGuid));
            if (list == null)
            {
                return Json(new { data = "" });
            }
            return Json(new { data = list });
        }

        private void ViewDataAdd()
        {
            TManagerRole role = RoleLogic.GetEntity(this.manger.ManagerRoleGuid);
            GoodsCategoryLogic logic = new GoodsCategoryLogic();
            if (role.IsSystem)//运营商/超管
                ViewData["CateGory"] = logic.GetSelectGoodsCategory(this.OperatorGuid, new Guid(), 0);
            else
                ViewData["CateGory"] = logic.GetSelectGoodsCategory(this.OperatorGuid, this.BusinessGuid, 1);
            ViewData.Add("Business", BusinessLogic.GetSelect(this.manger.ManagerRoleGuid));
        }
    }
}