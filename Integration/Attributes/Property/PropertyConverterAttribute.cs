using Integration.Abstracts;
using Integration.Interfaces;
using System;

namespace Integration.Attributes.Property
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class PropertyConverterAttribute : AbstractPropertyConverterAttribute, ICommonAttributeMarker
    {
        public PropertyConverterAttribute(System.Type converter) : base(converter)
        {
        }
    }
}
