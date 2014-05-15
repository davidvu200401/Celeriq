#pragma warning disable 0168
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace Celeriq.Common
{
    public class XmlHelper
    {
        private XmlHelper()
        {
        }

        public static XPathNavigator CreateXPathNavigator(XmlReader reader)
        {
            var document = new XPathDocument(reader);
            return document.CreateNavigator();
        }

        public static XPathNodeIterator GetIterator(XPathNavigator navigator, string xPath)
        {
            return (XPathNodeIterator) navigator.Evaluate(xPath);
        }

        #region GetXmlReader

        public static XmlReader GetXmlReader(FileInfo fileInfo)
        {
            var textReader = new XmlTextReader(fileInfo.FullName);
            return textReader;
        }

        #endregion

        #region GetNode

        public static System.Xml.XmlNode GetNode(System.Xml.XmlNode xmlNode, string XPath)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = xmlNode.SelectSingleNode(XPath);
                return node;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static System.Xml.XmlNode GetNode(System.Xml.XmlNode xmlNode, string XPath, XmlNamespaceManager nsManager)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = xmlNode.SelectSingleNode(XPath, nsManager);
                return node;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region GetNodeValue

        public static string GetNodeValue(System.Xml.XmlDocument document, string XPath, string defaultValue)
        {
            try
            {
                return GetNodeValue(document.DocumentElement, XPath, defaultValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetNodeValue(System.Xml.XmlNode element, string XPath, string defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return node.InnerText;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static int GetNodeValue(System.Xml.XmlNode element, string XPath, int defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return int.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static int? GetNodeValue(System.Xml.XmlNode element, string XPath, int? defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return int.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static Single GetNodeValue(System.Xml.XmlNode element, string XPath, Single defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return Single.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static Single? GetNodeValue(System.Xml.XmlNode element, string XPath, Single? defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return Single.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static double GetNodeValue(System.Xml.XmlNode element, string XPath, double defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return double.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static double? GetNodeValue(System.Xml.XmlNode element, string XPath, double? defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return double.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static bool GetNodeValue(System.Xml.XmlNode element, string XPath, bool defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return bool.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static bool? GetNodeValue(System.Xml.XmlNode element, string XPath, bool? defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return bool.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static DateTime GetNodeValue(System.Xml.XmlNode element, string XPath, DateTime defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return DateTime.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static DateTime? GetNodeValue(System.Xml.XmlNode element, string XPath, DateTime? defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else
                    return DateTime.Parse(node.InnerText);
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public static string GetNodeValue(System.Xml.XmlDocument document, string XPath, XmlNamespaceManager nsManager, string defaultValue)
        {
            try
            {
                return GetNodeValue(document.DocumentElement, XPath, nsManager, defaultValue);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetNodeValue(System.Xml.XmlNode element, string XPath, XmlNamespaceManager nsManager, string defaultValue)
        {
            try
            {
                System.Xml.XmlNode node = null;
                node = element.SelectSingleNode(XPath, nsManager);
                if (node == null)
                    return defaultValue;
                else
                    return node.InnerText;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        #endregion

        #region GetNodeXML

        public static string GetNodeXML(XmlDocument document, string XPath, string defaultValue, bool useOuter)
        {
            try
            {
                XmlNode node = null;
                node = document.SelectSingleNode(XPath);
                if (node == null)
                    return defaultValue;
                else if (useOuter)
                    return node.OuterXml;
                else
                    return node.InnerXml;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static string GetNodeXML(XmlDocument document, string XPath, string defaultValue)
        {
            return GetNodeXML(document, XPath, defaultValue, false);
        }

        #endregion

        #region GetAttributeValue

        public static bool AttributeExists(XmlNode node, string attributeName)
        {
            return (node.Attributes[attributeName] != null);
        }

        public static string GetAttribute(XmlNode node, string attributeName)
        {
            return GetAttribute(node, attributeName, "");
        }

        public static string GetAttribute(XmlNode node, string attributeName, string defaultValue)
        {
            var attr = node.Attributes[attributeName];
            if (attr == null)
                attr = node.Attributes[attributeName.ToLower()];

            if (attr == null)
                return defaultValue;
            else
                return attr.Value;
        }

        public static Guid GetAttribute(XmlNode node, string attributeName, Guid defaultValue)
        {
            var attr = node.Attributes[attributeName];
            if (attr == null)
                attr = node.Attributes[attributeName.ToLower()];

            if (attr == null)
                return defaultValue;
            else
                return new Guid(attr.Value);
        }

        public static double GetAttribute(XmlNode node, string attributeName, double defaultValue)
        {
            var attr = node.Attributes[attributeName];
            if (attr == null)
                attr = node.Attributes[attributeName.ToLower()];

            if (attr == null)
                return defaultValue;
            else
                return double.Parse(attr.Value);
        }

        public static int GetAttribute(XmlNode node, string attributeName, int defaultValue)
        {
            var attr = node.Attributes[attributeName];
            if (attr == null)
                attr = node.Attributes[attributeName.ToLower()];

            if (attr == null)
                return defaultValue;
            else
                return int.Parse(attr.Value);
        }

        public static long GetAttribute(XmlNode node, string attributeName, long defaultValue)
        {
            var attr = node.Attributes[attributeName];
            if (attr == null)
                attr = node.Attributes[attributeName.ToLower()];

            if (attr == null)
                return defaultValue;
            else
                return long.Parse(attr.Value);
        }

        public static bool GetAttribute(XmlNode node, string attributeName, bool defaultValue)
        {
            var attr = node.Attributes[attributeName];
            if (attr == null)
                attr = node.Attributes[attributeName.ToLower()];

            if (attr == null)
                return defaultValue;
            else
                return bool.Parse(attr.Value);
        }

        #endregion

        #region AddElement

        /// <summary />
        public static XmlNode AddElement(XmlElement element, string name, string value)
        {
            XmlDocument document = null;
            XmlElement elemNew = null;

            document = element.OwnerDocument;
            elemNew = document.CreateElement(name);
            if (!string.IsNullOrEmpty(value))
                elemNew.InnerText = value;
            return element.AppendChild(elemNew);
        }

        /// <summary />
        public static XmlNode AddElement(XmlDocument document, string name, string value)
        {
            var elemNew = document.CreateElement(name);
            if (!string.IsNullOrEmpty(value))
                elemNew.InnerText = value;
            return document.AppendChild(elemNew);
        }

        /// <summary />
        public static XmlNode AddElement(XmlNode element, string name, string value)
        {
            return AddElement((XmlElement) element, name, value);
        }

        /// <summary />
        public static XmlNode AddElement(XmlElement element, string name)
        {
            return AddElement((XmlNode) element, name);
        }

        /// <summary />
        public static XmlNode AddElement(XmlNode element, string name)
        {
            XmlDocument document = null;
            XmlElement elemNew = null;
            document = element.OwnerDocument;
            elemNew = document.CreateElement(name);
            return element.AppendChild(elemNew);
        }

        /// <summary />
        public static XmlNode AddElement(XmlDocument xmlDocument, string name)
        {
            XmlElement elemNew = null;
            elemNew = xmlDocument.CreateElement(name);
            return xmlDocument.AppendChild(elemNew);
        }

        #endregion

        #region AddAttribute

        public static XmlAttribute AddAttribute(XmlNode node, string name, string value)
        {
            XmlDocument docOwner = null;
            XmlAttribute attrNew = null;

            docOwner = node.OwnerDocument;
            attrNew = docOwner.CreateAttribute(name);
            attrNew.InnerText = value;
            node.Attributes.Append(attrNew);
            return attrNew;
        }

        public static XmlAttribute AddAttribute(XmlElement node, string name, bool value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlElement node, string name, double value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlElement node, string name, Guid value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlElement node, string name, int value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, bool value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, double value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, Guid value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, int value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        public static XmlAttribute AddAttribute(XmlElement node, string name, string value)
        {
            return AddAttribute((XmlNode) node, name, value);
        }

        public static XmlAttribute AddAttribute(XmlNode node, string name, long value)
        {
            return AddAttribute(node, name, value.ToString());
        }

        #endregion

        #region RemoveElement

        public static void RemoveElement(XmlDocument document, string XPath)
        {
            XmlNode parentNode = null;
            XmlNodeList nodes = null;

            nodes = document.SelectNodes(XPath);
            if (nodes != null)
            {
                foreach (XmlElement node in nodes)
                {
                    if (node != null)
                    {
                        parentNode = node.ParentNode;
                        node.RemoveAll();
                        parentNode.RemoveChild(node);
                    }
                }
            }
        }

        public static void RemoveElement(XmlElement element)
        {
            var parentNode = element.ParentNode;
            parentNode.RemoveChild(element);
        }

        public static void RemoveAttribute(XmlElement element, string attributeName)
        {
            XmlAttribute attrDelete = null;
            attrDelete = (XmlAttribute) element.Attributes.GetNamedItem(attributeName);
            if (attrDelete == null) return;
            element.Attributes.Remove(attrDelete);
        }

        #endregion

        #region UpdateElement

        public static void UpdateElement(XmlElement element, string newValue)
        {
            element.InnerText = newValue;
        }

        public static void UpdateElement(ref XmlDocument XMLDocument, string Xpath, string newValue)
        {
            XMLDocument.SelectSingleNode(Xpath).InnerText = newValue;
        }

        public static void UpdateAttribute(XmlElement XmlElement, string attributeName, string newValue)
        {
            XmlAttribute attrTemp = null;
            attrTemp = (XmlAttribute) XmlElement.Attributes.GetNamedItem(attributeName);
            attrTemp.InnerText = newValue;
        }

        #endregion

        #region GetElement

        public static XmlElement GetElement(XmlElement parentElement, string tagName)
        {
            var list = parentElement.GetElementsByTagName(tagName);
            if (list.Count > 0)
                return (XmlElement) list[0];
            else
                return null;
        }

        #endregion

        #region GetChildValue

        public static string GetChildValue(XmlNode parentNode, string childName)
        {
            return GetChildValue(parentNode, childName, null);
        }

        public static double GetChildValue(XmlNode parentNode, string childName, double defaultValue)
        {
            var node = parentNode.SelectSingleNode(childName);
            if (node != null)
                return double.Parse(node.InnerText);
            else
                return defaultValue;
        }

        public static int GetChildValue(XmlNode parentNode, string childName, int defaultValue)
        {
            var node = parentNode.SelectSingleNode(childName);
            if (node != null)
                return int.Parse(node.InnerText);
            else
                return defaultValue;
        }

        public static bool GetChildValue(XmlNode parentNode, string childName, bool defaultValue)
        {
            var node = parentNode.SelectSingleNode(childName);
            if (node != null)
                return bool.Parse(node.InnerText);
            else
                return defaultValue;
        }

        public static Guid GetChildValue(XmlNode parentNode, string childName, Guid defaultValue)
        {
            var node = parentNode.SelectSingleNode(childName);
            if (node != null)
                return new Guid(node.InnerText);
            else
                return defaultValue;
        }

        public static string GetChildValue(XmlNode parentNode, string childName, string defaultValue)
        {
            var node = parentNode.SelectSingleNode(childName);
            if (node != null)
                return node.InnerText;
            else
                return defaultValue;
        }

        #endregion

        #region FormatXMLString

        public static string FormatXMLString(string xml)
        {
            var xd = new XmlDocument();
            xd.LoadXml(xml);
            var sb = new StringBuilder();
            var sw = new StringWriter(sb);
            XmlTextWriter xtw = null;
            try
            {
                xtw = new XmlTextWriter(sw);
                xtw.Formatting = Formatting.Indented;
                xd.WriteTo(xtw);
            }
            finally
            {
                if (xtw != null)
                    xtw.Close();
            }
            return sb.ToString();
        }

        #endregion

    }
}