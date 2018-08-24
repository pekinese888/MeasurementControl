using NationalInstruments.Visa;
using Ivi.Visa;
using System;

namespace EnergyMeasurementCLI.Instruments
{
    public abstract class PowerMeter : IDisposable
    {
        public PowerMeter(string resourceName)
        {
            Console.WriteLine(this.GetType());
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
        }

        public PowerMeter(string resourceName, AccessModes accessModes, int timeoutMilliseconds)
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
        }

        ~PowerMeter()
        {
            if (_session != null)
            {
                _session.Dispose();
            }

        }

        private static bool TryToOpenSession(string resourceName)
        {
            ResourceOpenStatus status;
            try
            {
                GlobalResourceManager.Open(resourceName, AccessModes.None, 0, out status).Dispose();
            }
            catch (NativeVisaException visaException)
            {
                switch (visaException.ErrorCode)
                {
                    case NativeErrorCode.ResourceNotFound:
                        Console.WriteLine("Resource not found!");
                        status = ResourceOpenStatus.Unknown;
                        break;
                    default:
                        Console.WriteLine("Something went wrong!");
                        Console.WriteLine($"Errorcode: {visaException.ErrorCode}");
                        Console.WriteLine($"Message: {visaException.Message}");
                        Console.WriteLine(visaException.StackTrace);
                        status = ResourceOpenStatus.Unknown;
                        break;
                }
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

        public void Dispose()
        {
            _session.Dispose();
        }

        protected readonly Session _session;
    }
}