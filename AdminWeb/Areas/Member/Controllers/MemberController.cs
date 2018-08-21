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

namespace AdminWeb.Areas.Member.Controllers
{
    public class MemberController : AdminBaseController
    {
        // GET: Member/Member
        public ActionResult List(int id = 1)
        {
            string where = string.Format("1=1 and operatorGuid='{0}' ", this.OperatorGuid);
            if (!string.IsNullOrEmpty(Request["title"]))
            {
                where += string.Format(" and title like'%{0}%'", Request["title"]);
            }
            DalGeneric<TMemberCategory> Dal = new DalGeneric<TMemberCategory>();
            int total;
            string table = string.Format(@"(select m.Guid,m.OperatorGuid,m.Name MemberName,m.Mobile,c.Title,rm.Name RemName,m.RegSource,
m.CardID,m.AvailablePoint,m.AvailableValue,m.Status,m.HeadImg,m.Sex,m.RegTime,s.Name
 from TMember m

left join TMember rm on m.RecommendMemberGuid=rm.Guid
left join TMemberCategory c on m.CategoryGuid=c.Guid
left join TChainStore s on m.ChainStoreGuid =s.Guid) Tab");
            DataSet ds = Dal.GetPaged(id, 15, table, "*", where, "RegTime desc", out total);
            PagedList<DataRow> pagelist = new PagedList<DataRow>(ds.Tables[0].Select(), id, 10, total);
            return View(pagelist);
        }
    }
}