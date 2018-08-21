using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Dal;
using Shop.Entity;
using Shop.Dal.Emnu;
namespace Shop.Bll
{
    public class ArticleLogic
    {
        public static bool Add(TArticle model)
        {
            bool flag = false;
            DalGeneric<TArticle> dal = new DalGeneric<TArticle>();
            if (model.Guid == Guid.Empty)
            {
                model.Guid = Guid.NewGuid();
                model.IsManager = true;
                model.UserGuid = Guid.Empty;
                model.AddTime = DateTime.Now;
                if (dal.Add(model) > 0)
                    flag = true;
            }
            else
            {
                TArticle oldModel = dal.GetBy(c => c.Guid == model.Guid);
                oldModel.Title = model.Title;
                oldModel.author = model.author;
                oldModel.CategoryGuid = model.CategoryGuid;
                oldModel.Content = model.Content;
                oldModel.Souce = model.Souce;
                oldModel.Sort = model.Sort;
                oldModel.Status = model.Status;
                oldModel.Level = model.Level;
                oldModel.SmallImg = model.SmallImg;
                oldModel.Target = model.Target;
                oldModel.Describle = model.Describle;
                int ExnoQuery = dal.Modify(oldModel, "Title", "author", "CategoryGuid", "Content", "Souce", "Sort", "Status", "Level", "Target", "SmallImg", "Describle");
                if (ExnoQuery > 0)
                    flag = true;

            }
            return flag;
        }

        public static bool Delete(string GuidStrings)
        {
            dbContext db = new dbContext();
            string[] GuidArray = GuidStrings.Split(',');
            List<TArticle> articleList = new List<TArticle>();

            for (int i = 0; i < GuidArray.Length; i++)
            {
                if (!string.IsNullOrEmpty(GuidArray[i].ToString()))
                {
                    Guid gu = new Guid(GuidArray[i].ToString());
                    TArticle model = db.TArticle.Where(c => c.Guid == gu).FirstOrDefault();
                    articleList.Add(model);
                }
            }
            db.TArticle.RemoveRange(articleList);
            if (db.SaveChanges() > 0)
                return true;
            else
                return false;

        }
    }
}
