using Integration.Xml.Interfaces;
using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace Integration.Xml.Attributes.Property
{
    [AttributeUsage(AttributeTargets.Property)]
    public class XmlListAttribute : Attribute, IXmlAttributeMarker
    {
        public string NodeName { get; set; }

        public XmlListAttribute()
        {
            NodeName = "List";
        }
    }
}
