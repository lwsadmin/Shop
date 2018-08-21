using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shop.Entity;
using Shop.ViewModel;
using System.Data.Entity;
using Shop.Dal;
namespace Shop.Bll
{
    public class GoodsPropertyLogic
    {
        public TGoodsProperty GetModel(Guid guid)
        {
            dbContext db = new dbContext();
            return db.TGoodsProperty.Where(c => c.Guid == guid).FirstOrDefault();
        }

        private string GetPropertyValueName(Guid GoodsCategoryGuid)
        {
            string valueName = string.Empty, par;
            DalGeneric<TGoodsProperty> dal = new DalGeneric<TGoodsProperty>();
            dbContext db = new dbContext();
            string valueNameStr = (string)db.Database.SqlQuery<string>("select ISNULL( ','+p.ValueName,'') from dbo.TGoodsProperty	  p  where GoodsCategoryGuid='" + GoodsCategoryGuid + "' for xml path('') ").FirstOrDefault();
            for (int i = 1; i <= 10; i++)
            {
                par = string.Format("Parameter{0}", i);
                if (string.IsNullOrEmpty(valueNameStr) || !valueNameStr.Contains(par))
                {
                    valueName = par;
                    break;
                }
            }
            return valueName;
        }
        public bool Add(TGoodsProperty attr, out string msg)
        {
            if (attr == null)
            {
                msg = "错误!没有检测到信息!";
                return false;
            }
            dbContext db = new dbContext();
            if (attr.Guid == Guid.Empty)
            {
                int count = db.TGoodsProperty.Where(c => c.GoodsCategoryGuid == attr.GoodsCategoryGuid).Count();
                if (count >= 10)
                {
                    msg = "自定义属性最多只能添加10个!";
                    return false;
                }
                if (db.TGoodsProperty.Where(c => c.GoodsCategoryGuid == attr.GoodsCategoryGuid && c.Name == attr.Name).Count() > 0)
                {
                    msg = "该商品分类下已经包含名为【" + attr.Name + "】的属性，请勿重复添加!";
                    return false;

                }
                attr.Guid = Guid.NewGuid();
                attr.ValueName = GetPropertyValueName(attr.GoodsCategoryGuid);
                db.TGoodsProperty.Add(attr);
                if (db.GetValidationErrors().Count() == 0)
                {
                    db.SaveChanges();
                    msg = "添加成功!";
                    return true;
                }
                else
                {
                    msg = db.GetValidationErrors().FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                    return false;
                }
            }
            else
            {
                TGoodsProperty oldAttr = db.TGoodsProperty.Where(c => c.Guid == attr.Guid).FirstOrDefault();
                if (oldAttr == null)
                {
                    msg = "该属性不存在,可能已经被删除!";
                    return false;
                }
                oldAttr.Name = attr.Name;
                oldAttr.DefaultValue = attr.DefaultValue;
                oldAttr.Remark = attr.Remark;
                oldAttr.DataType = attr.DataType;
                oldAttr.GoodsCategoryGuid = attr.GoodsCategoryGuid;
                if (oldAttr.GoodsCategoryGuid != attr.GoodsCategoryGuid)
                {
                    int count = db.TGoodsProperty.Where(c => c.GoodsCategoryGuid == attr.GoodsCategoryGuid).Count();
                    if (count >= 10)
                    {
                        msg = "该商品分类已经包含10个自定义属性上限，不能修改到该商品分类!";
                        return false;
                    }
                }
                attr.ValueName = GetPropertyValueName(attr.GoodsCategoryGuid);
                oldAttr.Options = attr.Options;
                if (db.GetValidationErrors().Count() == 0)
                {
                    db.SaveChanges();
                    msg = "修改成功!";
                    return true;
                }
                else
                {
                    msg = db.GetValidationErrors().FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage;
                    return false;
                }
            }
        }


        public string GetGoodsAttr(Guid cateGoryGuid)
        {
            dbContext db = new dbContext();


            List<TGoodsProperty> PropertyList = db.TGoodsProperty.Where(c => c.GoodsCategoryGuid == cateGoryGuid).ToList();
            // Type t = msg.GetType();
            StringBuilder str = new StringBuilder();
            for (int i = 0; i < PropertyList.Count(); i++)
            {

                switch (PropertyList[i].DataType.ToString().ToLower())
                {
                    case "text":
                        str.Append(string.Format(@"                   
                    <div class='row'>
                        <div class='col-sm-8 col-md-offset-1'>
                            <div class='form-group'>
                                <label class='control-label col-sm-2'>{0}
                                </label>
                                <div class='col-sm-4'>
                                    <input type='text' class='form-control input-sm' name='{1}' id='{1}'  value='{2}'/>
                                </div>
                                <span  class='help-block'>请输入{0}</span>
                            </div>
                        </div>
                    </div>
                     ", PropertyList[i].Name, PropertyList[i].ValueName, PropertyList[i].DefaultValue));
                        break;

                    case "textarea":
                        str.Append(string.Format(@"              
                   <div class='row'>
                        <div class='col-sm-8 col-md-offset-1'>
                            <div class='form-group'>
                                <label class='control-label col-sm-2'>{0}</label>
                                <div class='col-sm-4'>
                                  <textarea  class='form-control input-sm' rows='3' cols='30' name='{1}' id='{1}'>{2}</textarea>
                                </div>
                            </div>
                        </div>
                    </div>", PropertyList[i].Name, PropertyList[i].ValueName, PropertyList[i].DefaultValue));
                        break;

                    case "option":
                        str.Append(string.Format(@"
             <div class='row'>
                 <div class='col-sm-8 col-md-offset-1'>
                     <div class='form-group'>
                                <label class='control-label col-sm-2'>{0}</label>
                                <div class='col-sm-4'>
                          <select class='form-control input-sm' id='{1}' name='{1}'>", PropertyList[i].Name, PropertyList[i].ValueName));
                        string[] arr = PropertyList[i].Options.Split('-');
                        foreach (var pro in arr)
                        {
                            string modelParameterValue = PropertyList[i].DefaultValue;
                            if (pro == modelParameterValue)
                            {
                                str.Append(string.Format("<option selected='selected' value='" + pro + "'>" + pro + "</option>"));
                            }
                            else
                            {
                                str.Append(string.Format("<option value='" + pro + "'>" + pro + "</option>"));
                            }
                        }
                        str.Append(string.Format(@"</select>
                    </div>
                </div>
              </div>
           </div>"));
                        break;
                    default:
                        break;
                }
            }

            return str.ToString();
        }

        public bool Delete(Guid guid, out string msg)
        {
            dbContext db = new dbContext();
            TGoodsProperty model = db.TGoodsProperty.Where(c => c.Guid == guid).FirstOrDefault();
            if (model == null)
            {
                msg = "该属性不存在,可能已经被删除!";
                return false;
            }

            db.TGoodsProperty.Remove(model);
            db.SaveChanges();
            msg = "删除成功!";
            return true;

        }
    }
}
