using System;
using System.Xml.Linq;
using System.IO;
using System.Linq;

namespace GES.HMI.Module.Model
{
    public static class XMLConfig
    {
        public static string GetValue(string filename, string key, string variable)
        {
            string filePath = Path.Combine("ConfigFile", filename);

            if (File.Exists(filePath))
            {
                var doc = XDocument.Load(filePath);
                var list = from appNode in doc.Descendants("appSettings").Elements()
                           where appNode.Attribute("key")?.Value == key
                           select appNode;

                var element = list.FirstOrDefault();

                if (element != null && element.Attribute(variable) != null)
                {
                    return element.Attribute(variable).Value;
                }
            }
            return null;
        }

        public static void WriteValue(string filename, string key, string variable, string value)
        {
            string filePath = Path.Combine("ConfigFile", filename);

            if (File.Exists(filePath))
            {
                var doc = XDocument.Load(filePath);
                var list = from appNode in doc.Descendants("appSettings").Elements()
                           where appNode.Attribute("key")?.Value == key
                           select appNode;
                var element = list.FirstOrDefault();

                if (element != null && element.Attribute(variable) != null)
                {
                    element.Attribute(variable).Value = value;
                    doc.Save(filePath);
                }
            }
        }
    }
}
