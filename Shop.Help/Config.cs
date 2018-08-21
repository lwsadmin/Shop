using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using System.Web;
using System.Web.Configuration;
namespace Shop.Help
{
    public class Config
    {

        #region 设置webconfig的值
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
        #endregion

        #region  通过key,获取appSetting的值
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
        #endregion


    }
}
