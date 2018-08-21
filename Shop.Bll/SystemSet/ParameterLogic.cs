using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.Help;
using Shop.Dal;
namespace Shop.Bll
{
    public class ParameterLogic
    {
        public static void SaveParameter(string key, string value, Guid operatorGuid)
        {
            dbContext db = new dbContext();
            TParameter parameter = db.TParameter.Where(c => c.OperatorGuid == operatorGuid && c.KeyName == key).FirstOrDefault();
            if (parameter == null)
            {
                parameter = new TParameter();
                parameter.Guid = Guid.NewGuid();
                parameter.KeyName = key;
                parameter.Value = value;
                parameter.OperatorGuid = operatorGuid;
                db.TParameter.Add(parameter);
                db.SaveChanges();
            }
            else
            {
                parameter.Value = value;
                db.SaveChanges();
            }

        }

        public static string GetParameter(string Key, Guid OperatorGuid)
        {
            // Parm
            dbContext db = new dbContext();
            TParameter parameter = db.TParameter.Where(c => c.OperatorGuid == OperatorGuid && c.KeyName == Key).FirstOrDefault();
            if (parameter != null)
            {
                return parameter.Value;
            }
            else { return ""; }
        }
    }
}
