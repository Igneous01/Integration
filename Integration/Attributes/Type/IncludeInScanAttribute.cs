using Integration.Interfaces;
using System;

namespace Integration.Attributes.Type
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct)]
    public class IncludeInScanAttribute : Attribute, IAttributeMarker
    {
    }
}
