/**
 *zfy 2014-11-29
 * 服务器信息获取
 */
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Shop.Help
{
    public static class Server
    {
        /// <summary>
        ///获取操作系统版本
        /// </summary>
        /// <returns></returns>
        public static string GetSystemVersion()
        {
            return Environment.OSVersion.ToString();
        }

        /// <summary>
        /// 服务器计算机名称
        /// </summary>
        /// <returns></returns>
        public static string GetServerName()
        {
            return HttpContext.Current.Server.MachineName;//服务器名称
        }

        /// <summary>
        /// 服务器IP地址  
        /// </summary>
        /// <returns></returns>
        public static string GetServerIp()
        {
            return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"];
        }
        /// <summary>
        /// 获取访问用户的IP
        /// </summary>
        public static string GetManagerIp()
        {
            string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            switch (result)
            {
                case null:
                case "":
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                    break;
            }
            if (!Safe.CheckIP(result))
            {
                return "Unknown";
            }
            return result;
        }

        public static object RegularExp { get; private set; }

        /// <summary>
        /// 服务器域名  
        /// </summary>
        /// <returns></returns>
        public static string GetDomainName()
        {
            return HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
        }

        /// <summary>
        /// /.NET解释引擎版本
        /// </summary>
        /// <returns></returns>
        public static string GetDotNetVersion()
        {
            return ".NET CLR" + Environment.Version.Major + "." + Environment.Version.Minor + "." + Environment.Version.Build + "." + Environment.Version.Revision;
        }

        /// <summary>
        /// IIS版本
        /// </summary>
        /// <returns></returns>
        public static string GetServerSoftware()
        {
            return HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"];
        }

        /// <summary>
        /// HTTP访问端口  
        /// </summary>
        /// <returns></returns>
        public static string GetServerPort()
        {
            return HttpContext.Current.Request.ServerVariables["SERVER_PORT"];
        }

        /// <summary>
        /// 虚拟目录的绝对路径  
        /// </summary>
        /// <returns></returns>
        public static string GetApplRhysicalPath()
        {
            return HttpContext.Current.Request.ServerVariables["APPL_RHYSICAL_PATH"];
        }

        /// <summary>
        /// 执行文件的绝对路径  
        /// </summary>
        /// <returns></returns>
        public static string GetPathTranslated()
        {
            return HttpContext.Current.Request.ServerVariables["PATH_TRANSLATED"];
        }

        /// <summary>
        /// 虚拟目录Session总数 
        /// </summary>
        /// <returns></returns>
        public static string GetSessionCount()
        {
            return HttpContext.Current.Session.Contents.Count.ToString();
        }

        /// <summary>
        /// 虚拟目录Application总数  
        /// </summary>
        /// <returns></returns>
        public static string GetApplicationCount()
        {
            return HttpContext.Current.Application.Contents.Count.ToString();
        }

        /// <summary>
        /// 服务器区域语言 
        /// </summary>
        /// <returns></returns>
        public static string GetAcceptLanguage()
        {
            return HttpContext.Current.Request.ServerVariables["HTTP_ACCEPT_LANGUAGE"];//服务器区域语言  
        }

        /// <summary>
        /// CPU个数    
        /// </summary>
        /// <returns></returns>
        public static string GetCpuCount()
        {
            return Environment.GetEnvironmentVariable("NUMBER_OF_PROCESSORS");//CPU个数    
        }

        /// <summary>
        /// CPU类型 
        /// </summary>
        /// <returns></returns>
        public static string GetCpuIdentifier()
        {
            return Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER");//CPU个数    
        }

        #region 系统日志 lws
        public static void WriteLog(string logMsg)
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
            m_streamWriter.WriteLine(string.Format("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff"), logMsg));
            m_streamWriter.Flush();
            m_streamWriter.Close();
            fs.Close();
        }
        #endregion

    }
}
