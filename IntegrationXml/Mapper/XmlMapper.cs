using Integration.Cache;
using Integration.Cache.Context;
using Integration.Xml.Attributes.Property;
using Integration.Xml.Attributes.Type;
using Integration.Xml.Context;
using Integration.Xml.Enums;
using Integration.Xml.Exceptions;
using Integration.Xml.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TB.ComponentModel;

namespace Integration.Xml.Mapper
{
    public class XmlMapper<T> where T : new()
    {
        private const string XPATH_ROOT_NODE_EXPRESSION = "(/*)";
        private Type type = typeof(T);
        private XmlMapperAttribute xmlMapperAttribute = new XmlMapperAttribute();

        public XmlMapper()
        {
            if (MetaDataCache.Contains<T>())
            {
                ClassMetaData metaData = MetaDataCache.Get<T>();
                if (metaData.ClassAttributeContext.ContainsAttribute<XmlMapperAttribute>())
                    xmlMapperAttribute = metaData.ClassAttributeContext.GetAttribute<XmlMapperAttribute>();
            }         
        }

        public T MapToObject(XmlDocument xml)
        {
            T instance = new T();
            instance = (T)ToObject(instance, xml.SelectSingleNode(XPATH_ROOT_NODE_EXPRESSION));
            return instance;
        }

        public string MapToXmlString(T instance)
        {
            return ToXmlWrapper(type, instance).ToString(xmlMapperAttribute.Formatting);
        }

        public XmlDocument MapToXmlDocument(T instance)
        {
            return ToXmlWrapper(type, instance).ToXmlDocument();
        }

        private object ToObject(object instance, XmlNode xmlNode, string nodeName = null)
        {
            System.Type type = instance.GetType();
            ClassMetaData classMetaData = MetaDataCache.Get(type);
            XmlMappingOperation xmlMappingOperation = XmlMappingOperation.NODE;
           
            if (classMetaData.ClassAttributeContext.ContainsAttribute<XmlMapperAttribute>())
            {
                XmlMapperAttribute xmlMapper = classMetaData.ClassAttributeContext.GetAttribute<XmlMapperAttribute>();
                xmlMappingOperation = xmlMapper.MappingOperation;
                if (nodeName == null)
                {
                    nodeName = xmlMapper.ParentNodeName;
                    xmlNode = xmlNode.SelectSingleNode($"//{nodeName}");
                }
            }

            foreach (PropertyInfo property in classMetaData.Properties)
            {
                string propertyName = property.Name;
                System.Type propertyType = property.PropertyType;

                if (classMetaData.HasPropertyAttributeContext<PropertyAttributeContext>(property))
                {
                    PropertyAttributeContext propertyAttributeContext = classMetaData.GetAttributeContextForProperty<PropertyAttributeContext>(property);

                    if (propertyAttributeContext.HasIgnoreAttribute)
                        continue;

                    propertyName = propertyAttributeContext.HasNameAttribute ? propertyAttributeContext.NameAttribute.Name : propertyName;
                }

                object propertyValue;

                if (xmlMappingOperation.Equals(XmlMappingOperation.NODE))
                {
                    XmlNode propertyNode = xmlNode.SelectSingleNode($"//{nodeName}/{propertyName}");
                    propertyValue = propertyNode?.InnerText;
                }
                else // ATTRIBUTE
                {
                    XmlNode propertyNode = xmlNode.SelectSingleNode($"//{nodeName}");
                    propertyValue = propertyNode.Attributes[propertyName]?.Value;
                }

                if (classMetaData.HasPropertyAttributeContext<XmlPropertyAttributeContext>(property))
                {
                    XmlPropertyAttributeContext xmlPropertyAttributeContext = classMetaData.GetAttributeContextForProperty<XmlPropertyAttributeContext>(property);

                    if (xmlPropertyAttributeContext.HasXmlListAttribute)
                    {
                        XmlListAttribute xmlList = xmlPropertyAttributeContext.XmlListAttribute;
                        XmlNodeList results = xmlNode.SelectNodes($"//{nodeName}/{propertyName}/{xmlList.NodeName}");
                        object childInstance = EnumerableXmlNodeListToObject(results, propertyType);
                        property.SetValue(instance, childInstance);
                        continue;
                    }
                    else if (xmlPropertyAttributeContext.HasXmlFlattenHierarchyAttribute)
                    {
                        object childInstance = Activator.CreateInstance(propertyType);
                        childInstance = ToObject(childInstance, xmlNode.SelectSingleNode($"//{nodeName}/{propertyName}"), propertyName);
                        property.SetValue(instance, childInstance);
                        continue;
                    }
                    else if (xmlPropertyAttributeContext.HasXmlPropertyConverterAttribute && propertyValue != null)
                    {
                        XmlPropertyConverterAttribute converter = xmlPropertyAttributeContext.XmlPropertyConverterAttribute;
                        try
                        {
                            propertyValue = converter.ConvertToSourceType(propertyValue);
                        }
                        catch (Exception ex)
                        {
                            throw new XmlPropertyConverterException("XmlPropertyConverter threw an exception", ex);
                        }          
                    }
                    else if (propertyType.IsClass && MetaDataCache.Contains(propertyType))
                    {
                        // TODO: Dont think this will work
                        object childInstance = Activator.CreateInstance(propertyType);
                        childInstance = ToObject(childInstance, xmlNode.SelectSingleNode($"//{nodeName}/{propertyName}"), propertyName);
                        property.SetValue(instance, childInstance);
                        continue;
                    }
                }
                // TODO: Remove duplicated code
                else if (propertyType.IsClass && MetaDataCache.Contains(propertyType))
                {
                    // TODO: Dont think this will work
                    object childInstance = Activator.CreateInstance(propertyType);
                    childInstance = ToObject(childInstance, xmlNode.SelectSingleNode($"//{nodeName}/{propertyName}"), propertyName);
                    property.SetValue(instance, childInstance);
                    continue;
                }

                property.SetValue(instance, UniversalTypeConverter.Convert(propertyValue, propertyType));
            }

            return instance;
        }

        private XDocument ToXmlWrapper(Type type, T instance)
        {         
            XElement rootElement = ToXml(instance, type);
            XDocument xDocument = new XDocument();
            xDocument.Add(rootElement);     

            // will throw exception if fails to construct XmlDocument
            if (xmlMapperAttribute.Validate)
                xDocument.ToXmlDocument();

            return xDocument;
        }

        private XElement ToXml(object instance, Type type, XElement element = null, string nodeName = null)
        {
            ClassMetaData classMetaData = MetaDataCache.Get(type);
            XmlMappingOperation xmlMappingOperation = xmlMapperAttribute.MappingOperation;
            bool ignoreNulls = xmlMapperAttribute.IgnoreNulls;

            if (classMetaData.ClassAttributeContext.ContainsAttribute<XmlMapperAttribute>())
            {
                XmlMapperAttribute xmlMapper = classMetaData.ClassAttributeContext.GetAttribute<XmlMapperAttribute>();
                xmlMappingOperation = xmlMapper.MappingOperation;
                ignoreNulls = xmlMapper.IgnoreNulls;

                if (element == null)
                {
                    element = new XElement(xmlMapper.ParentNodeName);
                    nodeName = xmlMapper.ParentNodeName;
                }
            }

            element.Name = nodeName;

            foreach (PropertyInfo property in classMetaData.Properties)
            {
                string propertyName = property.Name;
                System.Type propertyType = property.PropertyType;
                object propertyValue = property.GetValue(instance);

                if (propertyValue == null && ignoreNulls)
                    continue;

                if (classMetaData.HasPropertyAttributeContext<PropertyAttributeContext>(property))
                {
                    PropertyAttributeContext propertyAttributeContext = classMetaData.GetAttributeContextForProperty<PropertyAttributeContext>(property);
                    if (propertyAttributeContext.HasIgnoreAttribute)
                        continue;

                    propertyName = propertyAttributeContext.HasNameAttribute ? propertyAttributeContext.NameAttribute.Name : propertyName;
                }

                if (classMetaData.HasPropertyAttributeContext<XmlPropertyAttributeContext>(property))
                {
                    XmlPropertyAttributeContext xmlPropertyAttributeContext = classMetaData.GetAttributeContextForProperty<XmlPropertyAttributeContext>(property);

                    if (xmlPropertyAttributeContext.HasXmlPropertyConverterAttribute && propertyValue != null)
                    {
                        XmlPropertyConverterAttribute converter = xmlPropertyAttributeContext.XmlPropertyConverterAttribute;
                        try
                        {
                            propertyValue = converter.ConvertToDestinationType(propertyValue);
                        }
                        catch (Exception ex)
                        {
                            throw new XmlPropertyConverterException("XmlPropertyConverter threw an exception", ex);
                        }
                    }
                    else if (xmlPropertyAttributeContext.HasXmlListAttribute)
                    {
                        XmlListAttribute xmlList = xmlPropertyAttributeContext.XmlListAttribute;
                        element.Add(EnumerableToXElement(propertyValue, propertyName, xmlList.NodeName));
                        continue;
                    }
                    else if (xmlPropertyAttributeContext.HasXmlFlattenHierarchyAttribute)
                    {
                        element = ToXml(propertyValue, propertyType, element, propertyName);
                        continue;
                    }
                    else if (propertyType.IsClass && MetaDataCache.Contains(propertyType))
                    {
                        XElement propertyElement = new XElement(propertyName);
                        propertyElement = ToXml(propertyValue, propertyType, propertyElement, propertyName);
                        element.Add(propertyElement);
                        continue;
                    }
                }
                // TODO: Remove duplicated code
                else if (propertyType.IsClass && MetaDataCache.Contains(propertyType))
                {
                    XElement propertyElement = new XElement(propertyName);
                    propertyElement = ToXml(propertyValue, propertyType, propertyElement, propertyName);
                    element.Add(propertyElement);
                    continue;
                }

                if (xmlMappingOperation.Equals(XmlMappingOperation.ATTRIBUTE))
                    element.SetAttributeValue(propertyName, propertyValue);
                else if (xmlMappingOperation.Equals(XmlMappingOperation.NODE))
                    element.Add(new XElement(propertyName, propertyValue));
            }

            return element;
        }

        // TODO, needs to support nested complex objects inside here
        private XElement EnumerableToXElement(object enumerable, string parentNodeName, string childNodeName)
        {
            XElement propertyElement = new XElement(parentNodeName);

            foreach (var item in (IEnumerable)enumerable)
            {
                XElement itemElement = new XElement(childNodeName, item);
                propertyElement.Add(itemElement);
            }

            return propertyElement;
        }

        private object EnumerableXmlNodeListToObject(XmlNodeList nodeList, System.Type enumerableType)
        {
            object enumerable = Activator.CreateInstance(enumerableType);
            System.Type containedType = enumerableType.GetTypeInfo().GenericTypeArguments[0];

            foreach (XmlNode node in nodeList)
            {
                // TODO: not compatible with strings or other immutable types
                //object containedInstance = Activator.CreateInstance(containedType);
                //containedInstance = node.Value;
                enumerableType.GetMethod("Add").Invoke(enumerable, new[] { node.InnerText });
            }

            return enumerable;
        }
    }
}
