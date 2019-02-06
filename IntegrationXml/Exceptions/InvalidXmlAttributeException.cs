using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace Integration.Xml.Exceptions
{
    class InvalidXmlAttributeException : Exception
    {
        public InvalidXmlAttributeException()
        {
        }

        public InvalidXmlAttributeException(string message) : base(message)
        {
        }

        public InvalidXmlAttributeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidXmlAttributeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static InvalidXmlAttributeException Create(Type type, PropertyInfo property, Type attribute, string customMessage)
        {
            string message = $"{type.FullName}.{property.Name} has an invalid attribute {attribute.FullName} " +
                $"applied based on its type. Reason: {customMessage} Assembly: {type.AssemblyQualifiedName}";
            return new InvalidXmlAttributeException(message);
        }
    }
}
