using Integration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Integration.Enums;

namespace Integration.Common
{
    public class PropertyInfoImpl : IFieldPropertyInfo
    {
        private readonly PropertyInfo _property;

        public PropertyInfoImpl(PropertyInfo property)
        {
            _property = property;
        }

        public MemberTypes MemberType => _property.MemberType;

        public string Name => _property.Name;

        public Type DeclaringType => _property.DeclaringType;

        public Type ReflectedType => _property.ReflectedType;       

        public int MetadataToken => _property.MetadataToken;

        public Module Module => _property.Module;

        public IEnumerable<CustomAttributeData> CustomAttributes => _property.CustomAttributes;

        public object[] GetCustomAttributes(bool inherit) => _property.GetCustomAttributes(inherit);

        public object[] GetCustomAttributes(Type attributeType, bool inherit) => _property.GetCustomAttributes(attributeType, inherit);

        public IList<CustomAttributeData> GetCustomAttributesData() => _property.GetCustomAttributesData();   

        public bool IsDefined(Type attributeType, bool inherit) => _property.IsDefined(attributeType, inherit);

        public Type Type => _property.PropertyType;

        public object GetValue(object obj) => _property.GetValue(obj);

        public void SetValue(object obj, object value) => _property.SetValue(obj, value);

        public object WrappedInstance => _property;

        public PropertyFieldType WrappedType => PropertyFieldType.PROPERTY_INFO;
    }
}
