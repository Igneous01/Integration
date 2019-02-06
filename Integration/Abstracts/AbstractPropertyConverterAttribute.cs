using Integration.Exceptions;
using Integration.Interfaces;
using Integration.Utils;
using System;
using System.Reflection;

namespace Integration.Abstracts
{
    public class AbstractPropertyConverterAttribute : Attribute
    {
        public const string CONVERT_TO_SOURCE_METHOD_NAME = "ConvertToSourceType";
        public const string CONVERT_TO_DESTINATION_METHOD_NAME = "ConvertToDestinationType";
        public readonly Type _sourceType;
        public readonly Type _destinationType;
        public readonly Type _converterType;
        public readonly MethodInfo _convertToSourceMethod;
        public readonly MethodInfo _convertToDestinationMethod;
        public readonly object _converterInstance;

        public AbstractPropertyConverterAttribute(Type converter)
        {
            Type IPropertyConverter = typeof(IPropertyConverter<,>);
            if (!ReflectionUtils.IsAssignableToGenericType(converter, IPropertyConverter))
                throw new InterfaceNotImplementedException("Type " + converter.AssemblyQualifiedName + " does not implement interface " + IPropertyConverter.AssemblyQualifiedName);

            _converterType = converter;
            _convertToSourceMethod = _converterType.GetMethod(CONVERT_TO_SOURCE_METHOD_NAME, ReflectionUtils.GetPublicBindingFlags());
            _convertToDestinationMethod = _converterType.GetMethod(CONVERT_TO_DESTINATION_METHOD_NAME, ReflectionUtils.GetPublicBindingFlags());

            if (_convertToSourceMethod == null)
                throw MethodNotFoundException.CreateInstance(_converterType, IPropertyConverter.AssemblyQualifiedName, CONVERT_TO_SOURCE_METHOD_NAME);
            if (_convertToDestinationMethod == null)
                throw MethodNotFoundException.CreateInstance(_converterType, IPropertyConverter.AssemblyQualifiedName, CONVERT_TO_DESTINATION_METHOD_NAME);

            _sourceType = _convertToSourceMethod.ReturnType;
            _destinationType = _convertToDestinationMethod.ReturnType;

            _converterInstance = ReflectionUtils.CreateInstance(_converterType);
        }

        public object ConvertToSourceType(object value)
        {
            return _convertToSourceMethod.Invoke(_converterInstance, new object[] { value });
        }

        public object ConvertToDestinationType(object value)
        {
            return _convertToDestinationMethod.Invoke(_converterInstance, new object[] { value });
        }
    }
}
