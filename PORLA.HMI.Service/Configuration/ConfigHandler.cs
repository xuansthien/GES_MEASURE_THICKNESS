using log4net.Core;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PORLA.HMI.Service.Configuration
{
    public class ConfigHandler : IConfigHandler
    {
        private static log4net.ILog logger =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public string xmlPath;

        /// <summary>
        /// Keeps the entire content of xml file.
        /// </summary>
        private XmlDocument xmlDocument = null;
        XmlElement newHeaderElement = null;
        XmlNodeList listNode = null;
        XmlNode targetNode = null;
        XmlNode childNode = null;
        /// <summary>
        /// Consturctor with xml file path as parameter
        /// </summary>
        /// <param name="xmlPath"></param>

        public ConfigHandler()
        {
            //string xmlPath = ConfigValue.AppSetting.ConfigPath;
            //LoadXML(xmlPath);
            //this.xmlPath = xmlPath;
        }

        /// <summary>
        /// Loads the xml file from given location.
        /// If file is not found in given location, then it load contents from  
        /// a resource file with default values.  
        /// </summary>
        /// <returns>Success status</returns>
        public bool LoadXML(string xmlPath)
        {
            this.xmlPath = xmlPath;
            try
            {
                if (File.Exists(this.xmlPath))
                {
                    xmlDocument = new XmlDocument();
                    
                    using (StreamReader sr = new StreamReader(this.xmlPath))
                    {
                        xmlDocument.Load(sr);
                        return true;
                    }
                }

            }
            catch (XmlException ex)
            {
                logger.Error($"Unable to load xml : {ex}");
                return false;
            }
            catch (Exception ex)
            {
                logger.Error("Exception", ex);
                return false;
            }

            return false;
        }

        /// <summary>
        /// Validates node.
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="actualNodePath">Actual node path</param>
        /// <returns>Error code</returns>
        private string CheckNodeIsValid(string name, out string actualNodePath)
        {

            actualNodePath = string.Empty;
            if (string.IsNullOrEmpty(name))
            {
                return "Node is empty";
            }

            string[] segments = name.Split('.');

            StringBuilder sb = new StringBuilder("/");

            for (int i = 0; i < segments.Length; i++)
            {
                sb.Append(string.Format("/{0}", segments[i]));
                if (xmlDocument.SelectSingleNode(sb.ToString()) == null)
                {
                    return string.Format("Node was not found : {0}", sb.ToString());
                }
            }

            actualNodePath = sb.ToString();
            return string.Empty;
        }

        /// <summary>
        /// Converts string to specified data type.
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="str">string</param>
        /// <param name="value">converted value</param>
        /// <returns>Success status</returns>
        private bool Tryparse<T>(string str, out T value)
        {
            value = default(T);

            try
            {
                var converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(T));
                if (converter != null)
                {
                    value = (T)converter.ConvertFromString(str);
                    return true;
                }
            }
            catch (Exception)
            {
                return false;

            }
            return false;
        }

        public T GetValue<T>(string name)
        {
            T value;
            string errorCode;

            if (!this.GetValue<T>(name, out value, out errorCode))
            {
                logger.Error($"Get Value | Key {name} | Error : {errorCode}");
            }

            return value;
        }

        public bool GetValue<T>(string name, out T value)
        {
            string errorCode;
            return this.GetValue<T>(name, out value, out errorCode);
        }

        /// <summary>
        /// Gets value from node.
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="name">Name of node</param>
        /// <param name="value">value</param>
        /// <param name="errorCode">error code</param>
        /// <returns>Success status</returns>
        public bool GetValue<T>(string name, out T value, out string errorCode)
        {
            errorCode = string.Empty;
            value = default(T);

            if (xmlDocument == null)
            {
                errorCode = "Loaded xml was corrupted";
                return false;
            }

            if (!typeof(T).IsPrimitive && typeof(T) != typeof(string) && !typeof(T).IsEnum)
            {
                errorCode = "Datatype is not valid";
                return false;
            }

            string nodePath = string.Empty;

            errorCode = this.CheckNodeIsValid(name, out nodePath);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return false;
            }

            XmlNode node = xmlDocument.SelectSingleNode(nodePath);

            if (!this.Tryparse<T>(node.InnerText, out value))
            {
                errorCode = "Conversion to datatype fails";
                return false;
            }

            return true;
        }

        public bool SetValue<T>(string name, T value)
        {
            string errorCode;
            return this.SetValue<T>(name, value, out errorCode);
        }

        /// <summary>
        /// Sets value node. 
        /// </summary>
        /// <typeparam name="T">Data type</typeparam>
        /// <param name="name">Name of node</param>
        /// <param name="value">value</param>
        /// <param name="errorCode">error code</param>
        /// <returns>Success status</returns>
        public bool SetValue<T>(string name, T value, out string errorCode)
        {
            errorCode = string.Empty;

            if (xmlDocument == null)
            {
                errorCode = "Loaded xml was corrupted";
                return false;
            }

            string nodePath = string.Empty;

            errorCode = this.CheckNodeIsValid(name, out nodePath);
            if (!string.IsNullOrEmpty(errorCode))
            {
                return false;
            }


            XmlNode node = xmlDocument.SelectSingleNode(nodePath);
            node.InnerText = value.ToString();

            try
            {
                using (StreamWriter sw = new StreamWriter(this.xmlPath))
                {
                    xmlDocument.Save(sw);
                }
            }
            catch (Exception ex)
            {
                errorCode = "XMl file save failed " + ex.Message;
                return false;
            }

            return true;
        }

        public bool CreateNewElement<T>(string name, T value)
        {
            string errorCode;
            return this.CreateNewElement<T>(name, value, out errorCode);
        }

        public bool CreateNewHeaderElement(string name)
        {
            if (xmlDocument == null)
            {
                return false;
            }

            newHeaderElement = xmlDocument.CreateElement(name);
            return true;
        }

        public bool AppendToRoot()
        {
            if (xmlDocument == null)
            { 
                return false; 
            }
            if (newHeaderElement == null)
            {
                return false;
            }
            xmlDocument.DocumentElement.AppendChild(newHeaderElement);
            return true;
        }

        public bool CreateNewElement<T>(string name, T value, out string errorCode)
        {
            errorCode = string.Empty;
            if (xmlDocument == null)
            {
                errorCode = "Loaded xml was corrupted";
                return false;
            }
            
            XmlElement newItemElement = xmlDocument.CreateElement(name);
            newItemElement.InnerText = value.ToString();
            newHeaderElement.AppendChild(newItemElement);
            xmlDocument.DocumentElement.AppendChild(newHeaderElement);
            try
            {
                using (StreamWriter sw = new StreamWriter(this.xmlPath))
                {
                    xmlDocument.Save(sw);
                }
            }
            catch (Exception ex)
            {
                errorCode = "XMl file save failed " + ex.Message;
                return false;
            }

            return true;
        }

        public bool CheckNode(string path, string nameNode, string condition)
        {
            if (xmlDocument == null)
            {
                return false;
            }
            try
            {
                int nodeIndex = 0;
                listNode = xmlDocument.SelectNodes(path);
                foreach (XmlNode _node in listNode)
                {
                    if (_node.SelectSingleNode(nameNode).InnerText == condition)
                    {
                        targetNode = listNode[nodeIndex];
                        break;
                    }
                    nodeIndex++;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            
            return true;
        }
        public bool UpdateInnerSingleNode<T>(string name, T value)
        {
            string errorCode;
            return this.UpdateInnerSingleNode<T>(name, value, out errorCode);
        }
        public bool UpdateInnerSingleNode<T>(string name, T value, out string errorCode)
        {
            errorCode = string.Empty;
            if (xmlDocument == null)
            {
                errorCode = "Loaded xml was corrupted";
                return false;
            }
            if (targetNode == null)
            {
                errorCode = "Target Node is not existing";
                return false;
            }
            try
            {
                childNode = targetNode.SelectSingleNode(name);
                childNode.InnerText = value.ToString();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            
            return true;
        }

        public bool SaveXmlFile()
        {
            string errorCode;
            return this.SaveXmlFile(out errorCode);
        }

        public bool SaveXmlFile(out string errorCode)
        {
            errorCode = string.Empty;
            if (xmlDocument == null)
            {
                errorCode = "Loaded xml was corrupted";
                return false;
            }
            try
            {
                xmlDocument.Save(this.xmlPath);
            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return true;
        }
        public bool DeleteNode()
        {
            try
            {
                targetNode.RemoveAll();
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                return false;
            }
            
            
        }
        public bool RemoveNode(string condition)
        {
            // Select the book node to remove based on the title (you can use a different criteria)
            XmlNode nodeToRemove = xmlDocument.SelectSingleNode($"//RECIPECONFIG[RecipeName='{condition}']");
            if (nodeToRemove != null)
            {
                // Remove the selected node from its parent
                nodeToRemove.ParentNode.RemoveChild(nodeToRemove);
                // Save the modified document
                xmlDocument.Save(this.xmlPath);
                logger.Info("Remove and save new xml file => successful");
                return true;
            }
            else
            {
                logger.Info("Remove and save new xml file => failed");
                return false;
            }

        }
    }
}
