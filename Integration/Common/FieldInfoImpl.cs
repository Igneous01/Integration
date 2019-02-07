using Integration.Enums;
using Integration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Common
{
    public class FieldInfoImpl : IFieldPropertyInfo
    {
        private readonly FieldInfo _field;

        public FieldInfoImpl(FieldInfo field)
        {
            _field = field;
        }

        public MemberTypes MemberType => _field.MemberType;

        public string Name => _field.Name;

        public Type DeclaringType => _field.DeclaringType;

        public Type ReflectedType => _field.ReflectedType;

        public int MetadataToken => _field.MetadataToken;

        public Module Module => _field.Module;

        public IEnumerable<CustomAttributeData> CustomAttributes => _field.CustomAttributes;

        public object[] GetCustomAttributes(bool inherit) => _field.GetCustomAttributes(inherit);

        public object[] GetCustomAttributes(Type attributeType, bool inherit) => _field.GetCustomAttributes(attributeType, inherit);

        public IList<CustomAttributeData> GetCustomAttributesData() => _field.GetCustomAttributesData();

        public bool IsDefined(Type attributeType, bool inherit) => _field.IsDefined(attributeType, inherit);

        public Type Type => _field.FieldType;

        public object GetValue(object obj) => _field.GetValue(obj);

        public void SetValue(object obj, object value) => _field.SetValue(obj, value);

        public object WrappedInstance => _field;

        public PropertyFieldType WrappedType => PropertyFieldType.FIELD_INFO;
    }
}
