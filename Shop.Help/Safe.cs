/*
 ====================本类库主要功能====================
 * 正则表达式的验证
 * session的设置,获取
 * cookies的设置,删除,读取,加密/解密
 * IP地址的获取,IP与数字的相互转换
 * 普通文本的加密/解密
 * appsettings的获取/设置
 * 文本内部HTML的清除
 */


using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
using System.Configuration;
using System.Web.Mvc;
using System.Collections;
using System.Web.Caching;
using System.IO;

namespace Shop.Help
{
    public class Safe
    {

        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckMobile(string str)
        {

            return Regex.IsMatch(str, @"1\d{10}");
        }


        /// <summary>
        /// 验证邮编
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckPostCode(string str)
        {
            return Regex.IsMatch(str, @"[1-9]\d{5}");
        }


        /// <summary>
        /// 验证IDCard  身份证号
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIdCard(string str)
        {

            return Regex.IsMatch(str, @"\d{15}(\d\d[0-9xX])?");
        }


        /// <summary>
        /// 验证网址（URL）
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckUrl(string str)
        {
            return Regex.IsMatch(str, @"[a-zA-z]+://[^\s]*");
        }

        /// <summary>
        /// 验证IP
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckIP(string str)
        {
            return Regex.IsMatch(str, @"((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)");
        }

        /// <summary>
        /// 验证邮箱
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckEmail(string str)
        {

            return Regex.IsMatch(str, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
        }

        /// <summary>
        /// 验证QQ
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckQQ(string str)
        {
            return Regex.IsMatch(str, @"[1-9]\d{4,}");
        }


        /// <summary>
        /// 验证汉字
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }

        /// <summary>
        /// 验证固定电话
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool CheckTelephone(string str)
        {
            return Regex.IsMatch(str, @"(\d{4}-|\d{3}-)?(\d{8}|\d{7})");

        }

        /// <summary>
        ///验证英语单词
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckEnglish(string input)
        {
            return Regex.IsMatch(input, @"^\w+$");
        }
        /// <summary>
        /// 检查是否Int类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckInteger(string input)
        {
            return Regex.IsMatch(input, @"^-?\d+$");
        }
        /// <summary>
        /// 检查是否为字母和数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckLetterAndNumber(string input)
        {
            return Regex.IsMatch(input, "^[A-Za-z0-9_]+$");
        }
        /// <summary>
        /// 检查是否为整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckNumber(string input)
        {
            return Regex.IsMatch(input, @"^\d+$");
        }
        /// <summary>
        /// 检查是否为非0整数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckNumberNotZero(string input)
        {
            return Regex.IsMatch(input, @"^[^0]\d*$");
        }


        public static string GetMD5(string sDataIn, string saltValue)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] byt, bytHash;
            byt = System.Text.Encoding.UTF8.GetBytes(saltValue + sDataIn);
            bytHash = md5.ComputeHash(byt);
            md5.Clear();
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("x").PadLeft(2, '0');
            }
            return sTemp;
        }
        /// <summary> 
        /// 加密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Encrypt(string Text, string Key = "wenshang", string Iv = "lwsadmin")
        {
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            int i = cryptoProvider.KeySize;
            MemoryStream ms = new MemoryStream();
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);

            StreamWriter sw = new StreamWriter(cst);
            sw.Write(Text);
            sw.Flush();
            cst.FlushFinalBlock();
            sw.Flush();
            return Convert.ToBase64String(ms.GetBuffer(), 0, (int)ms.Length);
        }
        /// <summary> 
        /// 解密数据 
        /// </summary> 
        /// <param name="Text"></param> 
        /// <param name="sKey"></param> 
        /// <returns></returns> 
        public static string Decrypt(string Text, string Key = "wenshang",string Iv="lwsadmin")
        {
            //    public static string _KEY = "HQDCKEY1";  //密钥  
            //public static string _IV = "HQDCKEY2";   //向量  
            byte[] byKey = System.Text.ASCIIEncoding.ASCII.GetBytes(Key);
            byte[] byIV = System.Text.ASCIIEncoding.ASCII.GetBytes(Iv);

            byte[] byEnc;
            try
            {
                byEnc = Convert.FromBase64String(Text);
            }
            catch
            {
                return null;
            }

            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream ms = new MemoryStream(byEnc);
            CryptoStream cst = new CryptoStream(ms, cryptoProvider.CreateDecryptor(byKey, byIV), CryptoStreamMode.Read);
            StreamReader sr = new StreamReader(cst);
            return sr.ReadToEnd();
        }
        /// <summary>
        /// 将IP地址转为整数形式
        /// </summary>
        /// <returns>整数</returns>
        public static long IpToLong(IPAddress ip)
        {
            int x = 3;
            long o = 0;
            foreach (byte f in ip.GetAddressBytes())
            {
                o += (long)f << 8 * x--;
            }
            return o;
        }
        /// <summary>
        /// 将整数转为IP地址
        /// </summary>
        /// <returns>IP地址</returns>
        public static IPAddress LongToIP(long l)
        {
            byte[] b = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                b[3 - i] = (byte)(l >> 8 * i & 255);
            }
            return new IPAddress(b);
        }
        /// <summary>
        /// 获取IP
        /// </summary>
        /// <returns></returns>
        public static string GetIp()
        {
            string result = String.Empty;
            result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (null == result || result == String.Empty)
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (null == result || result == String.Empty)
            {
                return "0.0.0.0";
            }
            return result;
        }
        public static void CreatCookie(string key, string value, int day)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Value = value;
            cookie.Expires = DateTime.Now.AddDays(day);
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }
        /// <summary>
        /// 读取Cookie
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetCookie(string key)
        {
            string value = string.Empty;
            HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                value = cookie.Value;
                return value;
            }
            else
            {
                return value;
            }
        }
        /// <summary>
        /// 删除Cookie
        /// </summary>
        /// <param name="key"></param>
        public static void DelCookie(string key)
        {
            HttpCookie cookie = new HttpCookie(key);
            cookie.Expires = DateTime.Now.AddDays(-1);
            System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
        }
        private const string key = "1i950p-e2h&wStG7seiehov5";//key的长度必须为16位或24位，否则报错“指定键的大小对于此算法无效。”
                                                              /// <summary>
                                                              /// 加密Cookie
                                                              /// </summary>
                                                              /// <param name="str"></param>
                                                              /// <returns></returns>
        public static string EncryptCookie(string str)
        {
            byte[] data = UnicodeEncoding.Unicode.GetBytes(str);//如果加密中文，不能用ASCII码
            byte[] keys = ASCIIEncoding.ASCII.GetBytes(key);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = keys;//key的长度必须为16位或24位，否则报错“指定键的大小对于此算法无效。”，des.Key不支持中文
                           //des.Mode = CipherMode.ECB;设置运算模式
            des.Mode = CipherMode.ECB;
            ICryptoTransform cryp = des.CreateEncryptor();//加密

            return Convert.ToBase64String(cryp.TransformFinalBlock(data, 0, data.Length));
        }
        /// <summary>
        /// 解密Cookie
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string DecryptCookie(string str)
        {
            byte[] data = Convert.FromBase64String(str);
            byte[] keys = ASCIIEncoding.ASCII.GetBytes(key);
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            des.Key = keys;
            des.Mode = CipherMode.ECB;//设置运算模式
            des.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryp = des.CreateDecryptor();//解密
            return UnicodeEncoding.Unicode.GetString(cryp.TransformFinalBlock(data, 0, data.Length));
        }
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="strSessionName"></param>
        /// <param name="strValue"></param>
        /// <param name="iExpires">分钟为单位</param>
        public static void SessionAdd(string strSessionName, string strValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }
        /// <summary>
        /// 设置Session
        /// </summary>
        /// <param name="strSessionName"></param>
        /// <param name="strValue">Object</param>
        /// <param name="iExpires">分钟为单位</param>
        public static void SessionAdd(string strSessionName, Object strValue, int iExpires)
        {
            HttpContext.Current.Session[strSessionName] = strValue;
            HttpContext.Current.Session.Timeout = iExpires;
        }




        /// <summary>
        /// 读取Session
        /// </summary>
        /// <param name="strSessionName"></param>
        /// <returns></returns>
        public static Object SessionGet(string strSessionName)
        {
            if (HttpContext.Current.Session[strSessionName] == null)
            {
                return null;
            }
            else
            {
                return HttpContext.Current.Session[strSessionName];
            }
        }

        /// <summary>
        /// 设置webconfig的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetWebConfig(string key, string value)
        {
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");
            if (appSetting.Settings[key] == null)//如果不存在此节点，则添加  
            {
                appSetting.Settings.Add(key, value);
            }
            else//如果存在此节点，则修改  
            {
                appSetting.Settings[key].Value = value;
            }
            config.Save();
            config = null;
        }





        /// <summary>
        /// 通过key,获取appSetting的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>value</returns>
        public static string GetWebConfigValueByKey(string key)
        {
            string value = string.Empty;
            Configuration config = System.Web.Configuration.WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
            AppSettingsSection appSetting = (AppSettingsSection)config.GetSection("appSettings");
            if (appSetting.Settings[key] != null)//如果不存在此节点，则添加  
            {
                value = appSetting.Settings[key].Value;
            }
            config = null;
            return value;
        }
        /// <summary>
        /// 添加缓存 tips:会覆盖key相同的缓存
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">value</param>
        /// <param name="ts">过期时间段  比如10秒  20秒  等等</param>
        public static void SetCache(string key, Object value, TimeSpan ts)
        {
            HttpContext.Current.Cache.Insert(key, value, null, DateTime.UtcNow.Add(ts), Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 根据key  读取缓存信息
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static Object GetCache(string key)
        {
            return HttpContext.Current.Cache.Get(key);
        }


        /// <summary>
        /// 清空指定key的缓存
        /// </summary>
        /// <param name="cacheKey"></param>
        public static void ClearCache(string cacheKey)
        {

            List<string> keys = new List<string>();
            // retrieve application Cache enumerator
            IDictionaryEnumerator enumerator = HttpRuntime.Cache.GetEnumerator();
            // copy all keys that currently exist in Cache
            while (enumerator.MoveNext())
            {
                keys.Add(enumerator.Key.ToString());
            }
            // delete every key from cache
            for (int i = 0; i < keys.Count; i++)
            {
                if (keys[i] == cacheKey)
                {
                    HttpRuntime.Cache.Remove(keys[i]);
                }

            }
        }


        /// <summary>
        /// 清除文本中的html脚本
        /// </summary>
        /// <param name="Htmlstring">源文本</param>
        /// <returns>处理后的文本</returns>
        public static string CleanHTML(string Htmlstring)
        {

            //删除脚本 
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML 
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");

            Htmlstring.Replace(">", "");

            Htmlstring.Replace("\r\n", "");

            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring;

        }

    }
}
