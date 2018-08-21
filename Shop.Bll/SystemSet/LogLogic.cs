using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Help;
using Shop.Entity;
using System.Web;
using Shop.Dal;
using System.Data;
using System.IO;

namespace Shop.Bll
{
    public class LogLogic
    {
        #region 写入操作日志
        public static void WriteLog(string content)
        {
            TManager admin = ManagerLogic.GetLoginInfo();
            TLog log = new TLog();
            log.OperatorGuid = admin.OperatorGuid;
            log.Guid = Guid.NewGuid();
            log.Account = admin.LoginName;
            log.BusinessGuid = admin.BusinessGuid;
            log.ChainStoreGuid = admin.ChainStoreGuid;
            log.Content = content;
            log.OperatorTime = DateTime.Now;
            log.IP = Server.GetManagerIp();

            DalGeneric<TLog> dal = new DalGeneric<TLog>();
            dal.Add(log);
        }
        #endregion


        #region 系统异常日志 lws
        public static void WriteErrorLog(string logMsg)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            if (baseDirectory[baseDirectory.Length - 1] == '\\')
            {
                baseDirectory = baseDirectory.Remove(baseDirectory.Length - 1);
            }
            //string path = string.Format("{0}\\Log\\{1}", baseDirectory, Environment.MachineName);
            string path = string.Format("/Log/");
            string directoryPath = HttpContext.Current.Server.MapPath(path);//路径转换
            if (!Directory.Exists(directoryPath))//不存在这个文件夹就创建这个文件夹  
            {
                Directory.CreateDirectory(directoryPath);
            }
            string filepath = directoryPath + string.Format("\\{0}.txt", DateTime.Now.ToString("yyyy-MM-dd"));
            FileStream fs = new FileStream(@filepath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.ReadWrite);
            StreamWriter m_streamWriter = new StreamWriter(fs);
            m_streamWriter.BaseStream.Seek(0, SeekOrigin.End);
            m_streamWriter.WriteLine(string.Format("{0}************ /n/n{1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"), logMsg));
            m_streamWriter.Flush();
            m_streamWriter.Close();
            fs.Close();
        }
        #endregion
    }
}
