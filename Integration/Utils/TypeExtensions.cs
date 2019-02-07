using Integration.Common;
using Integration.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Utils
{
    public static class TypeExtensions
    {
        public static bool IsStruct(this Type source)
        {
            return source.IsValueType && !source.IsPrimitive && !source.IsEnum;
        }

        public static bool IsCollection(this Type source)
        {
            return source.GetInterfaces()
                            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        public static bool IsString(this Type source)
        {
            return typeof(string).IsAssignableFrom(source);
        }

        public static bool IsDictionary(this Type source)
        {
            return typeof(IDictionary).IsAssignableFrom(source);
        }

        public static bool HasDefaultConstructor(this Type source)
        {
            return source.GetConstructor(Type.EmptyTypes) != null;
        }

        public static IFieldPropertyInfo[] GetFieldsAndProperties(this Type source, BindingFlags bindingFlags)
        {
            var fields = source.GetFields(bindingFlags)
                    .Select(property => FieldPropertyInfoFactory.Instance.Create(property))
                    .Union(source.GetProperties(bindingFlags)
                            .Select(property => FieldPropertyInfoFactory.Instance.Create(property)));
            return fields.ToArray();
        }
    }
}
