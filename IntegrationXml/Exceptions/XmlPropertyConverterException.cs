using System;
using System.Runtime.Serialization;

namespace Integration.Xml.Exceptions
{
    public class XmlPropertyConverterException : Exception
    {
        public XmlPropertyConverterException()
        {
        }

        public XmlPropertyConverterException(string message) : base(message)
        {
        }

        public XmlPropertyConverterException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected XmlPropertyConverterException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
