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

namespace AdminWeb.Areas.GroupBuy.Controllers
{
    public class ImageController : Controller
    {
        // GET: GroupBuy/Image
        public ActionResult List(int pageIndex = 1)
        {
            string where = CommonLogic.GetWhereCondition();
            if (!string.IsNullOrEmpty(Request["title"]))
            {
                where += string.Format(" and title like'%{0}%'", Request["title"]);
            }
            string table = string.Format(@"(select i.Guid,i.OperatorGuid,i.BusinessGuid,i.ChainStoreGuid,i.ObjectGuid,
i.Sort as ImgSort,i.UploadTime,i.Url,
g.Title,g.Sort,g.AddTime from timage i left join TRushBuy g
on i.ObjectGuid=g.Guid)Tab");
            int pageSize = 15;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            int totol;
            DalGeneric<object> dal = new DalGeneric<object>();
            DataSet ds = dal.GetPaged(pageIndex, pageSize, table, "*", where, "Sort  asc,AddTime desc", out totol);
            PagedList<DataRow> pgaeList = new PagedList<DataRow>(ds.Tables[0].Select(), pageIndex, pageSize, totol);
            return View(pgaeList);
        }

        public ActionResult Delete(string Guids)
        {
            string msg = string.Empty;
            bool flag = new ImagesLogic().DeleteImages(Guids);
            return Json(new { succ = flag });
        }
        [HttpPost]
        public JsonResult ReUpload(string guid)
        {
            string msg = string.Empty;
            string url = Request["url"];
            guid = Request["guid"];
            bool flag = ImagesLogic.UpdateImage(new Guid(guid), url, out msg);
            return Json(new { succ = flag, msg = msg });
        }
    }
}