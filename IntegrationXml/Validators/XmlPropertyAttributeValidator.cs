using Integration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Integration.Cache;
using Integration.Xml.Context;
using Integration.Utils;
using Integration.Xml.Attributes.Property;
using Integration.Cache.Context;
using Integration.Xml.Exceptions;
using System.Collections;

namespace IntegrationXml.Validators
{
    public class XmlPropertyAttributeValidator : IPropertyAttributeValidator
    {
        private static readonly string FLATTEN_HIERARCHY_REASON_ERROR = $"{typeof(XmlFlattenHierarchyAttribute).Name} only applies to properties that are non-primitive types, such as classes that would involve creating a new node in the Xml. The current type does not satisfy these requirements.";
        private static readonly string LIST_REASON_ERROR = $"{typeof(XmlListAttribute).Name} only applies to properties that implement {typeof(ICollection<>).FullName}.";
        private static readonly string DICTIONARY_REASON_ERROR = $"{typeof(XmlDictionaryAttribute).Name} only applies to properties that implement {typeof(IDictionary).FullName}.";
        private static readonly string DICTIONARY_AUTOMAP_REASON_ERROR = $"Properties that implement IDictionary can only be automatically mapped if key is type {typeof(String).FullName}. To map this type, please add {typeof(XmlDictionaryAttribute).FullName} attribute to property.";

        public void Validate(IFieldPropertyInfo property, Type parentType, AbstractAttributeContext context)
        {
            XmlPropertyAttributeContext xmlContext = (XmlPropertyAttributeContext)context;
            Type type = property.Type;

            if (xmlContext.HasXmlFlattenHierarchyAttribute && !type.IsClass)
                throw InvalidXmlAttributeException.Create(parentType, property, typeof(XmlFlattenHierarchyAttribute), FLATTEN_HIERARCHY_REASON_ERROR);

            if (xmlContext.HasXmlListAttribute && !type.IsCollection())
                throw InvalidXmlAttributeException.Create(parentType, property, typeof(XmlListAttribute), LIST_REASON_ERROR);

            if (xmlContext.HasXmlDictionaryAttribute && !type.IsDictionary())
                throw InvalidXmlAttributeException.Create(parentType, property, typeof(XmlDictionaryAttribute), DICTIONARY_REASON_ERROR);

            if (!xmlContext.HasXmlDictionaryAttribute && type.IsDictionary() && type.GenericTypeArguments[0].Equals(typeof(String)))
                throw InvalidXmlMappingException.Create(parentType, property, DICTIONARY_AUTOMAP_REASON_ERROR);
        }
    }
}
