using System;

namespace MeasurementControlCLI.Exceptions
{
    [Serializable]
    public class WrongInstrumentException : Exception
    {
        public WrongInstrumentException() { }
        public WrongInstrumentException(string message) : base(message) { }
        public WrongInstrumentException(string message, Exception inner) : base(message, inner) { }
        protected WrongInstrumentException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
