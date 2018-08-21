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
    public class GoodsController : AdminBaseController
    {
        // GET: GoodsManage/Goods
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
                where += string.Format(@" and GoodsCategoryGuid='{0}'", Request.Form["GoodsCategory"]);
            }
            if (!string.IsNullOrEmpty(Request.Form["BrandGuid"]))
            {
                where += string.Format(@" and BrandGuid='{0}'", Request.Form["BrandGuid"]);
            }
            int total; int pageSize = 15;
            string table = string.Format(@"(select g.Guid,g.OperatorGuid,g.BusinessGuid,g.Title,g.Sort,g.SellCount,g.OnLinePrice,g.IsTakeOut,g.DistributionMoney,g.MarketPrice,g.addtime
,b.Title BTitle,c.Title CTitle,u.BusinessName from TGoods g
left join TGoodsCategory c
on g.GoodsCategoryGuid=c.Guid
left join TBrand b on g.BrandGuid=b.Guid
left join TBusiness u on g.BusinessGuid=u.Guid) Tab");
            if (!string.IsNullOrEmpty(Request["PageSize"]))
            {
                pageSize = Convert.ToInt32(Request["PageSize"]);
            }
            ViewData["NowPageSize"] = pageSize;
            DataSet ds = Dal.GetPaged(id, pageSize, table, "*", where, "sort asc,addtime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, pageSize, total);
            if (Request.IsAjaxRequest())
            {
                return View("_Table", pagelist);
            }
            SetViewData();
            return View(pagelist);
        }

        public ActionResult Add(string id)
        {
            SetViewData();
            DalGeneric<TGoods> dal = new DalGeneric<TGoods>();
            TGoods goods = new TGoods();
            TImage goodsImages = new TImage();
            GoodsModel goodsModel = new GoodsModel();
            List<TImage> imageList = new List<TImage>();
            ViewBag.Imgs = ""; ViewBag.Config = "";
            if (!string.IsNullOrEmpty(id))
            {
                goods = dal.GetBy(c => c.Guid == new Guid(id));
                ImagesLogic imageLogic = new ImagesLogic();
                imageList = imageLogic.GetImageList(new Guid(id));
                string imgs = "", config = "";
                for (int i = 0; i < imageList.Count; i++)
                {
                    if (string.IsNullOrEmpty(imageList[i].Url))
                        continue;
                    if (i == imageList.Count - 1)
                        imgs += string.Format("\'{0}\'", imageList[i].Url);
                    else
                        imgs += string.Format("\'{0}\',", imageList[i].Url);

                    config += string.Format(@"{ 
                            url:'/common/common/ImageDelete/{0}', key: '{0}',size:{1}},", imageList[i].Guid.ToString(), imageList[i].Size);
                }
            }
            else
            {
                goods.OperatorGuid = this.OperatorGuid;
                goods.ChainStoreGuid = this.ChainStoreGuid;
                goods.AllowDistribution = false;
                goods.Status = 0;
            }
            while (imageList.Count < 6)
            {
                TImage img = new TImage();
                img.Url = "";
                imageList.Add(img);
            }
            goodsModel.goods = goods;
            goodsModel.goodsImges = imageList;
            return View(goodsModel);
        }

        [HttpPost]
        public string GetGoodsAttr(Guid? goodsCategoryGuid)
        {

            if (goodsCategoryGuid != null && goodsCategoryGuid != Guid.Empty)
            {
                GoodsPropertyLogic logic = new GoodsPropertyLogic();
                string s = logic.GetGoodsAttr((Guid)goodsCategoryGuid);
                return s;
            }
            else
            {
                return "";
            }

            // return PartialView("_tabAttr");
        }


        [HttpPost]
        [ValidateInput(false)]
        public JsonResult AddPost(GoodsModel model)
        {
            string msg = string.Empty;
            bool flag = true;
            GoodsLogic logic = new GoodsLogic();
            if (model.goods.Guid == Guid.Empty)
            {
                model.goods.OperatorGuid = this.OperatorGuid;

                model.goods.ChainStoreGuid = this.ChainStoreGuid;
            }
            flag = logic.Add(model, out msg);
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

            ViewData["Brand"] = BrandLogic.GetBrandSelectList(this.OperatorGuid);

            ViewData["Business"] = BusinessLogic.GetSelect(role.Guid);
        }
    }
}