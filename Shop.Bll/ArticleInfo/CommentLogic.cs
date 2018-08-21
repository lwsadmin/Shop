using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Dal;
using Shop.Entity;
using Shop.Dal.Emnu;

namespace Shop.Bll.ArticleInfo
{
    public class CommentLogic
    {
        public bool Reply(Guid guid, string Content, out string msg)
        {
            dbContext db = new dbContext();

            TComment com = db.TComment.Where(c => c.Guid == guid).First();
            if (com == null)
            {
                msg = "该评论不存在,可能已经被删除!";
                return false;
            }
            TComment reply = new TComment();
            reply.ParentGuid = com.Guid;
            reply.OperatorGuid = com.OperatorGuid;
            reply.ObjectGuid = com.ObjectGuid;
            reply.Content = Content;
            reply.StartLevel = 0;
            reply.Type = com.Type;
            reply.SubmitTime = DateTime.Now;
            db.TComment.Add(reply);
            db.SaveChanges();
            msg = "回复成功!";
            return true;

        }

        public bool ChangeStatus(Guid guid, out string msg)
        {
            dbContext db = new dbContext();

            TComment com = db.TComment.Where(c => c.Guid == guid).First();
            if (com == null)
            {
                msg = "该评论不存在,可能已经被删除!";
                return false;
            }
            if (com.Status == 0)
            {
                com.Status = 1;
            }
            else
            {
                com.Status = 0;
            }
            db.SaveChanges();
            msg = "保存成功!";
            return true;
        }
    }
}
