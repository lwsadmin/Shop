using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Shop.Entity;
using Shop.Dal;
using Shop.Help;
using System.Web;
namespace Shop.Bll
{
    public class ManagerLogic
    {
        public static bool Add(TManager manager)
        {
            DalGeneric<TManager> dal = new DalGeneric<TManager>();

            if (manager.Guid == Guid.Empty)
            {
                manager.Guid = Guid.NewGuid();
                manager.AddTime = DateTime.Now;
                manager.LastLoginTime = DateTime.Now;
                dal.Add(manager);
                return true;
            }

            TManager newManager = dal.GetBy(c => c.Guid == manager.Guid);
            newManager.LoginName = manager.LoginName;
            newManager.Name = manager.Name;
            newManager.Phone = manager.Phone;
            newManager.Status = manager.Status;
            newManager.Remark = manager.Remark;
            newManager.Email = manager.Email;
            newManager.ManagerRoleGuid = manager.ManagerRoleGuid;
            if (!string.IsNullOrEmpty(manager.LoginPwd))
            {
                string str = Safe.Encrypt(manager.LoginPwd);
                newManager.LoginPwd = Safe.Encrypt(manager.LoginPwd);
                int num = dal.Modify(newManager, "LoginName", "Name", "LoginPwd", "Phone", "Status", "Remark", "Email", "ManagerRoleGuid");
                if (num > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                int num = dal.Modify(newManager, "LoginName", "Name", "Phone", "Status", "Remark", "Email", "ManagerRoleGuid");
                if (num > 0)
                    return true;
                else
                    return false;
            }
        }

        public static bool Delete(string Guids, out string msg)
        {
            string[] GuidArray = Guids.Split(',');
            dbContext db = new dbContext();
            List<TManager> managerList = new List<TManager>();
            for (int i = 0; i < GuidArray.Length; i++)
            {
                if (string.IsNullOrEmpty(GuidArray[i]))
                    continue;
                Guid gd = new Guid(GuidArray[i]);
                TManager model = db.TManager.Where(c => c.Guid == gd).FirstOrDefault();
                if (model == null)
                {
                    msg = string.Format("操作员{0}不存在，可能已经被删除!", GuidArray[i]);
                    continue;
                }
                managerList.Add(model);
            }
            db.TManager.RemoveRange(managerList);
            db.SaveChanges();
            msg = "删除成功!";
            return true;
        }




        public static TManager GetLoginInfo()
        {
            TManager manager = null;
            string CooikeConfig = Config.GetWebConfigValueByKey("LoginInfo");
            if (string.IsNullOrEmpty(CooikeConfig))
            {
                return new TManager();
            }
            string DecodeCooike = HttpUtility.UrlDecode(Safe.GetCookie(CooikeConfig));
            if (string.IsNullOrEmpty(DecodeCooike))
            {
                return manager;
            }
            manager = (TManager)JsonHelper.JsonToObject(DecodeCooike, new TManager());

            return manager;
        }
    }
}
