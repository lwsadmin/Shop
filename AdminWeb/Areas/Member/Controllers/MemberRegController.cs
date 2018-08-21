using Shop.Dal;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using Shop.Entity;
using System.Web.Mvc;
using System.Data.Sql;
using Webdiyer.WebControls.Mvc;
using Shop.AdminWeb.Controllers;
using Shop.Bll;
using Shop.Dal.Emnu;

namespace AdminWeb.Areas.Member.Controllers
{
    public class MemberRegController : AdminBaseController
    {
        // GET: Member/MemberReg
        public ActionResult Index()
        {
            TMember mem = new TMember();
            mem.OperatorGuid = this.OperatorGuid;
            mem.BusinessGuid = this.BusinessGuid;
            mem.ChainStoreGuid = this.ChainStoreGuid;
            mem.ProvinceId = 1; mem.CityId = 1; mem.DistrictId = 1;
            IEnumerable<TMemberCategory> cateGory = new List<TMemberCategory>();

            cateGory = MemberCategoryLogic.GetList(this.OperatorGuid);
            SelectList select = new SelectList(cateGory, "Guid", "Title");
            ViewData["MemberCategory"] = select;
            return View(mem);
        }

        public JsonResult Save(TMember member)
        {
            string msg = string.Empty;
            bool flag = MemberLogic.Reg(member, (int)MemberSource.门店登记, out msg);

            return Json(new { succ = flag, msg = msg });
        }

        public JsonResult CheckExists()
        {
            MemberLogic logic = new MemberLogic();
            TMember mem = logic.GetByMobile(Request.Form["param"], this.OperatorGuid);

            if (mem == null)
            {
                return Json(new { status = "y", msg = "" });
            }

            else
            {
                return Json(new { status = "n", msg = "该手机号已经被注册!" });

            }
        }

        public JsonResult CheckRemExists(string Mobile)
        {
            MemberLogic logic = new MemberLogic();
            TMember mem = logic.GetByMobile(Request.Form["param"], this.OperatorGuid);
            if (mem != null)
            {
                return Json(new { status = "y", msg = mem.Guid, name = mem.Name });
            }

            else
            {
                return Json(new { status = "n", msg = "该手机号不存在!", name = "" });

            }
        }
    }
}