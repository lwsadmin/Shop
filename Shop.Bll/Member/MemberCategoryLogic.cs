using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Dal;


namespace Shop.Bll
{
    public class MemberCategoryLogic
    {
        public static bool Add(TMemberCategory memberCategory, out string msg)
        {
            if (memberCategory != null)
            {
                dbContext db = new dbContext();
                if (memberCategory.Guid == Guid.Empty)
                {
                    memberCategory.Guid = Guid.NewGuid();
                    db.TMemberCategory.Add(memberCategory);
                    db.SaveChanges();
                    msg = "添加成功!";
                    return true;
                }
                else
                {
                    TMemberCategory sqlModel = db.TMemberCategory.Where(c => c.Guid == memberCategory.Guid).FirstOrDefault();
                    if (sqlModel == null)
                    {
                        msg = "该数据不存在,可能已经被删除！";
                        return false;
                    }
                    sqlModel.Title = memberCategory.Title;
                    sqlModel.DefaultPoint = memberCategory.DefaultPoint;
                    sqlModel.DefaultValue = memberCategory.DefaultValue;
                    sqlModel.SoldMoney = memberCategory.SoldMoney;
                    sqlModel.RecommendPoint = memberCategory.RecommendPoint;
                    sqlModel.UpgradeNeedPoint = memberCategory.UpgradeNeedPoint;
                    sqlModel.MemberRight = memberCategory.MemberRight;
                    db.SaveChanges();
                    msg = "修改成功!";
                    return true;
                }
            }
            else
            {
                msg = "实体类型不可为空！";
                return false;
            }
        }


        public static bool Delete(Guid guid, out string msg)
        {
            dbContext db = new dbContext();
            TMemberCategory memberCategory = db.TMemberCategory.Where(c => c.Guid == guid).FirstOrDefault();
            if (memberCategory == null)
            {
                msg = "该数据信息不存在，可能已经被删除!";
                return false;
            }
            int count = db.TMember.Where(c => c.CategoryGuid == guid).Count();
            if (count > 0)
            {
                msg = string.Format("改会员级别包含{0}个会员，无法删除!", count);
                return false;
            }
            db.TMemberCategory.Remove(memberCategory);
            db.SaveChanges();
            msg = "删除成功!";
            return true;
        }

        
        public static IEnumerable<TMemberCategory> GetList(Guid OperatorGuid)
        {
            dbContext db = new dbContext();

            return db.TMemberCategory.Where(c => c.OperatorGuid == OperatorGuid).ToArray();
        }
    }

}
