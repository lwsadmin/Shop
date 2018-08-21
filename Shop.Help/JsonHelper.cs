/*
 * json简单帮助工具类
 * zhifeiya
 * 2015-2-2
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web.Script.Serialization;
namespace Shop.Help
{
    public static class JsonHelper
    {
        /// <summary>
        /// 从一个对象信息生成Json串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ObjectToJson(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }

        public static string ToJson<T>(this T obj) where T : class
        {
            if (obj.GetType().GetCustomAttributes(typeof(DataContractAttribute), true).Length > 0)
            {
                DataContractJsonSerializer ser = new DataContractJsonSerializer(obj.GetType());
                string output = string.Empty;
                using (MemoryStream ms = new MemoryStream())
                {
                    ser.WriteObject(ms, obj);
                    StringBuilder sb = new StringBuilder();
                    sb.Append(Encoding.UTF8.GetString(ms.ToArray(), 0, ms.ToArray().Length));
                    output = sb.ToString();
                }
                return output;
            }
            else
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                return serializer.Serialize(obj);
            }
        }
        /// <summary>
        /// 从一个Json串生成对象信息
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object JsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString.Trim()));
            return serializer.ReadObject(mStream);
        }

        /// <summary>
        /// 序列化 对象到字符串
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <returns>序列化后的字符串</returns>
        public static string Serialize<T>(T obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Flush();
                stream.Close();
                return Convert.ToBase64String(buffer);
            }
            catch (Exception ex)
            {
                throw new Exception("序列化失败,原因:" + ex.Message);
            }
        }

        /// <summary>
        /// 反序列化 字符串到对象
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <param name="str">要转换为对象的字符串</param>
        /// <returns>反序列化出来的对象</returns>
        public static T Desrialize<T>(T obj, string str)
        {
            try
            {
                obj = default(T);
                IFormatter formatter = new BinaryFormatter();
                byte[] buffer = Convert.FromBase64String(str);
                MemoryStream stream = new MemoryStream(buffer);
                obj = (T)formatter.Deserialize(stream);
                stream.Flush();
                stream.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("反序列化失败,原因:" + ex.Message);
            }
            return obj;
        }

        //OldBegint 掌握此种即可
        /// <summary>
        /// 从一个Json串生成对象信息
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object DeJsonToObject(string jsonString, object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream mStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString.Trim()));
            return serializer.ReadObject(mStream);
        }
        /// <summary>
        /// 序列化 对象到字符串
        /// </summary>
        /// <param name="obj">泛型对象</param>
        /// <returns>序列化后的字符串</returns>
        public static string SerObjToJson<T>(T obj)
        {
            try
            {
                IFormatter formatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                formatter.Serialize(stream, obj);
                stream.Position = 0;
                byte[] buffer = new byte[stream.Length];
                stream.Read(buffer, 0, buffer.Length);
                stream.Flush();
                stream.Close();
                return Convert.ToBase64String(buffer);
            }
            catch (Exception ex)
            {
                throw new Exception("序列化失败,原因:" + ex.Message);
            }
        }

        public static Dictionary<string, object> JsonToDic(string strJson)
        {
            var json = new JavaScriptSerializer() { MaxJsonLength = int.MaxValue };
            Dictionary<string, object> dict = (Dictionary<string, object>)json.DeserializeObject(strJson);
            return dict;
        }
    }
}

