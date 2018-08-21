using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Dal;
using Shop.Help;
using System.Web.Mvc;
using System.Web;
using Shop.Bll;
using System.IO;
using Shop.Dal.Emnu;
namespace Shop.Bll
{
    public class CommonLogic
    {
        public static bool Login(string loginAccount, string loginName, string pwd, out string msg)
        {
            msg = "";
            dbContext db = new dbContext();
            TOperator op = db.TOperator.Where(c => c.LoginAccount.ToUpper() == loginAccount.ToUpper()).FirstOrDefault();
            if (op == null)
            {
                msg = "商户号不存在!"; return false;
            }

            string EncryptPwd = Safe.Encrypt(pwd);
            TManager manger = db.TManager.Where(c => c.LoginName == loginName && c.LoginPwd == EncryptPwd && c.OperatorGuid == op.Guid).FirstOrDefault();
            if (manger != null)
            {
                TManagerRole role = db.TManagerRole.Where(c => c.Guid == manger.ManagerRoleGuid).FirstOrDefault();
                if (role.IsSystem)
                {
                    role.Action = "System";
                }
                manger.LastLoginTime = DateTime.Now;
                db.SaveChanges();
                manger.LoginPwd = "";
                string config = Config.GetWebConfigValueByKey("LoginInfo");
                Safe.CreatCookie(config, HttpUtility.UrlEncode(JsonHelper.ObjectToJson(manger)), 1);
                return true;
            }
            else { msg = "用户名和密码不匹配!"; return false; }
        }


        public static decimal GetMoneyByPoint(decimal Point, Guid OperatorGuid)
        {
            decimal PointPrice = Convert.ToDecimal(ParameterLogic.GetParameter("PointPrice", OperatorGuid));

            return Point / PointPrice;
        }
        public static bool ManagerLoginOut()
        {
            string CooikeConfig = Config.GetWebConfigValueByKey("LoginInfo");
            Safe.DelCookie(CooikeConfig);
            return true;
        }
        public static string GetWhereCondition()
        {
            TManager manager = new TManager();
            manager = ManagerLogic.GetLoginInfo();
            TManagerRole role = RoleLogic.GetEntity(manager.ManagerRoleGuid);
            string where = string.Format(" 1=1 and OperatorGuid='{0}'", manager.OperatorGuid);
            if (role.IsSystem)//运营商/超管
            {
                return where;
            }
            else
            {
                //数据查看控制到商户
                where = string.Format(" 1=1  and BusinessGuid='{0}'", manager.BusinessGuid);
            }
            return where;
        }

        public static bool SaveSingleImg(HttpPostedFileBase file, Guid opertorGuid, out string url)
        {
            if (opertorGuid == Guid.Empty)
            {
                TManager manager = ManagerLogic.GetLoginInfo();
                opertorGuid = manager.OperatorGuid;
            }
            dbContext db = new dbContext();
            TOperator opertor = db.TOperator.Single(o => o.Guid == opertorGuid);
            string path = string.Format("/Upload/{0}/Images/{1}/", opertor.LoginAccount, DateTime.Now.Year.ToString());
            string directoryPath = HttpContext.Current.Server.MapPath(path);//路径转换
            if (!Directory.Exists(directoryPath))//不存在这个文件夹就创建这个文件夹  
            {
                Directory.CreateDirectory(directoryPath);
            }
            Random r = new Random(Guid.NewGuid().GetHashCode());
            string uploadFileName = DateTime.Now.ToString("MMddhhmmss") +
                DateTime.Now.Millisecond.ToString() +
                r.Next(1000, 9999).ToString() + Path.GetExtension(file.FileName);
            // string uploadFileName = DateTime.Now.ToString("MMddhhmmss") + r.Next(100000, 999999) + ".jpeg";
            directoryPath += uploadFileName;
            file.SaveAs(directoryPath);
            // FileStream fs = new FileStream(savepath, FileMode.Create);
            url = path + uploadFileName;//返回的没有转换的相对路径到前端，前端传入后台存入数据库
            return true;
        }

        public static async Task<bool> DeleteImages(string imgPath)
        {
            string directoryPath = HttpContext.Current.Server.MapPath(imgPath);
            if (File.Exists(directoryPath))
            {
                File.Delete(directoryPath);
            }
            return true;
        }




        public static string GetBillNumber(BillTypes billTypes, string TY = "")
        {
            string num1 = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            //string num1 = DateTime.Now.ToString("yyMMdd");
            Random rd = new Random();

            string endBillNumber = num1 + rd.Next(0, 9999).ToString("D5");
            if (!string.IsNullOrEmpty(TY))
            {
                endBillNumber = TY + endBillNumber;
            }
            return endBillNumber;
        }
        /// <summary>
        ///  获取券号，验证码,消费码等 生成8位随机数
        /// </summary>
        /// <returns></returns>
        public string GetCodeNumber()
        {
            Random rd = new Random();
            return rd.Next(0, 99999999).ToString("D8");
        }

    }
}
