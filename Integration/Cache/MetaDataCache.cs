using Integration.Exceptions;
using Integration.Interfaces;
using Integration.Utils;
using System;
using System.Collections.Concurrent;

namespace Integration.Cache
{
    public class MetaDataCache
    {
        private static ConcurrentDictionary<Type, ClassMetaData> _classes = Initializer.Initialize();

        public static ClassMetaData Get<T>()
        {
            return Get(typeof(T));
        }

        public static ClassMetaData Get(Type type)
        {
            if (_classes.ContainsKey(type))
                return _classes[type];
            else
            {
                // TODO: Include search for class level attributes
                if (ReflectionUtils.GetCustomAttributesForAllProperties<IAttributeMarker>(type).Length > 0)
                {
                    if (!_classes.TryAdd(type, new ClassMetaData(type)))
                        throw new ReflectionCacheException("Type " + type.AssemblyQualifiedName + " could not be added to the cache");
                    else
                        return _classes[type];
                }
                else
                    throw new InvalidTypeException("Type " + type.AssemblyQualifiedName + " does not exist in the cache, and does not have any attributes applied to the class");
            }
        }

        public static bool Contains<T>()
        {
            return _classes.ContainsKey(typeof(T));
        }

        public static bool Contains(Type type)
        {
            return _classes.ContainsKey(type);
        }
    }
}
