using Integration.Cache.Context;
using Integration.Exceptions;
using Integration.Interfaces;
using Integration.Utils;
using System;
using System.Collections.Concurrent;
using System.Reflection;
using ConcurrentDictionaryTypeContext = System.Collections.Concurrent.ConcurrentDictionary<System.Type, Integration.Cache.Context.AbstractAttributeContext>;

namespace Integration.Cache
{
    public class ClassMetaData
    {
        private ConcurrentDictionary<PropertyInfo, ConcurrentDictionaryTypeContext> _propertyContexts;
        private ClassAttributeContext _classAttributeContext;
        private Type _classType;
        private PropertyInfo[] _properties;

        public ClassMetaData(Type type)
        {
            _classType = type;
            _classAttributeContext = new ClassAttributeContext(type);
            _properties = type.GetProperties(ReflectionUtils.GetPublicBindingFlags());
            _propertyContexts = new ConcurrentDictionary<PropertyInfo, ConcurrentDictionaryTypeContext>();
            
            foreach (PropertyInfo property in _properties)
            {
                ConcurrentDictionaryTypeContext propertyDictionary = new ConcurrentDictionaryTypeContext();

                foreach(MetaDataEntry entry in MetaDataRegistry.Entries)
                {
                    if (ReflectionUtils.HasCustomAttributeType(entry.Interface, property))
                    {
                        AbstractAttributeContext context = (AbstractAttributeContext)Activator.CreateInstance(entry.Type, new object[] { property });
                        if (entry.Validator != null)
                        {
                            IPropertyAttributeValidator validatorInstance = (IPropertyAttributeValidator)Activator.CreateInstance(entry.Validator);
                            validatorInstance.Validate(property, _classType, context);
                        }
                        if (!propertyDictionary.TryAdd(entry.Type, context))
                            throw new ReflectionCacheException($"Type {type.AssemblyQualifiedName} could not be added to the cache because property {property.Name} could not be added to cache");
                    }                
                }

                if (!_propertyContexts.TryAdd(property, propertyDictionary))
                    throw new ReflectionCacheException($"Type {type.AssemblyQualifiedName} could not be added to the cache");
            }        
        }

        public Type Type { get { return _classType; } }

        public PropertyInfo[] Properties { get { return _properties; } }

        public ClassAttributeContext ClassAttributeContext { get { return _classAttributeContext; } }

        public AttributeContextType GetAttributeContextForProperty<AttributeContextType>(PropertyInfo property) where AttributeContextType : AbstractAttributeContext
        {
            return (AttributeContextType)_propertyContexts[property][typeof(AttributeContextType)];
        }

        public bool HasPropertyAttributeContext<AttributeContextType>(PropertyInfo property)
        {
            return _propertyContexts.ContainsKey(property) && _propertyContexts[property].ContainsKey(typeof(AttributeContextType));
        }

        public ConcurrentDictionaryTypeContext GetContextsForProperty(PropertyInfo property)
        {
            return _propertyContexts[property];
        }
    }
}
