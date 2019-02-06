using Integration.Cache;
using Integration.Cache.Context;
using Integration.Xml.Attributes.Property;
using Integration.Xml.Context;
using Integration.Xml.Enums;
using Integration.Xml.Interfaces;
using Integration.Xml.Utils;
using System;
using System.Collections;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using TB.ComponentModel;

namespace Integration.Xml.Attributes.Type
{
    [AttributeUsage(AttributeTargets.Class)]
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
