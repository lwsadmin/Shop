using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using Shop.Dal;
using Shop.Dal.xml;

namespace Shop.Bll
{
    public static class MenuLogic
    {



        public static XmlNodeList GetFrist()
        {
            XmlDocument Xml = new XmlData().MenuXml;
            XmlNodeList ProvinceNodeList = Xml.SelectNodes("//Menu//First");
            return ProvinceNodeList;
        }
        public static XmlNodeList GetSecond(string Name)
        {
            XmlDocument Xml = new XmlData().MenuXml;
            XmlNode FirstNode = Xml.SelectSingleNode("//Menu//First[@Name='" + Name + "']");
            return FirstNode.ChildNodes;
        }

        public static XmlNodeList GetThird(string Name)
        {
            XmlDocument Xml = new XmlData().MenuXml;
            XmlNode FirstNode = Xml.SelectSingleNode("//Menu//First//Second[@Name='" + Name + "']");
            return FirstNode.ChildNodes;
        }

        public static XmlNodeList GetFourth(string Name)
        {
            XmlDocument Xml = new XmlData().MenuXml;
            XmlNode FirstNode = Xml.SelectSingleNode("//Menu//First//Second/Third[@Name='" + Name + "']");
            return FirstNode.ChildNodes;
        }

        public static string GetChildNodes(string Name)
        {
            string str = "";
            XmlDocument Xml = new XmlData().MenuXml;
            XmlNode ActionNode = Xml.SelectSingleNode("//Menu//First//Second[@Name='" + Name + "']");
            if (ActionNode == null)
            {
                ActionNode = Xml.SelectSingleNode("//Menu//First//Second//Third[@Name='" + Name + "']");
            }
            foreach (XmlNode item in ActionNode.ChildNodes)
            {
                str += "," + item.Attributes["Name"].Value;
            }
            return str;
        }
    }
}
