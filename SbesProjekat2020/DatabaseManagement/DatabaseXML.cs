using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DatabaseManagement
{
    public class DatabaseXML : IDatabaseManagementXML
    {
        public void WriteInXml(string path, string user, string currentTime, string message)
        {

            if (File.Exists(path) == false)
            {
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
                xmlWriterSettings.Indent = true;
                xmlWriterSettings.NewLineOnAttributes = true;
                using (XmlWriter xmlWriter = XmlWriter.Create(path, xmlWriterSettings))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("Logging");
                    xmlWriter.WriteElementString("User", user);
                    xmlWriter.WriteElementString("Time", currentTime);
                    xmlWriter.WriteElementString("Info", message);
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndDocument();
                    xmlWriter.Flush();
                    xmlWriter.Close();
                }
            }
            else
            {
                XDocument xDocument = XDocument.Load(path);
                XElement root = xDocument.Element("Logging");
                IEnumerable<XElement> rows = root.Descendants();
                XElement firstRow = rows.First();
                firstRow.AddBeforeSelf(
                   new XElement("User", user),
                   new XElement("Time", currentTime),
                   new XElement("Info", message));
                xDocument.Save(path);
            }
        }
    }
}
