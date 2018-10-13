namespace MeasurementControlCLI.Instruments.PowerMeters
{
    /// <summary>
    /// Measurement Parameters unique to Power Meters
    /// </summary>
    public class MeasurementParameter : Instruments.MeasurementParameter
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

        protected MeasurementParameter(string toString, string description) : base(toString, description) { }
    }
}