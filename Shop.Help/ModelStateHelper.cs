using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Web.ModelBinding;
using System.Web.Mvc;

namespace Shop.Help
{
    public class ModelStateHelper
    {
        /// <summary>
        /// 根据控制器的modelstate,查询其中的错误信息,并拼接为字符串返回
        /// </summary>
        /// <param name="modelstate"></param>
        /// <returns></returns>
        public static string GetModelStateErrmsg(ModelStateDictionary modelstate)
        {
            string str = "";
            if (!modelstate.IsValid)
            {
                List<string> Keys = modelstate.Keys.ToList();
                foreach (var key in Keys)
                {
                    var errors = modelstate[key].Errors.ToList();
                    foreach (var error in errors)
                    {

                        str += " [" + error.ErrorMessage + "] ";
                    }
                }
            }
            return str;
        }
    }
}
