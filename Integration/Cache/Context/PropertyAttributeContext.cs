using Integration.Attributes.Property;
using Integration.Interfaces;
using Integration.Utils;
using System.Collections;
using System.Reflection;

namespace Integration.Cache.Context
{
    public class PropertyAttributeContext : AbstractAttributeContext
    {
        public PropertyAttributeContext(IFieldPropertyInfo property)
            : base(property)
        {
            Property = property;
            PropertyConverterAttribute = GetAttribute<PropertyConverterAttribute>();
            NameAttribute = GetAttribute<NameAttribute>();

            HasPropertyConverterAttribute = PropertyConverterAttribute != null;
            HasNameAttribute = NameAttribute != null;
            HasIgnoreAttribute = ContainsAttribute<IgnoreAttribute>();        
        }

        public IFieldPropertyInfo Property { get; private set; }
        public PropertyConverterAttribute PropertyConverterAttribute { get; private set; }
        public NameAttribute NameAttribute { get; private set; }

        public bool HasPropertyConverterAttribute { get; private set; }
        public bool HasNameAttribute { get; private set; }
        public bool HasIgnoreAttribute { get; private set; }
    }
}
