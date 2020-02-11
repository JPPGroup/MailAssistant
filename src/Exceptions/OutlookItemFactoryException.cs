using System;
using System.Runtime.Serialization;

namespace Jpp.AddIn.MailAssistant.Exceptions
{
    [Serializable]
    internal class OutlookItemFactoryException : ArgumentException
    {
        protected OutlookItemFactoryException() : base() { }
        protected OutlookItemFactoryException(string message) : base(message) { }
        protected OutlookItemFactoryException(string message, Exception innerException) : base(message, innerException) { }
        protected OutlookItemFactoryException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext) { }

        public OutlookItemFactoryException(string message, string paramName) : base(message, paramName) { }
    }
}
