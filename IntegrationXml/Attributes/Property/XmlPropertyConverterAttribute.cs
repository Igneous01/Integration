using System;
using Integration.Abstracts;
using Integration.Utils;
using Integration.Xml.Interfaces;
using Integration.Exceptions;

namespace Integration.Xml.Attributes.Property
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class XmlPropertyConverterAttribute : AbstractPropertyConverterAttribute, IXmlAttributeMarker
    {
        public XmlPropertyConverterAttribute(System.Type converter) : base(converter)
        {
            System.Type IPropertyConverter = typeof(IXmlPropertyConverter<>);
            if (!ReflectionUtils.IsAssignableToGenericType(converter, IPropertyConverter))
                throw new InterfaceNotImplementedException("Type " + converter.AssemblyQualifiedName + " does not implement interface " + IPropertyConverter.AssemblyQualifiedName);
        }
    }
}
