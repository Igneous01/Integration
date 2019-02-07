using Integration.Cache;
using Integration.Cache.Context;
using Integration.Utils;
using Integration.Xml.Attributes.Property;
using Integration.Xml.Attributes.Type;
using Integration.Xml.Context;
using Integration.Xml.Enums;
using Integration.Xml.Exceptions;
using Integration.Xml.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
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
            XmlMappingOperation xmlMappingOperation = xmlMapperAttribute.MappingOperation;
           
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
                        if (results.Count > 0)
                        {
                            object childInstance = CollectionXmlNodeListToObject(results, propertyType);
                            property.SetValue(instance, childInstance);
                        }                    
                        continue;
                    }
                    else if (xmlPropertyAttributeContext.HasXmlDictionaryAttribute)
                    {
                        XmlDictionaryAttribute xmlList = xmlPropertyAttributeContext.XmlDictionaryAttribute;
                        XmlNodeList results = xmlNode.SelectNodes($"//{nodeName}/{propertyName}/*");
                        if (results.Count > 0)
                        {
                            object childInstance = DictionaryXmlNodeListToObject(results, propertyType);
                            property.SetValue(instance, childInstance);
                        }
                        continue;
                    }
                    else if (xmlPropertyAttributeContext.HasXmlFlattenHierarchyAttribute)
                    {
                        object childInstance = Activator.CreateInstance(propertyType);
                        XmlNode results = xmlNode.SelectSingleNode($"//{nodeName}/{propertyName}");
                        if (results != null)
                        {
                            childInstance = ToObject(childInstance, results, propertyName);
                            property.SetValue(instance, childInstance);
                        }
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
                }
                else
                {
                    if (propertyType.IsDictionary())
                    {
                        XmlNodeList results = xmlNode.SelectNodes($"//{nodeName}/{propertyName}/*");
                        if (results.Count > 0)
                        {
                            object childInstance = DictionaryXmlNodeListToObject(results, propertyType);
                            property.SetValue(instance, childInstance);
                        }                   
                        continue;
                    }
                    else if (propertyType.IsCollection())
                    {
                        string listItemNodeName = propertyType.GenericTypeArguments[0].Name;
                        XmlNodeList results = xmlNode.SelectNodes($"//{nodeName}/{propertyName}/{listItemNodeName}");
                        if (results.Count > 0)
                        {
                            object childInstance = CollectionXmlNodeListToObject(results, propertyType);
                            property.SetValue(instance, childInstance);
                        }                 
                        continue;
                    }            
                    if (propertyType.IsClass && MetaDataCache.Contains(propertyType))
                    {
                        // TODO: Dont think this will work
                        object childInstance = Activator.CreateInstance(propertyType);
                        XmlNode results = xmlNode.SelectSingleNode($"//{nodeName}/{propertyName}");
                        if (results != null)
                        {
                            childInstance = ToObject(childInstance, results, propertyName);
                            property.SetValue(instance, childInstance);
                        }           
                        continue;
                    }
                }
               
                property.SetValue(instance, UniversalTypeConverter.Convert(propertyValue, propertyType));
            }

            return instance;
        }

        private object CollectionXmlNodeListToObject(XmlNodeList nodeList, Type collectionType)
        {
            object collection = CreateInstanceOfType(collectionType);     
            Type containedType = collectionType.GetTypeInfo().GenericTypeArguments[0];
            Type iCollectionType = typeof(ICollection<>).MakeGenericType(containedType);

            foreach (XmlNode node in nodeList)
            {
                object value = CreateInstanceOfType(containedType);

                if (containedType.IsClass && MetaDataCache.Contains(containedType))
                    value = ToObject(value, node, node.Name);
                else
                    value = node.InnerText;

                iCollectionType.GetMethod("Add").Invoke(collection, new[] { value });
            }

            return collection;
        }

        private object DictionaryXmlNodeListToObject(XmlNodeList nodeList, System.Type dictionaryType)
        {
            object dictionary = Activator.CreateInstance(dictionaryType);
            Type keyType = dictionaryType.GetTypeInfo().GenericTypeArguments[0];
            Type valueType = dictionaryType.GetTypeInfo().GenericTypeArguments[1];

            foreach (XmlNode node in nodeList)
            {
                object key = node.Name; // will be replaced with dictionary mapper later
                object value = CreateInstanceOfType(valueType);

                if (valueType.IsClass && MetaDataCache.Contains(valueType))
                    value = ToObject(value, node, node.Name);
                else
                    value = node.InnerText;

                dictionaryType.GetMethod("Add").Invoke(dictionary, new[] { node.Name, value });
            }

            return dictionary;
        }

        private object CreateInstanceOfType(Type type)
        {
            if (!type.HasDefaultConstructor())
                return null;
            else
                return Activator.CreateInstance(type);
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

            //element.Name = nodeName;

            foreach (PropertyInfo property in classMetaData.Properties)
            {
                string propertyName = property.Name;
                object propertyValue = property.GetValue(instance);
                System.Type propertyType = property.PropertyType;        

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
                            AddToXElement(element, xmlMappingOperation, propertyName, propertyValue);
                            continue;
                        }
                        catch (Exception ex)
                        {
                            throw new XmlPropertyConverterException("XmlPropertyConverter threw an exception", ex);
                        }
                    }
                    else if (xmlPropertyAttributeContext.HasXmlDictionaryAttribute)
                    {
                        XmlDictionaryAttribute xmlDictionary = xmlPropertyAttributeContext.XmlDictionaryAttribute;
                        element.Add(DictionaryToXElement(propertyValue, propertyName));
                        continue;
                    }
                    else if (xmlPropertyAttributeContext.HasXmlListAttribute)
                    {
                        XmlListAttribute xmlList = xmlPropertyAttributeContext.XmlListAttribute;
                        element.Add(CollectionToXElement(propertyValue, propertyName, xmlList.NodeName));
                        continue;
                    }                   
                    else if (xmlPropertyAttributeContext.HasXmlFlattenHierarchyAttribute)
                    {
                        element = ToXml(propertyValue, propertyType, element, propertyName);
                        continue;
                    }
                }
                else
                {
                    if (propertyType.IsDictionary())
                    {
                        element.Add(DictionaryToXElement(propertyValue, propertyName));
                        continue;
                    }
                    else if (propertyType.IsCollection())
                    {
                        element.Add(CollectionToXElement(propertyValue, propertyName, propertyType.GenericTypeArguments[0].Name));
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

                AddToXElement(element, xmlMappingOperation, propertyName, propertyValue);
            }

            return element;
        }

        private void AddToXElement(XElement element, XmlMappingOperation xmlMappingOperation, string propertyName, object propertyValue)
        {
            if (xmlMappingOperation.Equals(XmlMappingOperation.ATTRIBUTE))
                element.SetAttributeValue(propertyName, propertyValue);
            else if (xmlMappingOperation.Equals(XmlMappingOperation.NODE))
                element.Add(new XElement(propertyName, propertyValue));
        }

        private XElement CollectionToXElement(object collection, string parentNodeName, string childNodeName)
        {
            XElement propertyElement = new XElement(parentNodeName);
            Type containedType = collection.GetType().GenericTypeArguments[0];

            foreach (var item in (ICollection)collection)
            {
                XElement itemElement = new XElement(childNodeName);
                if (containedType.IsClass && MetaDataCache.Contains(containedType))
                    itemElement = ToXml(item, item.GetType(), itemElement, childNodeName);
                else
                    itemElement.SetValue(item);
                
                propertyElement.Add(itemElement);
            }

            return propertyElement;
        }

        private XElement DictionaryToXElement(object dictionary, string parentNodeName)
        {
            XElement propertyElement = new XElement(parentNodeName);
            Type keyType = dictionary.GetType().GenericTypeArguments[0];
            Type valueType = dictionary.GetType().GenericTypeArguments[1];

            foreach (DictionaryEntry kvp in (IDictionary)dictionary)
            {
                // TODO - this will call converter that converts Dictionary key to XElement
                XElement itemElement = new XElement(kvp.Key.ToString());
                if (valueType.IsClass && MetaDataCache.Contains(valueType))
                    itemElement = ToXml(kvp.Value, kvp.Value.GetType(), itemElement, itemElement.Name.LocalName);
                else
                    itemElement.SetValue(kvp.Value);

                propertyElement.Add(itemElement);
            }

            return propertyElement;
        }  
    }
}
