using Integration.Cache.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Interfaces
{
    public interface IPropertyAttributeValidator
    {
        void Validate(PropertyInfo property, Type parentType, AbstractAttributeContext context);
    }
}
