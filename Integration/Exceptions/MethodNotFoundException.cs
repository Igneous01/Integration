using System;
using System.Runtime.Serialization;

namespace Integration.Exceptions
{
    [Serializable]
    public class MethodNotFoundException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    https://msdn.microsoft.com/en-us/library/ms229064(v=vs.100).aspx
        //

        public MethodNotFoundException()
        {
        }

        public MethodNotFoundException(string message) : base(message)
        {
        }

        public MethodNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }

        protected MethodNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public static MethodNotFoundException CreateInstance(Type implementedType, string interfaceType, string methodName)
        {
            string text = "Type " + implementedType.AssemblyQualifiedName + " does not implement the interface " + interfaceType + " - method " + methodName + " not found, or the type has explicitly implemented the interface method " + methodName + " which is not supported";
            return new MethodNotFoundException(text);
        }
    }
}
