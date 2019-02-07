using Integration.Interfaces;
using Integration.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Integration.Cache.Context
{
    public abstract class AbstractAttributeContext
    {
        protected ConcurrentBag<IAttributeMarker> _attributes;

        public AbstractAttributeContext()
        {
            _attributes = new ConcurrentBag<IAttributeMarker>();
        }

        public AbstractAttributeContext(IFieldPropertyInfo propertyInfo) : this()
        {
            AddAttributes(ReflectionUtils.GetCustomAttributes<IAttributeMarker>(propertyInfo));
        }

        public AbstractAttributeContext(Type classType) : this()
        {
            AddAttributes(ReflectionUtils.GetCustomAttributes<IAttributeMarker>(classType));
        }

        public IReadOnlyCollection<IAttributeMarker> Attributes { get { return _attributes; } }

        public bool ContainsAttribute<T>() => _attributes?
                    .Where((IAttributeMarker attribute) => attribute is T)
                    .FirstOrDefault() != null;

        public bool ContainsAttribute(Type type) => _attributes?
                    .Where((IAttributeMarker attribute) => type.IsAssignableFrom(typeof(IAttributeMarker)))
                    .FirstOrDefault() != null;

        public T GetAttribute<T>() => _attributes
                    .Where((IAttributeMarker attribute) => attribute is T)
                    .Select((IAttributeMarker attribute) => (T)attribute)
                    .FirstOrDefault();

        protected void AddAttributes(IAttributeMarker[] attributes)
        {
            foreach (IAttributeMarker attribute in attributes)
                _attributes.Add(attribute);
        }
    }
}
