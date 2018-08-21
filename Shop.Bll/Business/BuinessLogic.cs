using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Dal;
using System.Web;
using System.Web.Mvc;
using System.Linq.Expressions;
namespace Shop.Bll
{
    public static class BusinessLogic
    {
        public static bool Add(TBusiness model)
        {
            bool flag = false;
            DalGeneric<TBusiness> dal = new DalGeneric<TBusiness>();
            DalGeneric<TChainStore> StoreDal = new DalGeneric<TChainStore>();
            if (model.Guid == Guid.Empty)
            {
                model.Guid = Guid.NewGuid();
                model.RegisterTime = DateTime.Now;
                model.StoreCount = 1;
                model.IsSystemBusiness = false;
                dal.Add(model);

                TChainStore store = new TChainStore();
                store.Name = model.BusinessName + "总部";
                store.DistrictId = model.DistrictId;
                store.CityId = model.CityID;
                store.ProvinceId = model.ProvinceId;
                store.Sort = (int)model.Sort;
                store.Address = model.Address;
                store.CreateTime = DateTime.Now;
                store.IsSystem = true;
                store.Introduce = model.Memo;
                store.Mobile = model.Tel;
                store.Contact = model.Contact;
                store.Guid = Guid.NewGuid();
                store.OperatorGuid = model.OperatorGuid;
                store.BusinessGuid = model.Guid;
               
                store.AvailablePoint = 0;
                store.AvailableValue = 0;
                store.AveragePrice = 0;
                store.TotalValue = 0;
                store.TotalPoint = 0;
                store.SettlementMoney = 0;

                if (StoreDal.Add(store) > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                TBusiness oldModel = dal.GetBy(c => c.Guid == model.Guid);
                oldModel.BusinessName = model.BusinessName;
                oldModel.Address = model.Address;
                oldModel.OperatorGuid = model.OperatorGuid;
                oldModel.AgentsGuid = model.AgentsGuid;
                oldModel.ProvinceId = model.ProvinceId;
                oldModel.CityID = model.CityID;
                oldModel.DistrictId = model.DistrictId;
                oldModel.Memo = model.Memo;
                oldModel.Contact = model.Contact;
                oldModel.Tel = model.Tel;
                oldModel.IndustryGuid = model.IndustryGuid;
                oldModel.Sort = model.Sort;
                if (dal.UpdateEntity(oldModel))
                    flag = true;
                else
                    flag = false;
            }
            return flag;
        }

        public static IEnumerable<SelectListItem> GetSelect(Guid mamageRoleGuid)
        {
            IEnumerable<SelectListItem> result;
            SelectListItem item = new SelectListItem();
            item.Value = "";
            item.Text = "-请选择-";
            TManagerRole role = RoleLogic.GetEntity(mamageRoleGuid);
            dbContext db = new dbContext();
            if (role.IsSystem)//运营商/超管/总部管理员
            {
                // result=db.Set<TBusiness>().Where(whereLambda)
                result = from u in db.TBusiness
                         where u.OperatorGuid == role.OperatorGuid
                         orderby u.RegisterTime descending
                         select new SelectListItem
                         {
                             Value = u.Guid.ToString().ToLower(),
                             Text = u.BusinessName
                         };

                return result;
            }
            else
            {
                result = from c in db.TBusiness
                         where c.Guid == role.BusinessGuid
                         orderby c.RegisterTime descending
                         select new SelectListItem
                         {
                             Text = c.BusinessName,
                             Value = c.Guid.ToString()
                         };

                return result;
            }

        }
    }
}
