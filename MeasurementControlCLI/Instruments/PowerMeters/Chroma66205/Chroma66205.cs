using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.Visa;
using Ivi.Visa;

namespace MeasurementControlCLI.Instruments.PowerMeters.Chroma66205
{
    /// <summary>
    /// Specific Implementation of the Chroma66205 Power Meter
    /// </summary>
    public sealed class Chroma66205 : PowerMeter
    {
        /// <summary>
        /// Initializes a new Instance of the Chroma66205 Class.
        /// </summary>
        /// <param name="resourceName">String that describes a unique VISA resource.</param>
        public Chroma66205(string resourceName) : base(resourceName)
        {
            Configuration = new _Configuration(this);
            Status = new _Status(this);
            
            if (IsCorrectInstrument())
            {
                _session = (MessageBasedSession)base._session;
                Initialize();
            }
            else
            {
                throw new Instruments.Exceptions.WrongInstrumentException("Expected session to Chroma 66205.");
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
        public Chroma66205(string resourceName, AccessModes accessModes, int timeoutMilliseconds) : base(resourceName, accessModes, timeoutMilliseconds)
        {
            Configuration = new _Configuration(this);
            Status = new _Status(this);

            if (IsCorrectInstrument())
            {
                _session = (MessageBasedSession)base._session;
                Initialize();
            }
            else
            {
                throw new Instruments.Exceptions.WrongInstrumentException("Expected session to Chroma 66205.");
            }
        }

        /// <summary>
        /// Checks if base._session leads to a Chroma 66205 Powermeter
        /// </summary>
        /// <returns>True if the check is positive</returns>
        protected override bool IsCorrectInstrument()
        {
            string response;
            try
            {
                MessageBasedSession testSession = (MessageBasedSession)base._session;
                testSession.FormattedIO.WriteLine("*IDN?");
                response = testSession.FormattedIO.ReadLine();
                if ((response.Split(',')[0] == "Chroma ATE") && ((response.Split(',')[1] == "66205")))
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
                throw new Instruments.Exceptions.CommunicationException("Getting Identification Information from Instrument failed!");
            }
        }

        /// <summary>
        /// Initialize the Chroma to Factory Defaults, set Transmit and Termination Characters.
        /// </summary>
        public void Initialize()
        {
            Reset();
            this.Configuration.RestoreConfig(Chroma66205._Configuration.ConfigurationStoreValue.FactoryDefaults);
            this.Configuration.System.TransmitSeperator.Value = Chroma66205._Configuration._System._TransmitSeperator.AllowedValue.Comma;
            this.Configuration.System.TransmitTerminator.Value = Chroma66205._Configuration._System._TransmitTerminator.AllowedValue.LineFeed;
        }

        /// <summary>
        /// This command performs device initial setting.
        /// </summary>
        private void Reset()
        {
            SendCommand("*RST", true);
        }

        /// <summary>
        /// Sends Command to the Chroma66205 and provides the Response as String and Clears the ReadBuffer if readResponse is true
        /// </summary>
        /// <param name="command">Command to be sent to the Chroma66205. Refer to Chroma66205 Manual for more information</param>
        /// <param name="readResponse">If true, the Response is provided as String and the ReadBuffer is cleared. If false, the return will be null and ReadBuffer will be untouched.</param>
        /// <returns></returns>
        private string SendCommand(string command, bool readResponse)
        {
            _session.FormattedIO.Write(command + this.Configuration.System.TransmitTerminator.Value.ToString());
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
        public override Dictionary<PowerMeters.MeasurementParameter, double> Measure(params PowerMeters.MeasurementParameter[] measurementParameters)
        {
            Dictionary<PowerMeters.MeasurementParameter, double> measurements = new Dictionary<PowerMeters.MeasurementParameter, double>();
            string message = "MEAS?";
            double[] values;


            foreach (PowerMeters.MeasurementParameter parameter in measurementParameters)
            {
                message = message + parameter.ToString + this.Configuration.System.TransmitSeperator.Value.ToString();
            }

            message = message + this.Configuration.System.TransmitTerminator.Value.ToString();

            SendCommand(message, false);

            values = _session.FormattedIO.ReadListOfDouble(measurementParameters.Length);

            int i = 0;
            foreach (PowerMeters.MeasurementParameter parameter in measurementParameters)
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
        public override Dictionary<PowerMeters.MeasurementParameter, double> Fetch(params PowerMeters.MeasurementParameter[] measurementParameters)
        {
            Dictionary<PowerMeters.MeasurementParameter, double> measurements = new Dictionary<PowerMeters.MeasurementParameter, double>();
            string message = "FETC?";
            double[] values;


            foreach (PowerMeters.MeasurementParameter parameter in measurementParameters)
            {
                message = message + parameter.ToString + this.Configuration.System.TransmitSeperator.ToString();
            }

            message = message + this.Configuration.System.TransmitTerminator.Value.ToString();

            SendCommand(message, false);

            values = _session.FormattedIO.ReadListOfDouble(measurementParameters.Length);

            int i = 0;
            foreach (PowerMeters.MeasurementParameter parameter in measurementParameters)
            {
                measurements.Add(parameter, values[i]);
                i++;
            }

            return measurements;
        }

        /// <summary>
        /// Contains all Configuration Settings of the Chroma 66205
        /// </summary>
        private class _Configuration
        {
            private Chroma66205 _chroma66205;

            public _Configuration(Chroma66205 chroma66205)
            {
                _chroma66205 = chroma66205;
                System = new _System(this);
            }

            /// <summary>
            /// This command stores the present state of the configuration in a specific memory location.
            /// </summary>
            public void SaveConfig(ConfigurationStoreValue config)
            {
                if(config > 0) _chroma66205.SendCommand($"*SAV {config}", true);
            }

            /// <summary>
            /// This command restores the power meter to a state that was previously stored in memory with the SaveConfig Command.
            /// </summary>
            public void RestoreConfig(ConfigurationStoreValue config)
            {
                _chroma66205.SendCommand($"*RCL {config}", true);
            }

            /// <summary>
            /// Values which are allowed as Configurations.
            /// </summary>
            public enum ConfigurationStoreValue
            {
                FactoryDefaults = 0,
                UserConfig1,
                UserConfig2,
                UserConfig3,
                UserConfig4,
                UserConfig5,
                UserConfig6,
                UserConfig7,
                UserConfig8,
                UserConfig9,
                UserConfig10
            }

            /// <summary>
            /// Contains all System Config Settings of the Chroma66205
            /// </summary>
            public class _System
            {
                private _Configuration _configuration;

                public _System(_Configuration configuration)
                {
                    _configuration = configuration;
                    Header = new _Header(this);
                    TransmitSeperator = new _TransmitSeperator(this);
                }

                /// <summary>
                /// This command turns response headers ON or OFF. The default is OFF.
                /// </summary>
                public class _Header
                {
                    private _System _system;

                    public _Header(_System system)
                    {
                        _system = system;
                    }

                    /// <summary>
                    /// Actual Value of the Propertie.
                    /// </summary>
                    protected AllowedValue _Value = null;
                    public AllowedValue Value
                    {
                        get
                        {
                            if (_Value == null)
                            {
                                switch (_system._configuration._chroma66205.SendCommand("SYST:HEAD?", true))
                                {
                                    case "ON":
                                        return AllowedValue.ON;
                                    case "OFF":
                                        return AllowedValue.OFF;
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
                            _system._configuration._chroma66205.SendCommand($"SYST:HEAD {value.ToString()}", true);
                            switch (_system._configuration._chroma66205.SendCommand("SYST:HEAD?", true))
                            {
                                case "ON":
                                    _Value = AllowedValue.ON;
                                    break;
                                case "OFF":
                                    _Value = AllowedValue.OFF;
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
                        /// <summary>
                        /// Turns Return Headers On
                        /// </summary>
                        public static readonly AllowedValue ON = new AllowedValue("ON");
                        /// <summary>
                        /// Turns Return Headers Off
                        /// </summary>
                        public static readonly AllowedValue OFF = new AllowedValue("OFF");
                    }
                }
                public _Header Header { get; }

                /// <summary>
                /// This command sets the message unit seperator for response messages.
                /// </summary>
                public class _TransmitSeperator
                {
                    private _System _system;

                    public _TransmitSeperator(_System system)
                    {
                        _system = system;
                    }

                    /// <summary>
                    /// Actual Value of the Propertie.
                    /// </summary>
                    protected AllowedValue _Value = null;
                    public AllowedValue Value
                    {
                        get
                        {
                            if (_Value == null)
                            {
                                switch (_system._configuration._chroma66205.SendCommand("SYST:TRAN:SEP?", true))
                                {
                                    case ",":
                                        return AllowedValue.Comma;
                                    case ";":
                                        return AllowedValue.Semicolon;
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
                            
                            switch (value.ToString())
                            {
                                case ",":
                                    _system._configuration._chroma66205.SendCommand($"SYST:TRAN:SEP ON", true);
                                    break;
                                case ";":
                                    _system._configuration._chroma66205.SendCommand($"SYST:TRAN:SEP OFF", true);
                                    break;
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                            switch (_system._configuration._chroma66205.SendCommand("SYST:TRAN:SEP?", true))
                            {
                                case ",":
                                    _Value = AllowedValue.Comma;
                                    break;
                                case ";":
                                    _Value = AllowedValue.Semicolon;
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

                        /// <summary>
                        /// Converts the AllowedValue of the TransmitSeperator to it´s respective String representation
                        /// </summary>
                        /// <returns>For Comma = "," for Semicolon = ";"</returns>
                        public new string ToString()
                        {
                            switch (_value)
                            {
                                case "0":
                                    return ",";
                                case "1":
                                    return ";";
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        private AllowedValue(string value)
                        {
                            _value = value;
                        }
                        /// <summary>
                        /// Set Transmit Seperator to Comma
                        /// </summary>
                        public static readonly AllowedValue Comma = new AllowedValue("0");
                        /// <summary>
                        /// Set Transmit Seperator to Semicolon
                        /// </summary>
                        public static readonly AllowedValue Semicolon = new AllowedValue("1");
                    }
                }
                public _TransmitSeperator TransmitSeperator { get; }

                /// <summary>
                /// This command sets the data terminator for response messages. The default is Linefeed.
                /// </summary>
                public class _TransmitTerminator
                {
                    /// <summary>
                    /// Actual Value of the Propertie.
                    /// </summary>
                    protected AllowedValue _Value = null;
                    public AllowedValue Value
                    {
                        get
                        {
                            throw new NotImplementedException();
                        }
                        set
                        {
                            throw new NotImplementedException();
                        }
                    }

                    /// <summary>
                    /// Allowed values for the Propertie.
                    /// </summary>
                    public class AllowedValue
                    {
                        private readonly string _value;

                        /// <summary>
                        /// Converts the AllowedValue of the TransmitTerminator to it´s respective String representation
                        /// </summary>
                        /// <returns>For LineFeed = "\n" for LF+CR = "\n\r"</returns>
                        public new string ToString()
                        {
                            switch (_value)
                            {
                                case "0":
                                    return "\n";
                                case "1":
                                    return "\n\r";
                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }

                        private AllowedValue(string value)
                        {
                            _value = value;
                        }
                        /// <summary>
                        /// Set Transmit Seperator to Comma
                        /// </summary>
                        public static readonly AllowedValue LineFeed = new AllowedValue("0");
                        /// <summary>
                        /// Set Transmit Seperator to Semicolon
                        /// </summary>
                        public static readonly AllowedValue LineFeedAndCarriageReturn = new AllowedValue("1");
                    }
                }
                public _TransmitTerminator TransmitTerminator { get; }

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
            public _System System { get; }
        }
        private _Configuration Configuration { get; }

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
        private new MessageBasedSession _session;
    }
}