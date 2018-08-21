using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Dal;

namespace Shop.Bll
{
    public class MemberLogic
    {
        public static bool Reg(TMember member, int RegSource, out string msg)
        {
            try
            {
                member.Guid = Guid.NewGuid();
                if (member.NickName == null)
                {
                    member.NickName = member.Name;
                }
                member.RegTime = DateTime.Now;
                member.RegSource = RegSource;
                member.Status = 0;
                dbContext db = new dbContext();
                db.TMember.Add(member);
                db.SaveChanges();
                msg = "注册成功!";
                return true;
            }
            catch (Exception e)
            {

                throw;
            }

        }

        public TMember GetByMobile(string Mobile,Guid OperatorGuid)
        {
            dbContext db = new dbContext();
            return db.TMember.Where(c => c.OperatorGuid == OperatorGuid && c.Mobile == Mobile).FirstOrDefault();
        }

        public static object GetMemberInfoByCardIdOrMobile(string cardId, Guid OperatorGuid)
        {
            if (string.IsNullOrEmpty(cardId))
                return new
                {
                    Success = false,
                    Message = "请输入卡号或手机号！"
                };
            dbContext db = new dbContext();
            TMember member = db.TMember.SingleOrDefault(m => m.CardID == cardId && m.OperatorGuid == OperatorGuid);
            if (member == null)
                member = db.TMember.SingleOrDefault(m => m.Mobile == cardId && m.OperatorGuid == OperatorGuid);
            if (member == null)
                return new
                {
                    Success = false,
                    Message = "不存在该会员！"
                };
            if (member.Status == -1)
                return new
                {
                    Success = false,
                    Message = "该会员已被锁定！"
                };
            if (member.Status == -2)
                return new
                {
                    Success = false,
                    Message = "该会员已被删除！"
                };
            //   QuickConsumeLogic quickConsumeLogic = new QuickConsumeLogic(OpertorGuid);
            string pointDesc, discountDesc; int pointLeef;
            var obj = new
            {
                Success = true,
                Guid = member.Guid,
                Name = member.NickName,
                Mobile = member.Mobile,
                CategoryGuid = member.CategoryGuid,
                CategoryName = db.TMemberCategory.Single(m => m.Guid == member.CategoryGuid).Title,
                Point = member.AvailablePoint.ToString("f2"),
                Value = member.AvailableValue.ToString("f2"),
                RegTime = member.RegTime.ToString("yyyy-MM-dd")
            };
            return obj;
        }
    }
}
