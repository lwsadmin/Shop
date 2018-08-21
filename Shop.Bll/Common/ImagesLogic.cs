using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
namespace Shop.Bll
{
    public class ImagesLogic
    {
        public static bool Add(List<TImage> list)
        {
            dbContext db = new dbContext();
            list = list.Where(c => c.Url != null && c.Url != "" && c.Guid == Guid.Empty).ToList();
            foreach (TImage item in list)
            {
                if (item.Guid == Guid.Empty)
                {
                    item.Guid = Guid.NewGuid();
                    item.UploadTime = DateTime.Now;
                    item.Sort = 0;
                }
                else { }
            }
            db.TImage.AddRange(list);
            db.SaveChanges();
            return true;
        }

        public List<TImage> GetImageList(Guid ObjectGuid)
        {
            dbContext db = new dbContext();
            return db.TImage.Where(c => c.ObjectGuid == ObjectGuid).OrderBy(c => c.Sort).ThenByDescending(c => c.UploadTime).ToList();
        }

        public bool DeleteImages(Guid objectGuid)
        {
            dbContext db = new dbContext();
            List<TImage> list = db.TImage.Where(c => c.ObjectGuid == objectGuid).ToList();
            if (list == null || list.Count <= 0)
                return false;
            db.TImage.RemoveRange(list);
            foreach (TImage item in list)
            {
                if (string.IsNullOrEmpty(item.Url))
                    continue;
                CommonLogic.DeleteImages(item.Url);
            }

            db.SaveChanges();
            return true;

        }
        public bool DeleteImages(string Guids)
        {
            dbContext db = new dbContext();
            List<TImage> list = new List<TImage>();
            //Guid本身带有'-',不可用'-'分隔
            string[] GuidArray = Guids.Split(',');
            for (int i = 0; i < GuidArray.Length; i++)
            {
                if (string.IsNullOrEmpty(GuidArray[i]))
                {
                    continue;
                }
                Guid guid = new Guid(GuidArray[i]);
                TImage img = db.TImage.Where(c => c.Guid == guid).FirstOrDefault();
                if (img != null)
                {
                    CommonLogic.DeleteImages(img.Url);
                    list.Add(img);
                }
            }
            db.TImage.RemoveRange(list);
            db.SaveChanges();
            return true;
        }

        public static bool UpdateImage(Guid ImageGuid, string Url, out string msg)
        {
            dbContext db = new dbContext();
            TImage img = db.TImage.Where(c => c.Guid == ImageGuid).FirstOrDefault();
            if (img == null)
            {
                msg = "该图片不存在，可能已经被删除!";
                return false;
            }
            //   CommonLogic.DeleteImages(img.Url);
            img.Url = Url;
            db.SaveChanges();
            msg = "";
            return true;
        }
    }
}
