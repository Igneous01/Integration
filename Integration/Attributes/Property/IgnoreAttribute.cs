using Integration.Interfaces;
using System;


namespace Integration.Attributes.Property
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute, ICommonAttributeMarker
    {
    }
}
