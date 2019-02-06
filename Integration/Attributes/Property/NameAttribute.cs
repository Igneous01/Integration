using Integration.Interfaces;
using System;

namespace Integration.Attributes.Property
{
    [AttributeUsage(AttributeTargets.Property)]
    public class NameAttribute : Attribute, ICommonAttributeMarker
    {
        public readonly string _name;
        public string Name { get { return _name; } }

        public NameAttribute(string Name)
        {
            _name = Name;
        }
    }
}
