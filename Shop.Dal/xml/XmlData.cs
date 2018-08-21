using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Reflection;
namespace Shop.Dal.xml
{
    public class XmlData
    {
        /// <summary>
        ///  菜单Xml
        /// </summary>
        public XmlDocument MenuXml
        {
            get
            {
                XmlDocument permitionXml = new XmlDocument();
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                XmlReader reader = XmlReader.Create(Assembly.GetExecutingAssembly().GetManifestResourceStream("Shop.Dal.xmlData.Menu.xml"), settings);
                permitionXml.Load(reader);
                return permitionXml;
            }
        }
        public XmlDocument ProvincesXml
        {
            get
            {
                XmlDocument permitionXml = new XmlDocument();
                permitionXml.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("Shop.Dal.xmlData.address.Provinces.xml"));
                return permitionXml;
            }

        }

        public XmlDocument CitiesXml
        {
            get
            {
                XmlDocument permitionXml = new XmlDocument();
                permitionXml.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("Shop.Dal.xmlData.address.Cities.xml"));
                return permitionXml;
            }

        }

        public XmlDocument DistrictsXml
        {
            get
            {
                XmlDocument permitionXml = new XmlDocument();
                permitionXml.Load(Assembly.GetExecutingAssembly().GetManifestResourceStream("Shop.Dal.xmlData.address.Districts.xml"));
                return permitionXml;
            }

        }
    }
}
