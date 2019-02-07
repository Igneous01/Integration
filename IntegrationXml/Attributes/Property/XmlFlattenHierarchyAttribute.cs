using Integration.Xml.Interfaces;
using System;
using System.Xml.Linq;

namespace Integration.Xml.Attributes.Property
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class XmlFlattenHierarchyAttribute : Attribute, IXmlAttributeMarker
    {
    }
}
