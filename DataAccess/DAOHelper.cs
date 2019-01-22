using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace BookStore.DataAccess
{
    public class DAOHelper<T>
    {
        private string rootName;
        private string fileName;
        private string keyProperty;

        public DAOHelper(string rootName, string keyProperty)
        {
            this.rootName = rootName;
            this.fileName = rootName + ".xml";
            this.keyProperty = keyProperty;
        }

        private String dataFolder
        {
            get
            {
                String dataSourceFolder = ConfigurationManager.AppSettings.Get("DataSourceFolder");

                return AppDomain.CurrentDomain.BaseDirectory + "\\" + dataSourceFolder;
            }
        }

        private string dataFilePath
        {
            get
            {
                return this.dataFolder + "//" + this.fileName;
            }
        }

        public List<T> GetAll(string dataFilePath)
        {
            XmlSerializer desSerializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(this.rootName));
            StreamReader xmlReader = new StreamReader(dataFilePath);
            List<T> items = (List<T>)desSerializer.Deserialize(xmlReader);

            xmlReader.Close();

            return items;
        }

        public void Save(object item)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.dataFilePath);

            String keyValue = item.GetType().GetProperty(this.keyProperty).GetValue(item, null).ToString();
            string xmlQuery = string.Format("{0}/{1}[@{2}=\"{3}\"]", this.rootName, item.GetType().Name, this.keyProperty, keyValue);
            XmlNode itemNode = doc.SelectSingleNode(xmlQuery);
            if (itemNode != null)
                doc.GetElementsByTagName(this.rootName)[0].RemoveChild(itemNode);

            XmlElement itemElement = doc.CreateElement(item.GetType().Name);
            XmlElement tmpElement = SerializeToXmlElement(item);
            foreach (XmlAttribute xmlAttribute in tmpElement.Attributes)
            {
                XmlAttribute xa = doc.CreateAttribute(xmlAttribute.Name);
                xa.Value = xmlAttribute.Value;
                itemElement.Attributes.Append(xa);
            }
            itemElement.InnerXml = tmpElement.InnerXml;

            XmlElement itemsElement = (XmlElement) doc.GetElementsByTagName(this.rootName).Item(0);
            itemsElement.AppendChild(itemElement);

            doc.Save(this.dataFilePath);
        }

        public List<T> GetAll()
        {
            XmlSerializer desSerializer = new XmlSerializer(typeof(List<T>), new XmlRootAttribute(this.rootName));
            StreamReader xmlReader = new StreamReader(dataFilePath);
            List<T> items = (List<T>)desSerializer.Deserialize(xmlReader);

            xmlReader.Close();

            return items;
        }

        private XmlElement SerializeToXmlElement(object item)
        {
            XmlDocument doc = new XmlDocument();

            using (XmlWriter writer = doc.CreateNavigator().AppendChild())
            {
                XmlSerializer serializer = new XmlSerializer(item.GetType());
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");
                serializer.Serialize(writer, item, ns);
            }

            return doc.DocumentElement;
        }
    }
}
