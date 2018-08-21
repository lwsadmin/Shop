using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using Shop.Entity;
using Shop.Dal;
using Shop.Help;
using System.Web.Mvc;

namespace Shop.Bll
{
    public class Power//权限类
    {

        public string Name { get; set; }
        public string ActionString { get; set; }
    }
    public class RoleLogic
    {

        public static TManagerRole GetEntity(Guid RoleGuid)
        {
            return new dbContext().TManagerRole.Where(c => c.Guid == RoleGuid).FirstOrDefault();
        }


        public static string GetManageRoleWhere()
        {
            dbContext db = new dbContext();
            TManager model = ManagerLogic.GetLoginInfo();
            TManagerRole mangeRole = db.TManagerRole.Where(c => c.Guid == model.ManagerRoleGuid).FirstOrDefault();

            if (mangeRole.IsSystem)//超管
            {
                return string.Format(" 1=1 and OperatorGuid='{0}'", mangeRole.OperatorGuid);
            }
            if (mangeRole.IsBusinessSuper)//商户超过
            {
                return string.Format(" 1=1 and BusinessGuid='{0}'", mangeRole.BusinessGuid);
            }//门店
            else
            {
                if (string.IsNullOrEmpty(mangeRole.ManageRole))
                    mangeRole.ManageRole = Guid.Empty.ToString();
                return string.Format(" 1=1 and Guid in ('{0}')", mangeRole.ManageRole);
            }

        }

        public static IEnumerable<SelectListItem> GetSelectRole(string where = "")
        {
            List<SelectListItem> result = new List<SelectListItem>();
            if (string.IsNullOrEmpty(where))
            {
                where = GetManageRoleWhere();
            }
            dbContext db = new dbContext();
            string table = "(select r.Guid,r.OperatorGuid,r.BusinessGuid,r.Title,r.ChainStoreGuid from TManagerRole r) Tab";

            DalGeneric<DataSet> dal = new DalGeneric<DataSet>();
            int total = 0;
            DataSet ds = dal.GetPaged(1, int.MaxValue, table, "guid,title", where, "", out total);
            //DataTable t = ds.Tables[0];

            foreach (DataRow item in ds.Tables[0].Select())
            {
                LogLogic.WriteErrorLog(item["title"].ToString());
                result.Add(new SelectListItem() { Text = item["title"].ToString(), Value = item["Guid"].ToString() });
            }

            return result;
        }
        public static List<TManagerRole> GetManageRole()
        {
            DalGeneric<TManagerRole> dal = new DalGeneric<TManagerRole>();

            dbContext db = new dbContext();
            TManager manager = new TManager();
            manager = ManagerLogic.GetLoginInfo();
            TManagerRole role = RoleLogic.GetEntity(manager.ManagerRoleGuid);
            string where = string.Format(" 1=1 and OperatorGuid='{0}'", manager.OperatorGuid);

            if (role.IsSystem)//运营商/超管/总部管理员
            {
                return db.TManagerRole.Where(c => c.OperatorGuid == role.OperatorGuid).ToList();
            }

            if (role.IsBusinessSuper)//商户最高管理员
            {

                return db.TManagerRole.Where(c => c.BusinessGuid == role.BusinessGuid).ToList();
            }
            else//按可管理角色
            {
                if (string.IsNullOrEmpty(role.ManageRole))
                {
                    return new List<TManagerRole>();
                }
                else { }
                var list = from c in db.TManagerRole
                           where new string[] { role.ManageRole.ToString() }.Contains(c.Guid.ToString())
                           select c;
                return list.ToList();
            }
        }


        public bool Add(TManagerRole role)
        {
            DalGeneric<TManagerRole> RoleDal = new DalGeneric<TManagerRole>();

            if (role.Guid == Guid.Empty)
            {
                role.Guid = Guid.NewGuid();
                role.AddTime = DateTime.Now;
                return RoleDal.Add(role) > 0;
            }
            else
            {
                return RoleDal.UpdateEntity(role);
            }

        }
        public string CheckBoxSelect(string MenuName, string RoleActionJson)
        {
            string actionAction = "";
            if (!string.IsNullOrEmpty(RoleActionJson))
            {
                //1.HashTable大数据量插入数据时需要花费比Dictionary大的多的时间。
                //2.for方式遍历HashTable和Dictionary速度最快。
                //3.在foreach方式遍历时Dictionary遍历速度更快。
                //4.在单线程的时候使用Dictionary更好一些，多线程的时候使用HashTable更好。
                //5.使用foreach，Dictionary是按照Add的顺序排列的，Hashtable则是无序的。
                List<Power> PowerList = (List<Power>)JsonHelper.JsonToObject(RoleActionJson, new List<Power>());//每次都序列化，是否可以优化
                foreach (Power item in PowerList)
                {
                    if (item.Name.Equals(MenuName))
                    {
                        actionAction = item.ActionString;
                    }
                }
            }
            return actionAction;
        }

        /// <summary>
        /// 获取角色具有的操作的权限
        /// </summary>
        /// <param name="MenuGuid">菜单Name</param>
        /// <param name="RoleGuid">角色Guid</param>
        /// <returns></returns>
        public static string GetPowerStr(string MenuName, Guid RoleGuid)
        {
            string ActionString = string.Empty;
            TManagerRole role = new dbContext().TManagerRole.Where(c => c.Guid == RoleGuid).FirstOrDefault();
            if (role.IsSystem)
            {
                ActionString += MenuName + "," + MenuLogic.GetChildNodes(MenuName);
                return ActionString;
            }
            if (role == null || string.IsNullOrEmpty(role.Action))
            {
                return "";

            }
            List<Power> PowerList = new List<Power>();
            PowerList = (List<Power>)JsonHelper.JsonToObject(role.Action, new List<Power>());
            Power power = PowerList.Where(c => c.Name == MenuName).FirstOrDefault();
            if (power == null)
            {
                return "";
            }
            else { return power.ActionString; }
        }
        /// <summary>
        /// 判断某个角色是否具有某个功能或菜单的权限
        /// </summary>
        /// <param name="RoleGuid">角色Guid</param>
        /// <param name="Name">菜单或功能的Name(Menu.Xml中的Name)</param>
        /// <param name="Url"></param>
        /// <returns>bool</returns>
        public static bool CheckPower(Guid RoleGuid, string Name, string Url = null)
        {
            string ActionString = string.Empty;
            TManagerRole role = RoleLogic.GetEntity(RoleGuid);
            if (role.IsSystem)
            {
                return true;
            }
            List<Power> PowerList = (List<Power>)JsonHelper.JsonToObject(role.Action, new List<Power>());
            Power power = PowerList.Where(c => c.Name == Name).FirstOrDefault();
            if (power == null)
            {
                return false;
            }
            if (power.ActionString.Contains(Name))
            {
                return true;
            }
            else { return false; }
        }

        public static bool Delete(string Guid, out string msg)
        {
            dbContext db = new dbContext();
            Guid gu = new System.Guid(Guid);
            TManagerRole model = db.TManagerRole.Where(c => c.Guid == gu).FirstOrDefault();
            int count = db.TManager.Where(c => c.ManagerRoleGuid == gu).Count();
            if (count > 0)
            {
                msg = "该角色下包含" + count + "个管理员账号，无法删除!";
                return false;
            }

            if (db.SaveChanges() > 0)
            { msg = "删除成功!"; return true; }
            else
            { msg = "删除失败!"; return false; }
        }
    }
}
