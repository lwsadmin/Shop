using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using Shop.Dal.xml;
namespace Shop.Help
{
    public static class AddressSelect
    {

        public static string GetPro(string ProID, string NameAttr = "ProviceID")
        {
            XmlDocument Xml = new XmlData().ProvincesXml;
            StringBuilder sb = new StringBuilder();
            //string path = HttpContext.Current.Server.MapPath("/Content/xmlData/address/Provinces.xml");
            //Xml.Load(path);
            XmlNodeList ProvinceNodeList = Xml.SelectNodes("//Province");
            StringBuilder strCity = new StringBuilder();
            sb.Append("<select  class='form-control input-sm' style='width:30%;float:left;' id='pro' name='" + NameAttr + "' onchange='ProChange(this)'>>");
            foreach (XmlNode node in ProvinceNodeList)
            {
                if (node.Attributes["ID"].Value == ProID)
                {
                    sb.Append("<option selected='selected' value='" + node.Attributes["ID"].Value + "'>" + node.Attributes["ProvinceName"].Value + "</option>");
                }
                else { sb.Append("<option value='" + node.Attributes["ID"].Value + "'>" + node.Attributes["ProvinceName"].Value + "</option>"); }

            }
            sb.Append("</select>");
            return sb.ToString();
        }
        public static string GetCity(string ProID, string CityID = null, string NameAttr = "CityID")
        {
            XmlDocument Xml = new XmlData().CitiesXml;
            //string path = HttpContext.Current.Server.MapPath("/Content/xmlData/address/Cities.xml");
            //Xml.Load(path);
            XmlNodeList CityNodeList = Xml.SelectNodes("//City[@PID='" + ProID + "']");
            StringBuilder strCity = new StringBuilder();
            strCity.Append("<select style='width:30%;float:left;' class='form-control input-sm' id='city' name='" + NameAttr + "' onchange='CityChange(this)'>");
            foreach (XmlNode node in CityNodeList)
            {
                if (node.Attributes["ID"].Value == CityID)
                {
                    strCity.Append("<option selected='selected' value='" + node.Attributes["ID"].Value + "'>" + node.Attributes["CityName"].Value + "</option>");
                }
                else
                {
                    strCity.Append("<option value='" + node.Attributes["ID"].Value + "'>" + node.Attributes["CityName"].Value + "</option>");
                }

            }
            strCity.Append("/<select>");
            return strCity.ToString();
        }

        public static string GetDis(string CityID, string DisID = null, string NameAttr = "DistrictID")
        {
            XmlDocument Xml = new XmlData().DistrictsXml;
            //string path = HttpContext.Current.Server.MapPath("/Content/xmlData/address/Districts.xml");
            //Xml.Load(path);
            XmlNodeList DisNodeList = Xml.SelectNodes("//District[@PID='" + CityID + "']");
            StringBuilder strCity = new StringBuilder();
            strCity.Append("<select style='width:30%;float:left;' class='form-control input-sm' id='dis' name='" + NameAttr + "'>");
            foreach (XmlNode node in DisNodeList)
            {
                if (node.Attributes["ID"].Value == DisID)
                {
                    strCity.Append("<option selected='selected' value='" + node.Attributes["ID"].Value + "'>" + node.Attributes["DistrictName"].Value + "</option>");

                }
                else { strCity.Append("<option value='" + node.Attributes["ID"].Value + "'>" + node.Attributes["DistrictName"].Value + "</option>"); }

            }
            strCity.Append("/<select>");
            return strCity.ToString();
        }

        public static MvcHtmlString GetAddress<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> ProvinceID, Expression<Func<TModel, TProperty>> CityID, Expression<Func<TModel, TProperty>> DistrictID, Object HtmlAttribute)
        {
            StringBuilder sb = new StringBuilder();
            string ProName = string.Empty; string CityName = string.Empty; string DisName = string.Empty;
            string ProValue = GetLamdaValue<TModel, TProperty>(htmlHelper, ProvinceID, out ProName);
            string CityValue = GetLamdaValue<TModel, TProperty>(htmlHelper, CityID, out CityName);
            string DisValue = GetLamdaValue<TModel, TProperty>(htmlHelper, DistrictID, out DisName);
            sb.Append("<script src = \"/Content/LwsJS/address.js\" ></script > ");
            sb.Append(GetPro(ProValue, ProName));
            sb.Append(GetCity(ProValue, CityValue, CityName));
            sb.Append(GetDis(CityValue, DisValue, DisName));
            return new MvcHtmlString(sb.ToString());
        }

        public static string GetLamdaValue<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> ID, out string name)
        {
            string lambda = ID.Body.ToString();//形如Model.ProviceID
            name = "";
            string value = string.Empty;
            if (htmlHelper.ViewData.Model != null)
            {
                name = ModelMetadata.FromLambdaExpression(ID, htmlHelper.ViewData).PropertyName.ToString();
                value = ModelMetadata.FromLambdaExpression(ID, htmlHelper.ViewData).Model.ToString();

            }

            return value;
        }
    }
}
