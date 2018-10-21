namespace MeasurementControlCLI.Instruments.Chroma66205
{
    /// <summary>
    /// Measurement Parameters Unique to the Chroma 66205
    /// </summary>
    public class Fetch_AllowedValue : Helper.AllowedValue
    {
        /// <summary>
        /// Positive Voltage Peak
        /// </summary>
        public static readonly Fetch_AllowedValue VPKp = new Fetch_AllowedValue("VPK+", "VPK+", "Positive Voltage Peak");
        /// <summary>
        /// Negative Voltage Peak
        /// </summary>
        public static readonly Fetch_AllowedValue VPKn = new Fetch_AllowedValue("VPK-", "VPK-", "Negative Voltage Peak");
        /// <summary>
        /// Total Harmonic Distortion of Voltage
        /// </summary>
        public static readonly Fetch_AllowedValue THDV = new Fetch_AllowedValue("THDV", "THDV", "Total Harmonic Distortion of Voltage");
        /// <summary>
        /// Positive Current Peak
        /// </summary>
        public static readonly Fetch_AllowedValue IPKp = new Fetch_AllowedValue("IPK+", "IPK+", "Positive Current Peak");
        /// <summary>
        /// Negative Current Peak
        /// </summary>
        public static readonly Fetch_AllowedValue IPKn = new Fetch_AllowedValue("IPK-", "IPK-", "Negative Current Peak");
        /// <summary>
        /// IS?
        /// </summary>
        public static readonly Fetch_AllowedValue IS = new Fetch_AllowedValue("IS", "IS", "");
        /// <summary>
        /// Crest Factor Current
        /// </summary>
        public static readonly Fetch_AllowedValue CFI = new Fetch_AllowedValue("CFI", "CFI", "Crest Factor Current");
        /// <summary>
        /// Total Harmonic Distortion of Current
        /// </summary>
        public static readonly Fetch_AllowedValue THDI = new Fetch_AllowedValue("THDI", "THDI", "Total Harmonic Distortion of Current");
        /// <summary>
        /// Power Factor
        /// </summary>
        public static readonly Fetch_AllowedValue PF = new Fetch_AllowedValue("PF", "PF", "Power Factor");
        /// <summary>
        /// Apparent Power
        /// </summary>
        public static readonly Fetch_AllowedValue VA = new Fetch_AllowedValue("VA", "VA", "Apparent Power");
        /// <summary>
        /// Reactive Power
        /// </summary>
        public static readonly Fetch_AllowedValue VAR = new Fetch_AllowedValue("VAR", "VAR", "Reactive Power");
        /// <summary>
        /// Watt-hour
        /// </summary>
        public static readonly Fetch_AllowedValue WH = new Fetch_AllowedValue("WH", "WH", "Watt-hour");
        /// <summary>
        /// Zero Crossing Frequency
        /// </summary>
        public static readonly Fetch_AllowedValue FREQ = new Fetch_AllowedValue("FREQ", "FREQ", "Frequency");
        /// <summary>
        /// Voltage DC Value
        /// </summary>
        public static readonly Fetch_AllowedValue VDC = new Fetch_AllowedValue("VDC", "VDC", "Voltage DC Value");
        /// <summary>
        /// Current DC Value
        /// </summary>
        public static readonly Fetch_AllowedValue IDC = new Fetch_AllowedValue("IDC", "IDC", "Current DC Value");
        /// <summary>
        /// Wattage DC Value
        /// </summary>
        public static readonly Fetch_AllowedValue WDC = new Fetch_AllowedValue("WDC", "WDC", "Wattage DC Value");
        /// <summary>
        /// Mean Voltage
        /// </summary>
        public static readonly Fetch_AllowedValue VMEAN = new Fetch_AllowedValue("VMEAN", "VMEAN", "Mean Voltage");
        /// <summary>
        /// Phase difference in degree
        /// </summary>
        public static readonly Fetch_AllowedValue DEG = new Fetch_AllowedValue("DEG", "DEG", "Phase difference in degree");
        /// <summary>
        /// Crest Factor Voltage
        /// </summary>
        public static readonly Fetch_AllowedValue CFV = new Fetch_AllowedValue("CFV", "CFV", "Crest Factor Voltage");
        /// <summary>
        /// Voltage Frequency
        /// </summary>
        public static readonly Fetch_AllowedValue VHZ = new Fetch_AllowedValue("VHZ", "VHZ", "Voltage Frequency");
        /// <summary>
        /// Current Frequency
        /// </summary>
        public static readonly Fetch_AllowedValue IHZ = new Fetch_AllowedValue("IHZ", "IHZ", "Current Frequency");
        /// <summary>
        /// Ampere Hours
        /// </summary>
        public static readonly Fetch_AllowedValue AH = new Fetch_AllowedValue("AH","AH", "Ampere Hours");

        Fetch_AllowedValue(string _MessageBasedSessionRepresentation, string _StringRepresentation, string _Description) : base(_MessageBasedSessionRepresentation, _StringRepresentation, _Description) { }
    }
}