using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace Integration.Xml.Utils
{
    public static class XmlUtils
    {
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }

        public static XmlDocument ToXmlDocument(this XPathDocument xPathDocument)
        {
            var xmlDocument = new XmlDocument();
            xmlDocument.Load(xPathDocument.CreateNavigator().ReadSubtree());
            return xmlDocument;
        }

        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }

        public static XDocument ToXDocument(this XPathDocument xPathDocument)
        {
            return XDocument.Load(xPathDocument.CreateNavigator().ReadSubtree());
        }

        public static XPathDocument ToXPathDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return new XPathDocument(nodeReader);
            }
        }

        public static XPathDocument ToXPathDocument(this XDocument xDocument)
        {
            using (var xmlReader = xDocument.CreateReader())
            {
                return new XPathDocument(xmlReader);
            }
        }
    }
}
