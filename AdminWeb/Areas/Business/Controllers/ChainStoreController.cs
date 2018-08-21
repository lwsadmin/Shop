using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web.Mvc;

using Shop.Help;
using Shop.Entity;
using Shop.Dal;
using Shop.Bll;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
using Shop.ViewModel;
using Shop.Dal.Emnu;
namespace Shop.AdminWeb.Areas.Business.Controllers
{
    public class ChainStoreController : AdminBaseController
    {
        // GET: Business/ChainStore
        public ActionResult List(int id = 1)
        {
            string where = CommonLogic.GetWhereCondition();

            if (!string.IsNullOrEmpty(Request["Business"]))
            {
                where += string.Format(" and BusinessGuid='{0}'", Request["Business"]);
            }
            if (!string.IsNullOrEmpty(Request["StoreName"]))
            {
                where += string.Format(" and Name like '%{0}%'", Request["StoreName"]);
            }
            int pageSize = 15;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            DalGeneric<TBusiness> dalBusiness = new DalGeneric<TBusiness>();
            string table = @"(select c.Guid,c.Image,c.Name,c.Contact,c.BusinessGuid,
c.Mobile,c.Sort,c.IsTakeOut,c.CreateTime,c.OperatorGuid,c.Longitude,c.Latitude,
b.BusinessName from TChainStore c left join 
TBusiness b  on c.BusinessGuid = b.Guid)tab";
            int total;
            DataSet ds = dalBusiness.GetPaged(id, pageSize, table, "*", where, "sort desc, Createtime desc", out total);
            PagedList<DataRow> pageList = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize);
            if (Request.IsAjaxRequest())
            {
                return View("_Table", pageList);
            }
            ViewDataAdd();
            return View(pageList);
        }

        public ActionResult Add(Guid? id)
        {
            StoreView store = new StoreView();
            TChainStore chainStore = new TChainStore();
            List<TImage> imgList = new List<TImage>();

            ViewDataAdd();

            DalGeneric<TChainStore> dal = new DalGeneric<TChainStore>();
            ViewData["config"] = "";
            if (id != null)
            {
                chainStore = dal.GetBy(c => c.Guid == id);
                DalGeneric<TImage> dalImg = new DalGeneric<TImage>();
                imgList = dalImg.GetListBy(c => c.ObjectGuid == (Guid)id);
                string str = "", config = "";
                for (int i = 0; i < imgList.Count; i++)
                {
                    if (string.IsNullOrEmpty(imgList[i].Url))
                    {
                        continue;
                    }
                    if (i == imgList.Count - 1)
                        str += string.Format("\'{0}\'", imgList[i].Url);
                    else
                        str += string.Format("\'{0}\',", imgList[i].Url);

                    config += "{ url:'/common/common/ImageDelete/" + imgList[i].Guid.ToString() + "', key: '" + imgList[i].Guid.ToString() + "' },";
                }
                ViewData["str"] = str;
                ViewData["config"] = config;
            }
            else
            {
                chainStore.IsTakeOut = false;
                chainStore.ProvinceId = 1;
                chainStore.CityId = 1;
                chainStore.DistrictId = 1;
                ViewData["str"] = string.Format("");
            }
            do
            {
                imgList.Add(new TImage());

            } while (imgList.Count < 5);
            store.store = chainStore;
            store.storeImges = imgList;
            return View(store);
        }
        [HttpPost]
        [ValidateInput(false)]
        public JsonResult Add(StoreView model)
        {
            List<TImage> imgList = new List<TImage>();
            model.store.ProvinceId = Convert.ToInt32(Request.Form["ProvinceId"]);
            model.store.CityId = Convert.ToInt32(Request.Form["CityId"]);
            model.store.DistrictId = Convert.ToInt32(Request.Form["DistrictId"]);

            model.storeImges = model.storeImges.Where(c => c.Url != null && c.Url != "").ToList();
            for (int i = 0; i < model.storeImges.Count(); i++)
            {
                model.storeImges[i].BusinessGuid = this.BusinessGuid;
                model.storeImges[i].OperatorGuid = this.OperatorGuid;
                model.storeImges[i].ChainStoreGuid = model.store.Guid;
                model.storeImges[i].UploadTime = DateTime.Now;
                model.storeImges[i].Guid = Guid.NewGuid();
                model.storeImges[i].ObjectGuid = model.storeImges[i].ChainStoreGuid;
                model.storeImges[i].Type = (int)ImageType.门店;

            }

            if (model.store.Guid == Guid.Empty)
            {
                model.store.OperatorGuid = this.OperatorGuid;
                return Json(new
                {
                    succ = ChainStoreLogic.Add(model)
                });
            }
            else
            {
                string msg = string.Empty;
                return Json(new
                {
                    succ = ChainStoreLogic.Edit(model, out msg),
                    msg = msg
                });
            }
        }

        public ActionResult Map()
        {
            return View();
        }
        [HttpPost]
        public JsonResult SaveMap(string guid, string lng, string lat)
        {
            string msg = string.Empty;
            bool flag = ChainStoreLogic.SaveMap(new Guid(guid), lng, lat, out msg);
            return Json(new { succ = flag, msg = msg });
        }
        public void ViewDataAdd()
        {
            //IEnumerable<SelectListItem> list = BusinessLogic.GetSelect();
            ViewData.Add("Business", BusinessLogic.GetSelect(this.manger.ManagerRoleGuid));
        }
    }
}