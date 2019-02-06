﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Utils
{
    public static class TypeExtensions
    {
        public static bool IsStruct(this Type source)
        {
            return source.IsValueType && !source.IsPrimitive && !source.IsEnum;
        }

        public static bool IsEnumerable(this Type source)
        {
            return typeof(IEnumerable).IsAssignableFrom(source);
        }

        public static bool IsDictionary(this Type source)
        {
            return typeof(IDictionary).IsAssignableFrom(source);
        }

        
    }
}