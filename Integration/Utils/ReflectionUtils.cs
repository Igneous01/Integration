using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Integration.Utils
{
    public class ReflectionUtils
    {
        public static T CreateInstance<T>() where T : new()
        {
            return new T();
        }

        public static object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }

        public static PropertyInfo[] GetPublicProperties(Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Instance | BindingFlags.Static);
        }

        public static Attribute[] GetCustomAttributes<T>()
        {
            return GetCustomAttributes<T, Attribute>();
        }

        public static Attribute[] GetCustomAttributes(Type type)
        {
            return GetCustomAttributes<Attribute>(type);
        }

        public static Attribute[] GetCustomAttributes(PropertyInfo property)
        {
            return GetCustomAttributes<Attribute>(property);
        }

        public static AttributeType[] GetCustomAttributes<AttributeType>(PropertyInfo property)
        {
            object[] attributes = property.GetCustomAttributes(typeof(AttributeType), true);
            return (AttributeType[])Convert.ChangeType(attributes, typeof(AttributeType[]));
        }

        public static Attribute[] GetCustomAttributes(PropertyInfo[] properties)
        {
            return GetCustomAttributes<Attribute>(properties);
        }

        public static AttributeType[] GetCustomAttributes<AttributeType>(PropertyInfo[] properties)
        {
            IEnumerable<AttributeType> results = properties
                                            .SelectMany((PropertyInfo property) => GetCustomAttributes<AttributeType>(property));
            return results.ToArray();
        }

        public static AttributeType[] GetCustomAttributes<T,AttributeType>()
        {
            object[] attributes = typeof(T).GetCustomAttributes(typeof(AttributeType), true);
            return (AttributeType[])Convert.ChangeType(attributes, typeof(AttributeType[]));
        }

        public static AttributeType[] GetCustomAttributes<AttributeType>(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof(AttributeType), true);
            return (AttributeType[])Convert.ChangeType(attributes, typeof(AttributeType[]));
        }

        public static object[] GetCustomAttributes(Type type, PropertyInfo property)
        {
            return property.GetCustomAttributes(type, true);
        }

        public static AttributeType[] GetCustomAttributesForAllProperties<T, AttributeType>()
        {
            return GetCustomAttributesForAllProperties<AttributeType>(typeof(T));
        }

        public static T[] GetCustomAttributesForAllProperties<T>(Type type)
        {
            IEnumerable<T> results = type.GetProperties(GetPublicBindingFlags())
                                            .SelectMany((PropertyInfo property) => GetCustomAttributes<T>(property));
            return results.ToArray();
        }

        public static bool IsAssignableToGenericType(Type givenType, Type genericType)
        {
            var interfaceTypes = givenType.GetInterfaces();

            foreach (var it in interfaceTypes)
            {
                if (it.IsGenericType && it.GetGenericTypeDefinition() == genericType)
                    return true;
            }

            if (givenType.IsGenericType && givenType.GetGenericTypeDefinition() == genericType)
                return true;

            Type baseType = givenType.BaseType;
            if (baseType == null) return false;

            return IsAssignableToGenericType(baseType, genericType);
        }

        public static bool IsAssignableToGenericType<T>(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(T);
        }

        public static bool HasCustomAttributeType<AttributeType>(PropertyInfo property)
        {
            return GetCustomAttributes<AttributeType>(property).Length > 0;
        }

        public static bool HasCustomAttributeType(Type type, PropertyInfo property)
        {
            return GetCustomAttributes(type, property).Length > 0;
        }

        public static bool HasCustomAttributeType<AttributeType>(Type type)
        {
            return GetCustomAttributes<AttributeType>(type).Length > 0;
        }

        public static BindingFlags GetPublicBindingFlags()
        {
            return BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy;
        }
    }
}
