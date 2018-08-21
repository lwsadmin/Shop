using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

//lws
namespace Shop.Help
{
  public  class Reflection
    {
      
        /// <summary>
        /// 判断 对象 中是否有该属性(不区分大小写)
        /// </summary>
        /// <param name="PropertyName">属性名称</param>
        /// <param name="o">目标对象</param>
        /// <returns></returns>
        public static bool JudgeHasProperty(string PropertyName, Object o)
        {
            if (o == null)
            {
                o = new { };
            }
            PropertyInfo[] p1 = o.GetType().GetProperties();
            bool b = false;
            foreach (PropertyInfo pi in p1)
            {
                if (pi.Name.ToLower() == PropertyName.ToLower())
                {
                    b = true;
                }
            }
            return b;
        }



        public static string GetDescriptionFromEnumValue(Type enumType, object enumValue)
        {
            try
            {
                object o = Enum.Parse(enumType, enumValue.ToString());

                string name = o.ToString();
                DescriptionAttribute[] customAttributes = (DescriptionAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(DescriptionAttribute), false);
                if ((customAttributes != null) && (customAttributes.Length == 1))
                {
                    return customAttributes[0].Description;
                }
                return name;
            }
            catch
            {
                return enumValue.ToString();
            }
        }
        /// <summary>
        /// 获取指定属性的值(不区分大小写)
        /// </summary>
        /// <param name="PropertyName">属性名称</param>
        /// <param name="o">目标对象</param>
        /// <returns></returns>
        public static Object GetPropertyValueByName(string PropertyName, Object o)
        {
            if (o == null)
            {
                o = new { };
            }
            //创建一个返回对象
            Object returnObject = new Object();
            PropertyInfo[] p1 = o.GetType().GetProperties();
            foreach (PropertyInfo pi in p1)
            {
                if (pi.Name.ToLower() == PropertyName.ToLower())
                {
                    returnObject = pi.GetValue(o,null);
                }
            }
            return returnObject;
        }



        /// <summary>
        /// 遍历属性的名称/值(显示形式:name=value)
        /// </summary>
        /// <param name="o"></param>
        public static void ForeachPropertyValues(Object o)
        {
            if (o == null)
            {
                o = new { };
            }
            PropertyInfo[] p1 = o.GetType().GetProperties();
            foreach (PropertyInfo pi in p1)
            {
                Console.WriteLine(pi.Name + ":" + pi.GetValue(o, null));
            }

        }

    }
}
