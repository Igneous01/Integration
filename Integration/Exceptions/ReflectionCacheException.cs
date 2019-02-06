using System;
using System.Runtime.Serialization;

namespace Integration.Exceptions
{
    [Serializable]
    class ReflectionCacheException : Exception
    {
        //
        // For guidelines regarding the creation of new exception types, see
        //    https://msdn.microsoft.com/en-us/library/ms229064(v=vs.100).aspx
        //

        public ReflectionCacheException()
        {
        }

        public ReflectionCacheException(string message) : base(message)
        {
        }

        public ReflectionCacheException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ReflectionCacheException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
