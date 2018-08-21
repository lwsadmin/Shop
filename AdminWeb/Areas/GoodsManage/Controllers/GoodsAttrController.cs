using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using System.Data;
using Shop.Bll;
using Shop.Dal;
using Shop.Entity;
using Shop.AdminWeb.Controllers;
using Shop.ViewModel;
namespace AdminWeb.Areas.GoodsManage.Controllers
{
    public class GoodsAttrController : AdminBaseController
    {
        // GET: GoodsManage/GoodsAttr
        public ActionResult List(int id = 1)
        {
            DalGeneric<TBrand> Dal = new DalGeneric<TBrand>();
            string where = string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);
            if (!string.IsNullOrEmpty(Request.Form["title"]))
            {
                where += string.Format(" and Name like '%{0}%'", Request.Form["title"].ToString());
            }
            if (!string.IsNullOrEmpty(Request.Form["GoodsCategory"]))
            {
                where += string.Format(@" and GoodsCategoryGuid='{0}'", Request.Form["GoodsCategory"]);
            }
            if (!string.IsNullOrEmpty(Request.Form["dataType"]))
            {
                where += string.Format(@" and DataType='{0}'", Request.Form["dataType"]);
            }
            int total; int pageSize = 15;
            string table = string.Format(@"(select p.Guid,p.OperatorGuid,p.DataType,p.Name,p.Searchable,
p.DefaultValue,p.Options,p.ValueName,p.GoodsCategoryGuid,c.Title
from dbo.TGoodsProperty p  left join TGoodsCategory c 
on p.GoodsCategoryGuid=c.Guid) Tab");
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            string fields = string.Format(@"Guid,OperatorGuid,DataType,Name,Searchable,ValueName,
DefaultValue, Options,GoodsCategoryGuid,Title");
            DataSet ds = Dal.GetPaged(id, pageSize, table, fields, where, "ValueName desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            if (Request.IsAjaxRequest())
            {
                return View("_Table", pagelist);
            }
            SetViewData();
            return View(pagelist);
        }


        public ActionResult Add(Guid? guid)
        {
            TGoodsProperty p = new TGoodsProperty();
            TManagerRole role = RoleLogic.GetEntity(this.manger.ManagerRoleGuid);
            GoodsCategoryLogic logic = new GoodsCategoryLogic();
            GoodsPropertyLogic pLogic = new GoodsPropertyLogic();
            if (role.IsSystem)//运营商/超管
            {

                ViewData["CateGory"] = logic.GetSelectGoodsCategory(this.OperatorGuid, new Guid(), 0);
            }
            else
            {
                ViewData["CateGory"] = logic.GetSelectGoodsCategory(this.OperatorGuid, this.BusinessGuid, 1);
            }
            if (guid != null)
            {
                p = pLogic.GetModel((Guid)guid);
            }
            else
            {
               
                p.OperatorGuid = this.OperatorGuid;
               
            }
            return PartialView("_Add", p);
        }

        public JsonResult Save(TGoodsProperty attr)
        {
            GoodsPropertyLogic logic = new GoodsPropertyLogic();
            string msg = string.Empty;
            attr.DataType = Request.Form["DataType"];
            bool flag = logic.Add(attr, out msg);
            return Json(new { succ = flag, msg = msg });
        }

        public JsonResult Delete(Guid guid)
        {
            GoodsPropertyLogic logic = new GoodsPropertyLogic();
            string msg = string.Empty;
            bool flag = logic.Delete(guid, out msg);
            return Json(new { succ = flag, msg = msg });
        }
        private void SetViewData()
        {
            GoodsCategoryLogic logic = new GoodsCategoryLogic();
            TManagerRole role = RoleLogic.GetEntity(this.manger.ManagerRoleGuid);
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