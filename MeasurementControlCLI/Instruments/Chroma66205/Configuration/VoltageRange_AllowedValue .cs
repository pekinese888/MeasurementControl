namespace MeasurementControlCLI.Instruments.Chroma66205.Configuration
{
    public class VoltageRange_AllowedValue : Helper.AllowedValue
    {
        VoltageRange_AllowedValue(string _MessageBasedSessionRepresentation, string _StringRepresentation, string _Description) : base(_MessageBasedSessionRepresentation, _StringRepresentation, _Description){}

        /// <summary>
        /// AUTO Range
        /// </summary>
        public static readonly VoltageRange_AllowedValue AUTO = new VoltageRange_AllowedValue("AUTO", "AUTO", "AUTO Range");
        /// <summary>
        /// 600V Range
        /// </summary>
        public static readonly VoltageRange_AllowedValue V600 = new VoltageRange_AllowedValue("V600", "V600", "600V Range");
        /// <summary>
        /// 300V Range
        /// </summary>
        public static readonly VoltageRange_AllowedValue V300 = new VoltageRange_AllowedValue("V300", "V300", "300V Range");
        /// <summary>
        /// 150V Range
        /// </summary>
        public static readonly VoltageRange_AllowedValue V150 = new VoltageRange_AllowedValue("V150", "V150", "150V Range");
        /// <summary>
        /// 60V Range
        /// </summary>
        public static readonly VoltageRange_AllowedValue V60 = new VoltageRange_AllowedValue("V60", "V60", "60V Range");
        /// <summary>
        /// 30V Range
        /// </summary>
        public static readonly VoltageRange_AllowedValue V30 = new VoltageRange_AllowedValue("V30", "V30", "30V Range");
        /// <summary>
        /// 15V Range
        /// </summary>
        public static readonly VoltageRange_AllowedValue V15 = new VoltageRange_AllowedValue("V15", "V15", "15V Range");
    }
}
