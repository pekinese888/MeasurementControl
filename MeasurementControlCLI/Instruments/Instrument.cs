using NationalInstruments.Visa;
using Ivi.Visa;
using System;
using System.Collections.Generic;
using System.Collections;

namespace MeasurementControlCLI.Instruments
{
    /// <summary>
    /// General Class for all Instruments
    /// </summary>
    public abstract class Instrument : IDisposable
    {
        /// <summary>
        /// Constructor, opening a Session to the given NI-Visa Instrument.
        /// </summary>
        /// <param name="resourceName">The Visa Resource Name</param>
        public Instrument(string resourceName)
        {

            if (ResourceClassIsInstrument(resourceName))
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
                    throw new Instruments.Exceptions.CannotConnectInstrumentException($"Unable to Open Session with Instrument!");
                }
            }
            else
            {
                throw new ArgumentException($"Wrong Resource Class! Expected INSTR.");
            }
        }

        /// <summary>
        /// Constructor, opening a Session to the given NI-Visa Instrument.
        /// </summary>
        /// <param name="resourceName">The Visa Resource Name</param>
        /// <param name="accessModes">The modes by which the resource is to be accessed.</param>
        /// <param name="timeoutMilliseconds">The timeout in milliseconds. 
        /// This applies to the time taken to acquire the requested lock, 
        /// and may also include the time needed to open a session to the resource.</param>
        public Instrument(string resourceName, AccessModes accessModes, int timeoutMilliseconds)
        {

            if (ResourceClassIsInstrument(resourceName))
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
                    throw new Instruments.Exceptions.CannotConnectInstrumentException($"Unable to Open Session with Instrument!");
                }
            }
            else
            {
                throw new ArgumentException($"Wrong Resource Class! Expected INSTR but got {GlobalResourceManager.Parse(resourceName).ResourceClass}");
            }
        }

        /// <summary>
        /// Method to Check if the Visa Resource Name belongs to an Instrument
        /// </summary>
        /// <param name="resourceName">The Visa Resource Name</param>
        /// <returns>True if Resource Class in an Instrument, False Otherwise</returns>
        private static bool ResourceClassIsInstrument(string resourceName)
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

        /// <summary>
        /// Tries if Session is able to be opened
        /// </summary>
        /// <param name="resourceName">The Visa Resource Name</param>
        /// <returns>True if session can be opened</returns>
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

        /// <summary>
        /// Instrument Destructor, making sure the Session gets Disposed if the Instrument Object gets destroyed
        /// </summary>
        ~Instrument()
        {
            this.Dispose();

        }

        /// <summary>
        /// Implements iDisposable 
        /// </summary>
        public void Dispose()
        {
            if (_session != null)
            {
                _session.Dispose();
            }
        }

        protected readonly Session _session;
    }
}


