using System;

namespace MeasurementControlCLI.Exceptions
{
    [Serializable]
    public class CannotConnectInstrumentException : Exception
    {
        public CannotConnectInstrumentException() { }
        public CannotConnectInstrumentException(string message) : base(message) { }
        public CannotConnectInstrumentException(string message, Exception inner) : base(message, inner) { }
        protected CannotConnectInstrumentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
