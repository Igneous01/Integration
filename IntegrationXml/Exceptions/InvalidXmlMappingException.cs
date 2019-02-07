using Integration.Interfaces;
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Integration.Xml.Exceptions
{
    public class InvalidXmlMappingException : Exception
    {
        public InvalidXmlMappingException()
        {
        }

        public InvalidXmlMappingException(string message) : base(message)
        {
        }

        public InvalidXmlMappingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidXmlMappingException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static InvalidXmlMappingException Create(Type parentType, IFieldPropertyInfo property, string reason)
        {
            string message = $"{parentType.FullName}.{property.Name} cannot be mapped - {reason}";
            return new InvalidXmlMappingException(message);
        }
    }
}
