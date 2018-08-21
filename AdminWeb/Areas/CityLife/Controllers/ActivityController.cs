using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Shop.Bll;
using Shop.Dal;
using Shop.Entity;
using Shop.Help;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;

namespace AdminWeb.Areas.CityLife.Controllers
{
    public class ActivityController : AdminBaseController
    {
        // GET: CityLife/Acitvity
        public ActionResult CategoryList(int id = 1)
        {
            string where = string.Format("1=1 and operatorGuid='{0}'", this.OperatorGuid);
            if (!string.IsNullOrEmpty(Request["title"]))
            {
                where += string.Format(" and title like'%{0}%'", Request["title"]);
            }

            int pageSize = 15;
            if (!string.IsNullOrEmpty(Request.Form["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request.Form["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            DalGeneric<TArticleCategory> Dal = new DalGeneric<TArticleCategory>();
            string table = string.Format(@"(select Guid,OperatorGuid,Title,Sort,Remark from TActivityCategory) Tab");
            int total;
            DataSet ds = Dal.GetPaged(id, pageSize, table, "*", where, "sort asc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            if (Request.IsAjaxRequest())
            {
                return View("_CategoryTable", pagelist);
            }
            return View(pagelist);
        }

        // GET: CityLife/Acitvity/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CityLife/Acitvity/Create
        public ActionResult CategoryEdit(Guid? guid)
        {
            TActivityCategory Category = new TActivityCategory();
            ActivityLogic logic = new ActivityLogic();
            Category = logic.GetModel((Guid)guid);
            if (Category == null)
                Category = new TActivityCategory();
            return PartialView("_CategoryAdd", Category);
        }

        // POST: CityLife/Acitvity/CategoryAdd
        [HttpPost]
        public JsonResult CategoryAdd(TActivityCategory model)
        {
            bool flag = true; string msg = string.Empty;
            try
            {
                // TODO: Add insert logic here
                if (model.OperatorGuid == Guid.Empty)
                    model.OperatorGuid = this.OperatorGuid;

                ActivityLogic logic = new ActivityLogic();
                bool falg = logic.AddCategory(model, out msg);
                return Json(new { succ = flag, msg = msg });
            }
            catch (Exception e)
            {
                return Json(new { succ = false, msg = e.Message });
            }
        }

        // GET: CityLife/Acitvity/Edit/5
        public ActionResult CategoryDelete(string id)
        {
            Guid gd = new Guid(id);
            string msg = string.Empty;
            ActivityLogic logic = new ActivityLogic();
            bool flag = logic.CategoryDelete(id, out msg);
            return Json(new { succ = flag, msg = msg });
        }

        // POST: CityLife/Acitvity/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CityLife/Acitvity/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CityLife/Acitvity/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
