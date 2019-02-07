using Integration.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Common
{
    public class FieldPropertyInfoFactory
    {
        private static FieldPropertyInfoFactory _instance = new FieldPropertyInfoFactory();
        public static FieldPropertyInfoFactory Instance { get { return _instance; } }

        private FieldPropertyInfoFactory()
        {
        }

        public IFieldPropertyInfo Create(PropertyInfo property)
        {
            return new PropertyInfoImpl(property);
        }

        public IFieldPropertyInfo Create(FieldInfo field)
        {
            return new FieldInfoImpl(field);
        }
    }
}
