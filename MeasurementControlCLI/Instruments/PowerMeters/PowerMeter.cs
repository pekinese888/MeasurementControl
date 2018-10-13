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


        /// <summary>
        /// Checks 
        /// </summary>
        /// <returns>Returns True if the VISA Session Points to the correct Instrument</returns>
        protected abstract bool IsCorrectInstrument();

        /// <summary>
        /// This command lets the user get measurement data from the Power Meter. Measure triggers the acquisition of new data before returning data.
        /// </summary>
        /// <param name="measurementParameters">Paremeters to be measured.</param>
        /// <returns>Dictionary which contains PowerMeter.MeasurementParameter as Key Value and the actual measurement value as value pair.</returns>
        public abstract Dictionary<string, double> Measure(params MeasurementParameter[] measurementParameters);

        /// <summary>
        /// This command lets the user get measurement data from the Power Meter. Fetch returns the previously acquired data from the measurement buffer.
        /// </summary>
        /// <param name="measurementParameters">Paremeters to be measured.</param>
        /// <returns>Dictionary which contains PowerMeter.MeasurementParameter as Key Value and the actual measurement value as value pair.</returns>
        public abstract Dictionary<string, double> Fetch(params MeasurementParameter[] measurementParameters);
    }
}