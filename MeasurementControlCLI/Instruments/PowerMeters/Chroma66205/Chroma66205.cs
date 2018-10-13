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
            if (IsCorrectInstrument())
            {
                _session = (MessageBasedSession)base._session;
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
            if (IsCorrectInstrument())
            {
                _session = (MessageBasedSession)base._session;
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
        /// This command performs device initial setting.
        /// </summary>
        public void Reset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This command requests execution of, and queries the result of self-test.
        /// </summary>
        public int RunSelfTest()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This command lets the user get measurement data from the Power Meter. Measure triggers the acquisition of new data before returning data.
        /// </summary>
        /// <param name="measurementParameters">Paremeters to be measured.</param>
        /// <returns>Dictionary which contains PowerMeter.MeasurementParameter as Key Value and the actual measurement value as value pair.</returns>
        public override Dictionary<string, double> Measure(params PowerMeters.MeasurementParameter[] measurementParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This command lets the user get measurement data from the Power Meter. Fetch returns the previously acquired data from the measurement buffer.
        /// </summary>
        /// <param name="measurementParameters">Paremeters to be measured.</param>
        /// <returns>Dictionary which contains PowerMeter.MeasurementParameter as Key Value and the actual measurement value as value pair.</returns>
        public override Dictionary<string, double> Fetch(params PowerMeters.MeasurementParameter[] measurementParameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Contains all Configuration Settings of the Chroma 66205
        /// </summary>
        public class _Configuration
        {
            /// <summary>
            /// This command stores the present state of the configuration in a specific memory location.
            /// </summary>
            public void SaveConfig(ConfigurationStoreValue config)
            {
                throw new NotImplementedException();
            }

            /// <summary>
            /// This command restores the power meter to a state that was previously stored in memory with the SaveConfig Command.
            /// </summary>
            public void RestoreConfig(ConfigurationStoreValue config)
            {
                throw new NotImplementedException();
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
            /// Contains all Register Based actions on the Chroma66205
            /// </summary>
            public class _Registers
            {
                /// <summary>
                /// This command clears the status byte register and the event registers
                /// </summary>
                public static void ClearStatusByteRegister()
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// This Property sets the standard event status enable register. 
                /// This command programs the Standard Event register bits. 
                /// If one or more of the enabled events of the Standard Event register is set, the ESB of Status Byte Register is set too.
                /// </summary>
                public static Byte EventStatusEnableRegister
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
                /// This command reads out the contents of the standard event status register (SESR)
                /// </summary>
                public static byte EventStatusRegister
                {
                    get
                    {
                        throw new NotImplementedException();
                    }
                }

                /// <summary>
                /// This command sets the service request enable register (SRER).
                /// </summary>
                public static byte ServiceRequestEnableRegister
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
                /// This command queries the status byte register.
                /// </summary>
                public static byte StatusByteRegister
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
            }
            public _Registers Registers { get; }

            /// <summary>
            /// Contains all System Config Settings of the Chroma66205
            /// </summary>
            public class _System
            {
                /// <summary>
                /// This command turns response headers ON or OFF. The default is OFF.
                /// </summary>
                public class _Header
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
                        private string _value;

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
                    /// <summary>
                    /// Actual Value of the Propertie.
                    /// </summary>
                    protected AllowedValue _Value = null;
                    protected AllowedValue Value
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
                        private string _value;

                        public new string ToString()
                        {
                            return _value;
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
                        private string _value;

                        public new string ToString()
                        {
                            return _value;
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
                public static void SetLocal()
                {
                    throw new NotImplementedException();
                }

                /// <summary>
                /// THis command sets the Power Meter in Remote State and the Front Panel will be disabled except for the SETUP Key pressed.
                /// </summary>
                public static void SetRemote()
                {
                    throw new NotImplementedException();
                }

            }
            public _System System { get; }
        }
        public _Configuration Configuration { get; }

        /// <summary>
        /// Contains all Status Informations of the Chroma 66205
        /// </summary>
        public struct _Status
        {
            /// <summary>
            /// This Command queries the Error Status of the Instrument.
            /// </summary>
            public string Error
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// This command queries manufacturer´s name, model name, serial number and firmware version
            /// </summary>
            public string Identification
            {
                get
                {
                    throw new NotImplementedException();
                }
            }

            /// <summary>
            /// This query returns a Floatingpoint Representation of the SCPI Version number for which the instrument complies.
            /// </summary>
            public double Version
            {
                get
                {
                    throw new NotImplementedException();
                }
            }
        }
        public _Status Status { get; }

        /// <summary>
        /// Internal representation of the Messagebased Visa Session
        /// </summary>
        public new MessageBasedSession _session;
    }
}