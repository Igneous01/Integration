using Integration.Interfaces;
using System;


namespace Integration.Attributes.Property
{
    [AttributeUsage(AttributeTargets.Property)]
    public class IgnoreAttribute : Attribute, ICommonAttributeMarker
    {
    }
}
