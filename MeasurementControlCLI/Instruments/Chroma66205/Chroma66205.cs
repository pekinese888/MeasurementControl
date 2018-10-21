using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.Visa;
using Ivi.Visa;

namespace MeasurementControlCLI.Instruments.Chroma66205
{
    /// <summary>
    /// Specific Implementation of the Chroma66205 Power Meter
    /// </summary>
    public class Chroma66205 : IDisposable
    {
        /// <summary>
        /// Initializes a new Instance of the Chroma66205 Class.
        /// </summary>
        /// <param name="resourceName">String that describes a unique VISA resource.</param>
        public Chroma66205(string resourceName)
        {
            if (ResourceClassIsInstrument(resourceName))
            {
                if (SessionCanBeOpened(resourceName))
                {
                    switch (GlobalResourceManager.Parse(resourceName).InterfaceType)
                    {
                        case HardwareInterfaceType.Custom:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Gpib:
                            _session = new GpibSession(resourceName);
                            break;
                        case HardwareInterfaceType.Vxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.GpibVxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Serial:
                            _session = new SerialSession(resourceName);
                            break;
                        case HardwareInterfaceType.Pxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Tcp:
                            _session = new TcpipSession(resourceName);
                            break;
                        case HardwareInterfaceType.Usb:
                            _session = new UsbSession(resourceName);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new Exceptions.CannotConnectInstrumentException($"Unable to Open Session with Instrument!");
                }
            }
            else
            {
                throw new ArgumentException($"Wrong Resource Class! Expected INSTR.");
            }
            if (IsCorrectInstrument())
            {
                InternalConfiguration = new _InternalConfiguration(this);
                Configuration = new _ExternalConfiguration(this);
                Status = new _Status(this);
                Initialize();
            }
            else
            {
                Dispose();
                throw new Exceptions.WrongInstrumentException("Expected session to Chroma 66205.");
            }
        }

        /// <summary>
        /// Initializes a new Instance of the Chroma66205 Class.
        /// </summary>
        /// <param name="resourceName">VISA String to the Resource</param>
        /// <param name="accessModes">The modes by which the resource is to be accessed.</param>
        /// <param name="timeoutMilliseconds">
        /// The maximum time in milliseconds that this method waits to open a VISA session
        /// with the specified resource. This parameter does not set the TimeoutMilliseconds Property of the Instance.
        /// </param>
        public Chroma66205(string resourceName, AccessModes accessModes, int timeoutMilliseconds)
        {
            if (ResourceClassIsInstrument(resourceName))
            {
                if (SessionCanBeOpened(resourceName))
                {
                    switch (GlobalResourceManager.Parse(resourceName).InterfaceType)
                    {
                        case HardwareInterfaceType.Custom:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Gpib:
                            _session = new GpibSession(resourceName, accessModes, timeoutMilliseconds);
                            break;
                        case HardwareInterfaceType.Vxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.GpibVxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Serial:
                            _session = new SerialSession(resourceName, accessModes, timeoutMilliseconds);
                            break;
                        case HardwareInterfaceType.Pxi:
                            throw new NotImplementedException();
                        case HardwareInterfaceType.Tcp:
                            _session = new TcpipSession(resourceName, accessModes, timeoutMilliseconds);
                            break;
                        case HardwareInterfaceType.Usb:
                            _session = new UsbSession(resourceName, accessModes, timeoutMilliseconds);
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
                else
                {
                    throw new Exceptions.CannotConnectInstrumentException($"Unable to Open Session with Instrument!");
                }
            }
            else
            {
                throw new ArgumentException($"Wrong Resource Class! Expected INSTR.");
            }
            if (IsCorrectInstrument())
            {
                InternalConfiguration = new _InternalConfiguration(this);
                Configuration = new _ExternalConfiguration(this);
                Status = new _Status(this);
                Initialize();
            }
            else
            {
                Dispose();
                throw new Exceptions.WrongInstrumentException("Expected session to Chroma 66205.");
            }
        }

        /// <summary>
        /// Method to Check if the Visa Resource Name belongs to an Instrument
        /// </summary>
        /// <param name="resourceName">The Visa Resource Name</param>
        /// <returns>True if Resource Class in an Instrument, False Otherwise</returns>
        private bool ResourceClassIsInstrument(string resourceName)
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
        private bool SessionCanBeOpened(string resourceName)
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
        /// Checks if base._session leads to a Chroma 66205 Powermeter
        /// </summary>
        /// <returns>True if the check is positive</returns>
        private bool IsCorrectInstrument()
        {
            string response;
            try
            {
                response = SendCommand("*IDN?", true);
                if ((response.Split(',')[0] == "Chroma ATE") && (response.Split(',')[1] == "66205"))
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw new Exceptions.CommunicationException("Getting Identification Information from Instrument failed!");
            }
        }

        /// <summary>
        /// Instrument Destructor, making sure the Session gets Disposed if the Instrument Object gets destroyed
        /// </summary>
        ~Chroma66205()
        {
            Dispose();

        }

        /// <summary>
        /// Implements iDisposable 
        /// </summary>
        public void Dispose()
        {
            if (_session != null)
            {
                Initialize();
                _session.Dispose();
            }
        }

        /// <summary>
        /// Initialize the Chroma to Factory Defaults, set Transmit and Termination Characters.
        /// </summary>
        public void Initialize()
        {
            Reset();
            Configuration.RestoreConfig(Instruments.Chroma66205.Configuration.RestoreConfig_AllowedValue.FactoryDefaults);
            InternalConfiguration.TransmitSeperator.Value = Instruments.Chroma66205.InternalConfiguration.TransmitSeperator_AllowedValue.Comma;
            InternalConfiguration.TransmitTerminator.Value = Instruments.Chroma66205.InternalConfiguration.TransmitTerminator_AllowedValue.LineFeed;
        }

        /// <summary>
        /// This command performs device initial setting.
        /// </summary>
        private void Reset()
        {
            SendCommand("*RST", true);
        }

        /// <summary>
        /// Sends Command to the Chroma66205. If readResponse is true it provides the Response as String and Clears the ReadBuffer.
        /// </summary>
        /// <param name="command">Command to be sent to the Chroma66205. Refer to Chroma66205 Manual for more information</param>
        /// <param name="readResponse">If true, the Response is provided as String and the ReadBuffer is cleared. If false, the return will be null and ReadBuffer will be untouched.</param>
        /// <returns></returns>
        private string SendCommand(string command, bool readResponse)
        {
            _session.FormattedIO.Write(command + InternalConfiguration.TransmitTerminator.Value.MessageBasedSessionRepresentation);
            _session.FormattedIO.FlushWrite(_session.SendEndEnabled);

            if (readResponse)
            {
                string response = _session.FormattedIO.ReadLine();
                _session.FormattedIO.DiscardBuffers();
                return response;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// This command requests execution of, and queries the result of self-test.
        /// </summary>
        public int RunSelfTest()
        {
            return Convert.ToInt32(SendCommand("TST?", true));
        }

        /// <summary>
        /// This command lets the user get measurement data from the Power Meter. Measure triggers the acquisition of new data before returning data.
        /// </summary>
        /// <param name="measurementParameters">Paremeters to be measured.</param>
        /// <returns>Dictionary which contains PowerMeter.MeasurementParameter as Key Value and the actual measurement value as value pair.</returns>
        public Dictionary<Measure_AllowedValue, double> Measure(params Measure_AllowedValue[] measurementParameters)
        {
            Dictionary<Measure_AllowedValue, double> measurements = new Dictionary<Measure_AllowedValue, double>();
            string message = "MEAS?";
            double[] values;


            foreach (Measure_AllowedValue parameter in measurementParameters)
            {
                message = message + parameter.MessageBasedSessionRepresentation + InternalConfiguration.TransmitSeperator.Value.MessageBasedSessionRepresentation;
            }

            SendCommand(message, false);

            values = _session.FormattedIO.ReadListOfDouble(measurementParameters.Length);

            int i = 0;
            foreach (Measure_AllowedValue parameter in measurementParameters)
            {
                measurements.Add(parameter, values[i]);
                i++;
            }

            return measurements;
        }

        /// <summary>
        /// This command lets the user get measurement data from the Power Meter. Fetch returns the previously acquired data from the measurement buffer.
        /// </summary>
        /// <param name="measurementParameters">Paremeters to be measured.</param>
        /// <returns>Dictionary which contains PowerMeter.MeasurementParameter as Key Value and the actual measurement value as value pair.</returns>
        public Dictionary<Fetch_AllowedValue, double> Fetch(params Fetch_AllowedValue[] measurementParameters)
        {
            Dictionary<Fetch_AllowedValue, double> measurements = new Dictionary<Fetch_AllowedValue, double>();
            string message = "FETC?";
            double[] values;


            foreach (Fetch_AllowedValue parameter in measurementParameters)
            {
                message = message + parameter.MessageBasedSessionRepresentation + InternalConfiguration.TransmitSeperator.Value.MessageBasedSessionRepresentation;
            }

            SendCommand(message, false);

            values = _session.FormattedIO.ReadListOfDouble(measurementParameters.Length);

            int i = 0;
            foreach (Fetch_AllowedValue parameter in measurementParameters)
            {
                measurements.Add(parameter, values[i]);
                i++;
            }

            return measurements;
        }

        /// <summary>
        /// Contains all Configuration Settings of the Chroma 66205
        /// </summary>
        private class _InternalConfiguration
        {
            private Chroma66205 _chroma66205;

            public _InternalConfiguration(Chroma66205 chroma66205)
            {
                _chroma66205 = chroma66205;
                Header = new _Header(this);
                TransmitSeperator = new _TransmitSeperator(this);
                TransmitTerminator = new _TransmitTerminator(this);
            }

            /// <summary>
            /// This command turns response headers ON or OFF. The default is OFF.
            /// </summary>
            public class _Header
            {
                private _InternalConfiguration _internalConfiguration;

                public _Header(_InternalConfiguration internalConfiguration)
                {
                    _internalConfiguration = internalConfiguration;
                }

                /// <summary>
                /// Actual Value of the Propertie.
                /// </summary>
                private InternalConfiguration.Header_AllowedValue _Value = null;
                public InternalConfiguration.Header_AllowedValue Value
                {
                    get
                    {
                        if (_Value == null)
                        {
                            switch (_internalConfiguration._chroma66205.SendCommand("SYST:HEAD?", true))
                            {
                                case "ON":
                                    _Value = Instruments.Chroma66205.InternalConfiguration.Header_AllowedValue.ON;
                                    return Instruments.Chroma66205.InternalConfiguration.Header_AllowedValue.ON;
                                case "OFF":
                                    _Value = Instruments.Chroma66205.InternalConfiguration.Header_AllowedValue.OFF;
                                    return Instruments.Chroma66205.InternalConfiguration.Header_AllowedValue.OFF;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            return _Value;
                        }
                    }
                    set
                    {
                        _internalConfiguration._chroma66205.SendCommand($"SYST:HEAD {value.MessageBasedSessionRepresentation}", true);
                        switch (_internalConfiguration._chroma66205.SendCommand("SYST:HEAD?", true))
                        {
                            case "ON":
                                _Value = Instruments.Chroma66205.InternalConfiguration.Header_AllowedValue.ON;
                                break;
                            case "OFF":
                                _Value = Instruments.Chroma66205.InternalConfiguration.Header_AllowedValue.OFF;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }


                /// <summary>
                /// Allowed values for the Propertie.
                /// </summary>
                public class AllowedValue
                {
                    private readonly string _value;

                    public new string ToString()
                    {
                        return _value;
                    }

                    private AllowedValue(string value)
                    {
                        _value = value;
                    }

                }
            }
            public _Header Header { get; }

            /// <summary>
            /// This command sets the message unit seperator for response messages.
            /// </summary>
            public class _TransmitSeperator
            {
                private _InternalConfiguration _internalConfiguration;

                public _TransmitSeperator(_InternalConfiguration internalConfiguration)
                {
                    _internalConfiguration = internalConfiguration;
                }

                /// <summary>
                /// Actual Value of the Propertie.
                /// </summary>
                private InternalConfiguration.TransmitSeperator_AllowedValue _Value = null;
                public InternalConfiguration.TransmitSeperator_AllowedValue Value
                {
                    get
                    {
                        if (_Value == null)
                        {
                            switch (_internalConfiguration._chroma66205.SendCommand("SYST:TRAN:SEP?", true))
                            {
                                case "0":
                                    _Value = Instruments.Chroma66205.InternalConfiguration.TransmitSeperator_AllowedValue.Comma;
                                    return Instruments.Chroma66205.InternalConfiguration.TransmitSeperator_AllowedValue.Comma;
                                case "1":
                                    _Value = Instruments.Chroma66205.InternalConfiguration.TransmitSeperator_AllowedValue.Semicolon;
                                    return Instruments.Chroma66205.InternalConfiguration.TransmitSeperator_AllowedValue.Semicolon;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            return _Value;
                        }
                    }
                    set
                    {


                        _internalConfiguration._chroma66205.SendCommand($"SYST:TRAN:SEP {value.MessageBasedSessionRepresentation}", true);

                        switch (_internalConfiguration._chroma66205.SendCommand("SYST:TRAN:SEP?", true))
                        {
                            case "0":
                                _Value = Instruments.Chroma66205.InternalConfiguration.TransmitSeperator_AllowedValue.Comma;
                                break;
                            case "1":
                                _Value = Instruments.Chroma66205.InternalConfiguration.TransmitSeperator_AllowedValue.Semicolon;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
            public _TransmitSeperator TransmitSeperator { get; }

            /// <summary>
            /// This command sets the data terminator for response messages. The default is Linefeed.
            /// </summary>
            public class _TransmitTerminator
            {
                private _InternalConfiguration _internalConfiguration;

                public _TransmitTerminator(_InternalConfiguration internalConfiguration)
                {
                    _internalConfiguration = internalConfiguration;
                }

                /// <summary>
                /// Actual Value of the Propertie.
                /// </summary>
                protected InternalConfiguration.TransmitTerminator_AllowedValue _Value = null;
                public InternalConfiguration.TransmitTerminator_AllowedValue Value
                {
                    get
                    {
                        if (_Value == null)
                        {
                            switch (_internalConfiguration._chroma66205.SendCommand("SYST:TRAN:TERM?", true))
                            {
                                case "0":
                                    _Value = Instruments.Chroma66205.InternalConfiguration.TransmitTerminator_AllowedValue.LineFeed;
                                    return Instruments.Chroma66205.InternalConfiguration.TransmitTerminator_AllowedValue.LineFeed;
                                case "1":
                                    _Value = Instruments.Chroma66205.InternalConfiguration.TransmitTerminator_AllowedValue.LineFeedAndCarriageReturn;
                                    return Instruments.Chroma66205.InternalConfiguration.TransmitTerminator_AllowedValue.LineFeedAndCarriageReturn;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                        else
                        {
                            return _Value;
                        }
                    }
                    set
                    {
                        _internalConfiguration._chroma66205.SendCommand($"SYST:TRAN:SEP {value.MessageBasedSessionRepresentation}", true);
                        switch (_internalConfiguration._chroma66205.SendCommand("SYST:TRAN:SEP?", true))
                        {
                            case "0":
                                _Value = Instruments.Chroma66205.InternalConfiguration.TransmitTerminator_AllowedValue.LineFeed;
                                break;
                            case "1":
                                _Value = Instruments.Chroma66205.InternalConfiguration.TransmitTerminator_AllowedValue.LineFeedAndCarriageReturn;
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                }
            }
            public _TransmitTerminator TransmitTerminator { get; }
        }
        private _InternalConfiguration InternalConfiguration { get; }

        public class _ExternalConfiguration
        {
            private Chroma66205 _chroma66205;
            public _ExternalConfiguration(Chroma66205 chroma66205)
            {
                _chroma66205 = chroma66205;
            }

            /// <summary>
            /// This command stores the present state of the configuration in a specific memory location.
            /// </summary>
            public void SaveConfig(Configuration.SaveConfig_AllowedValue config)
            {
                _chroma66205.SendCommand($"*SAV {config.MessageBasedSessionRepresentation}", true);
            }

            /// <summary>
            /// This command restores the power meter to a state that was previously stored in memory with the SaveConfig Command.
            /// </summary>
            public void RestoreConfig(Configuration.RestoreConfig_AllowedValue config)
            {
                _chroma66205.SendCommand($"*RCL {config.MessageBasedSessionRepresentation}", true);
            }

            /// <summary>
            /// THis command sets the Power Meter in Local State and the Front Panel will work.
            /// </summary>
            public void SetLocal()
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// THis command sets the Power Meter in Remote State and the Front Panel will be disabled except for the SETUP Key pressed.
            /// </summary>
            public void SetRemote()
            {
                throw new NotImplementedException();
            }

        }
        public _ExternalConfiguration Configuration { get; }

        /// <summary>
        /// Contains all Status Informations of the Chroma 66205
        /// </summary>
        public struct _Status
        {
            Chroma66205 _chroma66205;

            public _Status(Chroma66205 chroma66205)
            {
                _chroma66205 = chroma66205;
            }

            /// <summary>
            /// This Command queries the Error Status of the Instrument.
            /// </summary>
            public string Error
            {
                get
                {
                    return _chroma66205.SendCommand("SYST:ERR?", true);
                }
            }

            /// <summary>
            /// This command queries manufacturer´s name, model name, serial number and firmware version
            /// </summary>
            public string Identification
            {
                get
                {
                    return _chroma66205.SendCommand("*IDN?", true);
                }
            }

            /// <summary>
            /// This query returns a Floatingpoint Representation of the SCPI Version number for which the instrument complies.
            /// </summary>
            public double Version
            {
                get
                {
                    return Convert.ToDouble(_chroma66205.SendCommand("SYST:VER?", true));
                }
            }
        }
        public _Status Status { get; }

        /// <summary>
        /// Internal representation of the Messagebased Visa Session
        /// </summary>
        private MessageBasedSession _session;
    }
}