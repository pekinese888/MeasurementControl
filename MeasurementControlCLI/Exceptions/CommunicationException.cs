using System;

namespace MeasurementControlCLI.Exceptions
{
    [Serializable]
    public class CommunicationException : Exception
    {
        public CommunicationException() { }
        public CommunicationException(string message) : base(message) { }
        public CommunicationException(string message, Exception inner) : base(message, inner) { }
        protected CommunicationException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
