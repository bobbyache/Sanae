using System;
using System.Runtime.Serialization;

namespace CygSoft.Sanae.Index
{
    [Serializable()]
    public class InvalidFileIndexVersionException : Exception, ISerializable
    {
        public InvalidFileIndexVersionException() : base() { }
        public InvalidFileIndexVersionException(string message) : base(message) { }
        public InvalidFileIndexVersionException(string message, System.Exception inner) : base(message, inner) { }
        public InvalidFileIndexVersionException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}