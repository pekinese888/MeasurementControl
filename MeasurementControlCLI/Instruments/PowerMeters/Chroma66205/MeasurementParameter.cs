namespace MeasurementControlCLI.Instruments.PowerMeters.Chroma66205
{
    /// <summary>
    /// Measurement Parameters Unique to the Chroma 66205
    /// </summary>
    public class MeasurementParameter : PowerMeters.MeasurementParameter
    {
        /// <summary>
        /// Positive Voltage Peak
        /// </summary>
        public static readonly MeasurementParameter VPKp = new MeasurementParameter("VPK+", "Positive Voltage Peak");
        /// <summary>
        /// Negative Voltage Peak
        /// </summary>
        public static readonly MeasurementParameter VPKn = new MeasurementParameter("VPK-", "Negative Voltage Peak");
        /// <summary>
        /// Total Harmonic Distortion of Voltage
        /// </summary>
        public static readonly MeasurementParameter THDV = new MeasurementParameter("THDV", "Total Harmonic Distortion of Voltage");
        /// <summary>
        /// Positive Current Peak
        /// </summary>
        public static readonly MeasurementParameter IPKp = new MeasurementParameter("IPK+", "Positive Current Peak");
        /// <summary>
        /// Negative Current Peak
        /// </summary>
        public static readonly MeasurementParameter IPKn = new MeasurementParameter("IPK-", "Negative Current Peak");
        /// <summary>
        /// IS?
        /// </summary>
        public static readonly MeasurementParameter IS = new MeasurementParameter("IS", "");
        /// <summary>
        /// Crest Factor Current
        /// </summary>
        public static readonly MeasurementParameter CFI = new MeasurementParameter("CFI", "Crest Factor Current");
        /// <summary>
        /// Total Harmonic Distortion of Current
        /// </summary>
        public static readonly MeasurementParameter THDI = new MeasurementParameter("THDI", "Total Harmonic Distortion of Current");
        /// <summary>
        /// Power Factor
        /// </summary>
        public static readonly MeasurementParameter PF = new MeasurementParameter("PF", "Power Factor");
        /// <summary>
        /// Apparent Power
        /// </summary>
        public static readonly MeasurementParameter VA = new MeasurementParameter("VA", "Apparent Power");
        /// <summary>
        /// Reactive Power
        /// </summary>
        public static readonly MeasurementParameter VAR = new MeasurementParameter("VAR", "Reactive Power");
        /// <summary>
        /// Watt-hour
        /// </summary>
        public static readonly MeasurementParameter WH = new MeasurementParameter("WH", "Watt-hour");
        /// <summary>
        /// Zero Crossing Frequency
        /// </summary>
        public static readonly MeasurementParameter FREQ = new MeasurementParameter("FREQ", "Frequency");
        /// <summary>
        /// Voltage DC Value
        /// </summary>
        public static readonly MeasurementParameter VDC = new MeasurementParameter("VDC", "Voltage DC Value");
        /// <summary>
        /// Current DC Value
        /// </summary>
        public static readonly MeasurementParameter IDC = new MeasurementParameter("IDC", "Current DC Value");
        /// <summary>
        /// Wattage DC Value
        /// </summary>
        public static readonly MeasurementParameter WDC = new MeasurementParameter("WDC", "Wattage DC Value");
        /// <summary>
        /// Mean Voltage
        /// </summary>
        public static readonly MeasurementParameter VMEAN = new MeasurementParameter("VMEAN", "Mean Voltage");
        /// <summary>
        /// Phase difference in degree
        /// </summary>
        public static readonly MeasurementParameter DEG = new MeasurementParameter("DEG", "Phase difference in degree");
        /// <summary>
        /// Crest Factor Voltage
        /// </summary>
        public static readonly MeasurementParameter CFV = new MeasurementParameter("CFV", "Crest Factor Voltage");
        /// <summary>
        /// Voltage Frequency
        /// </summary>
        public static readonly MeasurementParameter VHZ = new MeasurementParameter("VHZ", "Voltage Frequency");
        /// <summary>
        /// Current Frequency
        /// </summary>
        public static readonly MeasurementParameter IHZ = new MeasurementParameter("IHZ", "Current Frequency");
        /// <summary>
        /// Ampere Hours
        /// </summary>
        public static readonly MeasurementParameter AH = new MeasurementParameter("AH", "Ampere Hours");

        MeasurementParameter(string toString, string description) : base(toString, description) { }
    }
}