namespace MeasurementControlCLI.Instruments.Chroma66205
{
    /// <summary>
    /// Measurement Parameters Unique to the Chroma 66205
    /// </summary>
    public class Measure_AllowedValue : Helper.AllowedValue
    {
        /// <summary>
        /// Positive Voltage Peak
        /// </summary>
        public static readonly Measure_AllowedValue VPKp = new Measure_AllowedValue("VPK+", "VPK+", "Positive Voltage Peak");
        /// <summary>
        /// Negative Voltage Peak
        /// </summary>
        public static readonly Measure_AllowedValue VPKn = new Measure_AllowedValue("VPK-", "VPK-", "Negative Voltage Peak");
        /// <summary>
        /// Total Harmonic Distortion of Voltage
        /// </summary>
        public static readonly Measure_AllowedValue THDV = new Measure_AllowedValue("THDV", "THDV", "Total Harmonic Distortion of Voltage");
        /// <summary>
        /// Positive Current Peak
        /// </summary>
        public static readonly Measure_AllowedValue IPKp = new Measure_AllowedValue("IPK+", "IPK+", "Positive Current Peak");
        /// <summary>
        /// Negative Current Peak
        /// </summary>
        public static readonly Measure_AllowedValue IPKn = new Measure_AllowedValue("IPK-", "IPK-", "Negative Current Peak");
        /// <summary>
        /// IS?
        /// </summary>
        public static readonly Measure_AllowedValue IS = new Measure_AllowedValue("IS", "IS", "");
        /// <summary>
        /// Crest Factor Current
        /// </summary>
        public static readonly Measure_AllowedValue CFI = new Measure_AllowedValue("CFI", "CFI", "Crest Factor Current");
        /// <summary>
        /// Total Harmonic Distortion of Current
        /// </summary>
        public static readonly Measure_AllowedValue THDI = new Measure_AllowedValue("THDI", "THDI", "Total Harmonic Distortion of Current");
        /// <summary>
        /// Power Factor
        /// </summary>
        public static readonly Measure_AllowedValue PF = new Measure_AllowedValue("PF", "PF", "Power Factor");
        /// <summary>
        /// Apparent Power
        /// </summary>
        public static readonly Measure_AllowedValue VA = new Measure_AllowedValue("VA", "VA", "Apparent Power");
        /// <summary>
        /// Reactive Power
        /// </summary>
        public static readonly Measure_AllowedValue VAR = new Measure_AllowedValue("VAR", "VAR", "Reactive Power");
        /// <summary>
        /// Watt-hour
        /// </summary>
        public static readonly Measure_AllowedValue WH = new Measure_AllowedValue("WH", "WH", "Watt-hour");
        /// <summary>
        /// Zero Crossing Frequency
        /// </summary>
        public static readonly Measure_AllowedValue FREQ = new Measure_AllowedValue("FREQ", "FREQ", "Frequency");
        /// <summary>
        /// Voltage DC Value
        /// </summary>
        public static readonly Measure_AllowedValue VDC = new Measure_AllowedValue("VDC", "VDC", "Voltage DC Value");
        /// <summary>
        /// Current DC Value
        /// </summary>
        public static readonly Measure_AllowedValue IDC = new Measure_AllowedValue("IDC", "IDC", "Current DC Value");
        /// <summary>
        /// Wattage DC Value
        /// </summary>
        public static readonly Measure_AllowedValue WDC = new Measure_AllowedValue("WDC", "WDC", "Wattage DC Value");
        /// <summary>
        /// Mean Voltage
        /// </summary>
        public static readonly Measure_AllowedValue VMEAN = new Measure_AllowedValue("VMEAN", "VMEAN", "Mean Voltage");
        /// <summary>
        /// Phase difference in degree
        /// </summary>
        public static readonly Measure_AllowedValue DEG = new Measure_AllowedValue("DEG", "DEG", "Phase difference in degree");
        /// <summary>
        /// Crest Factor Voltage
        /// </summary>
        public static readonly Measure_AllowedValue CFV = new Measure_AllowedValue("CFV", "CFV", "Crest Factor Voltage");
        /// <summary>
        /// Voltage Frequency
        /// </summary>
        public static readonly Measure_AllowedValue VHZ = new Measure_AllowedValue("VHZ", "VHZ", "Voltage Frequency");
        /// <summary>
        /// Current Frequency
        /// </summary>
        public static readonly Measure_AllowedValue IHZ = new Measure_AllowedValue("IHZ", "IHZ", "Current Frequency");
        /// <summary>
        /// Ampere Hours
        /// </summary>
        public static readonly Measure_AllowedValue AH = new Measure_AllowedValue("AH", "AH", "Ampere Hours");

        Measure_AllowedValue(string _MessageBasedSessionRepresentation, string _StringRepresentation, string _Description) : base(_MessageBasedSessionRepresentation, _StringRepresentation, _Description) { }
    }
}