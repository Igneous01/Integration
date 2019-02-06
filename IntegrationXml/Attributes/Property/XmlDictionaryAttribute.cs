using Integration.Xml.Enums;
using Integration.Xml.Interfaces;
using System;

namespace Integration.Xml.Attributes.Property
{
    [AttributeUsage(AttributeTargets.Property)]
    public class XmlDictionaryAttribute : Attribute, IXmlAttributeMarker
    {
        public XmlMappingOperation MappingOperation { get; set; }

        public XmlDictionaryAttribute()
        {
            MappingOperation = XmlMappingOperation.ATTRIBUTE;
        }
    }
}
