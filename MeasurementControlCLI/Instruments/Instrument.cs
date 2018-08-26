using NationalInstruments.Visa;
using Ivi.Visa;
using System;

namespace MeasurementControlCLI.Instruments
{
    /// <summary>
    /// General Class for all Instruments
    /// </summary>
    public class Instrument : IDisposable
    {
        public Instrument(string resourceName)
        {

            if (CheckResourceCLass(resourceName))
            {
                if (TryToOpenSession(resourceName))
                {
                    switch (GlobalResourceManager.Parse(resourceName).InterfaceType)
                    {
                        case HardwareInterfaceType.Custom:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Gpib:
                            this._session = new GpibSession(resourceName);
                            break;
                        case HardwareInterfaceType.Vxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.GpibVxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Serial:
                            this._session = new SerialSession(resourceName);
                            break;
                        case HardwareInterfaceType.Pxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Tcp:
                            this._session = new TcpipSession(resourceName);
                            break;
                        case HardwareInterfaceType.Usb:
                            this._session = new UsbSession(resourceName);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new CannotConnectInstrumentException($"Unable to Open Session with Instrument!");
                }
            }
            else
            {
                throw new ArgumentException($"Wrong Resource Class! Expected INSTR.");
            }
        }

        public Instrument(string resourceName, AccessModes accessModes, int timeoutMilliseconds)
        {

            if (CheckResourceCLass(resourceName))
            {
                if (TryToOpenSession(resourceName))
                {
                    switch (GlobalResourceManager.Parse(resourceName).InterfaceType)
                    {
                        case HardwareInterfaceType.Custom:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Gpib:
                            this._session = new GpibSession(resourceName, accessModes, timeoutMilliseconds);
                            break;
                        case HardwareInterfaceType.Vxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.GpibVxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Serial:
                            this._session = new SerialSession(resourceName, accessModes, timeoutMilliseconds);
                            break;
                        case HardwareInterfaceType.Pxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Tcp:
                            this._session = new TcpipSession(resourceName, accessModes, timeoutMilliseconds);
                            break;
                        case HardwareInterfaceType.Usb:
                            this._session = new UsbSession(resourceName, accessModes, timeoutMilliseconds);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new CannotConnectInstrumentException($"Unable to Open Session with Instrument!");
                }
            }
            else
            {
                throw new ArgumentException($"Wrong Resource Class! Expected INSTR but got {GlobalResourceManager.Parse(resourceName).ResourceClass}");
            }
        }

        private static bool CheckResourceCLass(string resourceName)
        {
            try
            {
                if (GlobalResourceManager.Parse(resourceName).ResourceClass == "INSTR") return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        private static bool TryToOpenSession(string resourceName)
        {
            ResourceOpenStatus status;
            try
            {
                GlobalResourceManager.Open(resourceName, AccessModes.None, 0, out status).Dispose();
            }
            catch (Exception)
            {
                status = ResourceOpenStatus.Unknown;
            }
            if (status == ResourceOpenStatus.Success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        ~Instrument()
        {
            if (_session != null)
            {
                _session.Dispose();
            }

        }

        public void Dispose()
        {
            if (_session != null)
            {
                _session.Dispose();
            }
        }

        public abstract class MeasurementParameter
        {
            public new string ToString { get; protected set; }
            public string Description { get; protected set; }

            protected MeasurementParameter(string toString, string description)
            {
                ToString = toString;
                Description = description;
            }
        }

        protected readonly Session _session;
    }
}


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


