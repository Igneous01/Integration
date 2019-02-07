using Integration.Interfaces;
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

        public static Attribute[] GetCustomAttributes(FieldInfo field)
        {
            return GetCustomAttributes<Attribute>(field);
        }

        public static Attribute[] GetCustomAttributes(IFieldPropertyInfo fieldPropertyInfo)
        {
            return GetCustomAttributes<Attribute>(fieldPropertyInfo);
        }

        public static AttributeType[] GetCustomAttributes<AttributeType>(PropertyInfo property)
        {
            object[] attributes = property.GetCustomAttributes(typeof(AttributeType), true);
            return (AttributeType[])Convert.ChangeType(attributes, typeof(AttributeType[]));
        }

        public static AttributeType[] GetCustomAttributes<AttributeType>(FieldInfo field)
        {
            object[] attributes = field.GetCustomAttributes(typeof(AttributeType), true);
            return (AttributeType[])Convert.ChangeType(attributes, typeof(AttributeType[]));
        }

        public static AttributeType[] GetCustomAttributes<AttributeType>(IFieldPropertyInfo fieldPropertyInfo)
        {
            object[] attributes = fieldPropertyInfo.GetCustomAttributes(typeof(AttributeType), true);
            return (AttributeType[])Convert.ChangeType(attributes, typeof(AttributeType[]));
        }

        public static Attribute[] GetCustomAttributes(PropertyInfo[] properties)
        {
            return GetCustomAttributes<Attribute>(properties);
        }

        public static Attribute[] GetCustomAttributes(FieldInfo[] fields)
        {
            return GetCustomAttributes<Attribute>(fields);
        }

        public static AttributeType[] GetCustomAttributes<AttributeType>(PropertyInfo[] properties)
        {
            IEnumerable<AttributeType> results = properties
                                            .SelectMany((PropertyInfo property) => GetCustomAttributes<AttributeType>(property));
            return results.ToArray();
        }

        public static AttributeType[] GetCustomAttributes<AttributeType>(FieldInfo[] fields)
        {
            IEnumerable<AttributeType> results = fields
                                            .SelectMany((FieldInfo field) => GetCustomAttributes<AttributeType>(field));
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

        public static object[] GetCustomAttributes(Type type, FieldInfo field)
        {
            return field.GetCustomAttributes(type, true);
        }

        public static object[] GetCustomAttributes(Type type, IFieldPropertyInfo fieldPropertyInfo)
        {
            return fieldPropertyInfo.GetCustomAttributes(type, true);
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

        public static bool HasCustomAttributeType<AttributeType>(IFieldPropertyInfo fieldPropertyInfo)
        {
            return GetCustomAttributes<AttributeType>(fieldPropertyInfo).Length > 0;
        }

        public static bool HasCustomAttributeType<AttributeType>(FieldInfo field)
        {
            return GetCustomAttributes<AttributeType>(field).Length > 0;
        }

        public static bool HasCustomAttributeType<AttributeType>(PropertyInfo property)
        {
            return GetCustomAttributes<AttributeType>(property).Length > 0;
        }

        public static bool HasCustomAttributeType(Type type, PropertyInfo property)
        {
            return GetCustomAttributes(type, property).Length > 0;
        }

        public static bool HasCustomAttributeType(Type type, FieldInfo field)
        {
            return GetCustomAttributes(type, field).Length > 0;
        }

        public static bool HasCustomAttributeType(Type type, IFieldPropertyInfo fieldPropertyInfo)
        {
            return GetCustomAttributes(type, fieldPropertyInfo).Length > 0;
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
