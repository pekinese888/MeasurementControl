using NationalInstruments.Visa;
using Ivi.Visa;
using System;
using System.Collections.Generic;

namespace MeasurementControlCLI.Instruments.PowerMeters
{
    /// <summary>
    /// CLass for all Power Meters
    /// </summary>
    public abstract class PowerMeter : Instrument
    {
        public PowerMeter(string resourceName) : base(resourceName)
        {
        }

        public PowerMeter(string resourceName, AccessModes accessModes, int timeoutMilliseconds) : base(resourceName, accessModes, timeoutMilliseconds)
        {
        }

        protected abstract bool IsCorrectInstrument();

        public abstract Dictionary<string, double> Measure(params PowerMeter.MeasurementParameter[] measurementParameters);
        /// <summary>
        /// Measurement Parameters unique to Power Meters
        /// </summary>
        public new class MeasurementParameter : Instrument.MeasurementParameter
        {
            /// <summary>
            /// RMS Voltage
            /// </summary>
            public static readonly MeasurementParameter V = new MeasurementParameter("V", "RMS Voltage");
            /// <summary>
            /// RMS Current
            /// </summary>
            public static readonly MeasurementParameter I = new MeasurementParameter("I", "RMS Current");
            /// <summary>
            /// Active Power
            /// </summary>
            public static readonly MeasurementParameter W = new MeasurementParameter("W", "Active Power");

            protected MeasurementParameter(string toString, string description):base(toString,description){}
        }
    }
}