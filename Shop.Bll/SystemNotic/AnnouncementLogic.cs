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
    public class AnnouncementLogic
    {
        public static bool Add(TAnnouncement model)
        {

            model.Guid = Guid.NewGuid();
            model.ReadCount = 0;
            model.CreateTime = DateTime.Now;
            model.UserAccount = "admin";

            dbContext db = new dbContext();
            db.TAnnouncement.Add(model);
            db.SaveChanges();
            return true;
        }

        public static bool Delete(Guid guid, out string msg)
        {
            dbContext db = new dbContext();
            TAnnouncement model = db.TAnnouncement.Where(c => c.Guid == guid).FirstOrDefault();
            if (model == null)
            {
                msg = "该公告不存在,可能已经被删除！";
                return false;
            }
            db.TAnnouncement.Remove(model);

            List<TAnnouncementReader> readerList = db.TAnnouncementReader.Where(c => c.AnnouncementGuid == model.Guid).ToList();
            if (readerList.Count > 0)
            {
                db.TAnnouncementReader.RemoveRange(readerList);
            }
            db.SaveChanges();
            msg = "删除成功!";
            return true;
        }


        public static bool Read(Guid guid, out string msg)
        {
            dbContext db = new dbContext();
            TAnnouncement model = db.TAnnouncement.Where(c => c.Guid == guid).FirstOrDefault();
            if (model == null)
            {
                msg = "该公告不存在,可能已经被删除！";
                return false;
            }
            TAnnouncementReader reader = new TAnnouncementReader();
            reader.AnnouncementGuid = guid;
            reader.OperatorGuid = model.OperatorGuid;
            reader.Guid = Guid.NewGuid();
            reader.OperterTime = DateTime.Now;
            reader.UserAccount = ManagerLogic.GetLoginInfo().LoginName;
            db.TAnnouncementReader.Add(reader);

            model.ReadCount += 1;
            db.SaveChanges();
            msg = "";
            return true;
        }
    }
}
