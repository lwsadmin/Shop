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
    public class GroupBuyController : AdminBaseController
    {
        // GET: GroupBuy/GroupBuy

        public ActionResult List(int pageIndex = 1)
        {


            string where = CommonLogic.GetWhereCondition();

            if (!string.IsNullOrEmpty(Request.Form["title"]))
            {
                where += string.Format(" and title like '%{0}%'", Request.Form["title"]);
            }
            if (!string.IsNullOrEmpty(Request.Form["Business"]))
            {
                where += string.Format(" and BusinessGuid = '{0}'", Request.Form["Business"]);
            }
            int pageSize = 15;
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewDataAdd();
            if (!Request.IsAjaxRequest())
            {

            }
            string table = string.Format(@"(select g.Guid, g.Title,b.BusinessName,g.BusinessGuid,
g.Price,g.MarketPrice,g.Status,g.StoreCount,g.sort,g.OperatorGuid,
g.SellCount,g.Images,g.AddTime from TRushBuy g 
left join TBusiness b on g.BusinessGuid=b.Guid
)Tab");
            ViewData["NowPageSize"] = pageSize;
            int totol;
            DalGeneric<object> dal = new DalGeneric<object>();
            DataSet ds = dal.GetPaged(pageIndex, pageSize, table, "*", where, " sort asc,addtime desc", out totol);
            PagedList<DataRow> pageList = new PagedList<DataRow>(ds.Tables[0].Select(), pageIndex, pageSize, totol);
            return View(pageList);
        }

        public ActionResult Add(Guid? id)
        {
            TRushBuy groupBuy = new TRushBuy();
            List<TImage> imageList = new List<TImage>();
            groupBuy.NeedBook = false;
            TImage newImg = new TImage();
            ImagesLogic logic = new ImagesLogic();
            if (id != null)
            {
                groupBuy = TRushBuyLogic.GetEntity((Guid)id);
                imageList = logic.GetImageList((Guid)id);
                while (imageList.Count < 6)
                {

                    imageList.Add(new TImage());
                }
            }
            else
            {
                for (int i = 0; i < 6; i++)
                {
                    imageList.Add(new TImage());
                }
            }
            ViewDataAdd();
            GroupBuyImage viewModel = new GroupBuyImage();
            viewModel.ImageList = imageList;
            viewModel.GroupBuy = groupBuy;
            return View(viewModel);
        }
        [ValidateInput(false)]
        public JsonResult AddSave(GroupBuyImage model)
        {
            string msg = string.Empty;
            if (model.GroupBuy.Guid == Guid.Empty)
            {
                model.GroupBuy.OperatorGuid = this.OperatorGuid;
                model.GroupBuy.BusinessGuid = this.BusinessGuid;
                model.GroupBuy.ChainStoreGuid = this.ChainStoreGuid;
            }
            bool flag = TRushBuyLogic.Add(model.GroupBuy, out msg);
            foreach (TImage img in model.ImageList)
            {
                if (string.IsNullOrEmpty(img.Url))
                {
                    continue;
                }
                img.Type = (int)CateGory.GroupBuy;
                img.OperatorGuid = model.GroupBuy.OperatorGuid;
                img.BusinessGuid = model.GroupBuy.BusinessGuid;
                img.ChainStoreGuid = model.GroupBuy.ChainStoreGuid;
                img.ObjectGuid = model.GroupBuy.Guid;
            }
            ImagesLogic.Add(model.ImageList);
            return Json(new { succ = flag, msg = msg });
        }

        public JsonResult DeleteImage(string guid)
        {
            string msg = string.Empty;
            bool flag = new ImagesLogic().DeleteImages(guid);
            return Json(new { succ = flag });
        }
        public void ViewDataAdd()
        {
            ViewData.Add("Business", BusinessLogic.GetSelect(this.manger.ManagerRoleGuid));
        }
    }
}