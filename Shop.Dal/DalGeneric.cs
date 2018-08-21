using Shop.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Data;
//using System.Data.Entity;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Configuration;

using System.Xml;

using System.Collections;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Web;

namespace Shop.Dal
{
    /// <summary>
    /// 数据操作,泛型类
    /// </summary>
    /// <typeparam name="T">要操作的的表对应的实体类的类型</typeparam>
    /// 

    /*
     * where（泛型类型约束）
       定义：在定义泛型的时候，我们可以使用 where 限制参数的范围。
       使用：在使用泛型的时候，你必须尊守 where 限制参数的范围，否则编译不会通过。

        class DalGeneric<T, U>
        where T : class///约束T参数必须为“引用 类型{ }”
        where U : struct///约束U参数必须为“值 类型”
    { }
     */
    public class DalGeneric<T> where T : class, new()
    {

        dbContext db = new dbContext();

        #region 1.0 新增实体  +int Add(T model)
        public int Add(T model)
        {

            db.Set<T>().Add(model);
            return db.SaveChanges();//保存成功后，会将自增的id设置给 model的 主键属性，并返回受影响行数 
        }
        #endregion

        #region 2.0 根据id 删除实体  +int Del(T model)
        public int Del(T model)
        {
            db.Set<T>().Attach(model); db.Set<T>().Remove(model); return db.SaveChanges();
        }
        #endregion

        #region 3.0 根据条件删除 +int DelBy(Expression<Func<T, bool>> delWhere)
        public int DelBy(Expression<Func<T, bool>> delWhere)
        {   //3.1查询要删除的数据
            List<T> listDeleting = db.Set<T>().Where(delWhere).ToList();
            //3.2将要删除的数据 用删除方法添加到 EF 容器中 
            listDeleting.ForEach(u =>
            {
                db.Set<T>().Attach(u);//先附加到 EF容器 
                db.Set<T>().Remove(u);//标识为 删除 状态 
            });
            //3.3一次性 生成sql语句到数据库执行删除 
            return db.SaveChanges();
        }
        #endregion

        #region 4.0 修改 +int Modify(T model, params string[] proNames)

        /// <summary>
        /// 4.0 修改，如：
        /// T u = new T() { uId = 1, uLoginName = "asdfasdf" };
        /// this.Modify(u, "uLoginName");
        /// </summary>
        /// <param name="model"></param>
        /// <param name="proNames"></param>
        /// <returns></returns>
        public int Modify(T model, params string[] proNames)
        {
            //4.1将 对象 添加到 EF中
            DbEntityEntry entry = db.Entry<T>(model);
            //4.2先设置 对象的包装 状态为 Unchanged
            entry.State = System.Data.Entity.EntityState.Modified;

            //4.3循环 被修改的属性名 数组
            foreach (string proName in proNames)
            {
                //4.4将每个 被修改的属性的状态 设置为已修改状态;后面生成update语句时，就只为已修改的属性 更新 
                entry.Property(proName).IsModified = true;
            }
            //4.5一次性 生成sql语句到数据库执行
            return db.SaveChanges();
        }
        #endregion

        #region 4.x 批量修改 +int Modify(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedProNames)
        /// <summary>
        /// 4.0 批量修改
        /// </summary>
        /// <param name="model">要修改的实体对象</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="modifiedProNames">要修改的 属性 名称</param>
        /// <returns></returns>
        public int ModifyBy(T model, Expression<Func<T, bool>> whereLambda, params string[] modifiedProNames)
        {
            //4.1查询要修改的数据 
            List<T> listModifing = db.Set<T>().Where(whereLambda).ToList();
            //获取 实体类 类型对象 
            Type t = typeof(T); // model.GetType();
            //获取 实体类 所有的 公有属性 
            List<PropertyInfo> proInfos = t.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            //创建 实体属性 字典集合 
            Dictionary<string, PropertyInfo> dictPros = new Dictionary<string, PropertyInfo>();
            //将 实体属性 中要修改的属性名 添加到 字典集合中 键：属性名 值：属性对象
            proInfos.ForEach(p => { if (modifiedProNames.Contains(p.Name)) { dictPros.Add(p.Name, p); } });
            //4.3循环 要修改的属性名
            foreach (string proName in modifiedProNames)
            {
                //判断 要修改的属性名是否在 实体类的属性集合中存在
                if (dictPros.ContainsKey(proName))
                {
                    //如果存在，则取出要修改的 属性对象 
                    PropertyInfo proInfo = dictPros[proName];
                    //取出 要修改的值 
                    object newValue = proInfo.GetValue(model, null); //object newValue = model.uName;
                    //4.4批量设置 要修改 对象的 属性
                    foreach (T usrO in listModifing)
                    {
                        //为 要修改的对象 的 要修改的属性 设置新的值
                        proInfo.SetValue(usrO, newValue, null); //usrO.uName = newValue;
                    }
                }
            }
            //4.4一次性 生成sql语句到数据库执行 
            return db.SaveChanges();
        }
        #endregion

        #region 5.0 根据条件查询 +List<T> GetListBy(Expression<Func<T,bool>> whereLambda)
        /// <summary>
        /// 根据条件查询
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public List<T> GetListBy(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).ToList();
        }
        #endregion

        #region 5.1 根据条件 排序 和查询 + List<T> GetListBy<TKey>
        /// <summary>
        /// 根据条件排序和查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <returns></returns>
        public List<T> GetListBy<TKey>(Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderLambda)
        {
            return db.Set<T>().Where(whereLambda).OrderBy(orderLambda).ToList();
        }
        #endregion

        #region 5.2 根据条件查询一个实体 +T GetBy(Expression<Func<T, bool>> whereLambda)
        /// <summary>
        /// 获取一个实体
        /// 注:如果提示ObjectStateManager 中已存在具有同一键的对象,请使用GetByNoTracking.
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T GetBy(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).FirstOrDefault();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        public T GetByNoTracking(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().AsNoTracking().Where(whereLambda).FirstOrDefault();
        }
        #endregion

        #region 6.0 分页查询 + List<T> GetPagedList<TKey>
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="whereLambda"></param>
        /// <param name="orderBy"></param>
        /// <returns></returns>
        public List<T> GetPagedList<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda, Expression<Func<T, TKey>> orderBy)
        {
            // 分页 一定注意： Skip 之前一定要 OrderBy 
            return db.Set<T>().Where(whereLambda).OrderBy(orderBy).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
        }
        #endregion

        #region 7.0 更新实体所有字段(整体更新)  +bool UpdateEntity(T model)
        //实现对数据库的修改功能
        public bool UpdateEntity(T entity)
        {
            db.Set<T>().Attach(entity);
            db.Entry<T>(entity).State = System.Data.Entity.EntityState.Modified;
            if (db.GetValidationErrors().Count()==0)
            {
            
                return db.SaveChanges() > 0;
            }
            else
            {
                string msg = db.GetValidationErrors().FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                WriteLog(msg);
                return false;
            }
           
        }
        #endregion

        public  DataSet  GetPaged(int start, int limit, string table, string fields, string where, string orderBy, out int total)
        {
            total = 0;
            start = (start - 1) * limit + 1;
            DataSet ds = new DataSet();
            string ConnectionString = ConfigurationManager.ConnectionStrings["SqlContent"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                SqlCommand comm = new SqlCommand("GetPaged", conn);
                comm.CommandTimeout = 60;
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@PageIndex", start / limit);
                comm.Parameters.AddWithValue("@PageSize", limit);
                comm.Parameters.AddWithValue("@Table", table);
                comm.Parameters.AddWithValue("@Field", fields);
                comm.Parameters.AddWithValue("@OrderBy", orderBy);
                comm.Parameters.AddWithValue("@Where", where);
                comm.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
                comm.Parameters["@TotalCount"].Direction = ParameterDirection.Output;
                comm.Parameters.Add("@Descript", SqlDbType.VarChar, 100);
                comm.Parameters["@Descript"].Direction = ParameterDirection.Output;
                SqlDataAdapter sda = new SqlDataAdapter(comm);
                sda.Fill(ds);
                if (comm.Parameters["@Descript"].Value.ToString() != "successful")
                {
                    throw new Exception(comm.Parameters["@Descript"].Value.ToString());
                }
                int.TryParse(comm.Parameters["@TotalCount"].Value.ToString(), out total);
            }
            return ds;
        }


        #region 系统日志 lws
        private static void WriteLog(string logMsg)
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

