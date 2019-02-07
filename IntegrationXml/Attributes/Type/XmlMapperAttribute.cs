using Integration.Xml.Enums;
using Integration.Xml.Interfaces;
using System;
using System.Xml.Linq;

namespace Integration.Xml.Attributes.Type
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class XmlMapperAttribute : Attribute, IXmlAttributeMarker
    {
        public string ParentNodeName { get; set; }
        public bool IgnoreNulls { get; set; }
        public bool Validate { get; set; }
        public XmlMappingOperation MappingOperation { get; set; }
        public SaveOptions Formatting { get; set; }

        public XmlMapperAttribute()
        {
            ParentNodeName = "Xml";
            IgnoreNulls = true;
            Validate = true;
            MappingOperation = XmlMappingOperation.NODE;    
            Formatting = SaveOptions.None;
        }
    }
}
