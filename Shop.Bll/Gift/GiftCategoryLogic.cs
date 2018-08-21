using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Shop.Dal;
using Shop.Entity;

using System.Web.Mvc;
namespace Shop.Bll
{
    public class GiftCategoryLogic
    {

        public bool Add(TGiftCategory model)
        {
            DalGeneric<TGiftCategory> dal = new DalGeneric<TGiftCategory>();
            int count = 0;
            if (model.Guid == Guid.Empty)
            {
                model.Guid = Guid.NewGuid();
                model.AddTime = DateTime.Now;
                count = dal.Add(model);
                if (count > 0)
                    return true;
                else
                    return false;
            }
            TGiftCategory oldModel = dal.GetBy(c => c.Guid == model.Guid);
            oldModel.Title = model.Title;
            oldModel.Remark = model.Remark;
            oldModel.Sort = model.Sort;

            if (dal.Modify(oldModel, "Title", "Sort", "Remark") > 0)
                return true;
            else
                return false;
        }
        public static List<SelectListItem> GetDropdownList(Guid BusinessGuid)
        {
            dbContext db = new dbContext();
            //      SelectList list=new SelectList ()
            List<SelectListItem> itemList = new List<SelectListItem>();
            var obj = db.TGiftCategory.Where(c => c.BusinessGuid == BusinessGuid).Select(c => new
            {
                Title = c.Title,
                Guid = c.Guid
            });
            foreach (var item in obj)
            {
                SelectListItem selectItem = new SelectListItem();
                selectItem.Text = item.Title;
                selectItem.Value = item.Guid.ToString();
                itemList.Add(selectItem);
            }
            return itemList;
        }
        public static TGiftCategory GetModel(Guid guid)
        {
            dbContext db = new dbContext();
            return db.TGiftCategory.Where(c => c.Guid == guid).FirstOrDefault();
        }
        public static bool Add(TGiftCategory Category, out string msg)
        {
            msg = "";

            return true;
        }
    }
}
